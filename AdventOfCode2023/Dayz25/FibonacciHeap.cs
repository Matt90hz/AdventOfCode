using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz25;
public sealed class FibonacciHeap<TKey, TPriority>(IComparer<TPriority>? comparer = null)
    where TPriority : IComparable<TPriority>
    where TKey : notnull
{
    public sealed class Node
    {
        internal Node? Parent;
        internal Node? Child;
        internal Node Left;
        internal Node Right;
        internal int Degree;
        internal bool Mark;

        public TKey Key { get; internal set; }
        public TPriority Priority { get; internal set; }

        internal Node(TKey key, TPriority priority)
        {
            Key = key;
            Priority = priority;
            Left = this;
            Right = this;
        }
    }

    private Node? _minNode;
    private int _count;
    private readonly IComparer<TPriority> _comparer = comparer ?? Comparer<TPriority>.Default;
    private readonly Dictionary<TKey, Node> _nodes = [];

    public bool IsEmpty => _minNode is null;

    public int Count => _count;

    public IReadOnlyDictionary<TKey, Node> Nodes => _nodes;

    public TPriority MinimumPriority
    {
        get
        {
            if (_minNode is null)
                throw new InvalidOperationException("Heap is empty.");
            return _minNode.Priority;
        }
    }

    public TKey MinimumKey
    {
        get
        {
            if (_minNode is null)
                throw new InvalidOperationException("Heap is empty.");
            return _minNode.Key;
        }
    }

    public Node Insert(TKey key, TPriority priority)
    {
        var node = new Node(key, priority);

        if (_minNode is null)
        {
            _minNode = node;
        }
        else
        {
            // splice node into root list
            node.Left = _minNode;
            node.Right = _minNode.Right;
            _minNode.Right.Left = node;
            _minNode.Right = node;

            if (_comparer.Compare(priority, _minNode.Priority) < 0)
                _minNode = node;
        }

        _count++;
        _nodes.Add(node.Key, node);

        return node;
    }

    public Node ExtractMin()
    {
        var z = _minNode ?? throw new InvalidOperationException("Heap is empty.");

        // bring children of z to root list
        if (z.Child is Node child)
        {
            var start = child;
            do
            {
                var next = child.Right;
                child.Parent = null;

                // splice into root list
                child.Left = _minNode;
                child.Right = _minNode!.Right;
                _minNode.Right.Left = child;
                _minNode.Right = child;

                child = next;
            } while (child != start);
        }

        // remove z from root list
        z.Left.Right = z.Right;
        z.Right.Left = z.Left;

        if (z == z.Right)
        {
            _minNode = null;
        }
        else
        {
            _minNode = z.Right;
            Consolidate();
        }

        _count--;

        _nodes.Remove(z.Key);

        return z;
    }

    public void DecreaseKey(Node x, TPriority newPriority)
    {
        if (_comparer.Compare(newPriority, x.Priority) > 0)
            throw new ArgumentException("New priority must be ≤ current priority.");

        x.Priority = newPriority;
        var y = x.Parent;

        if (y is not null && _comparer.Compare(x.Priority, y.Priority) < 0)
        {
            Cut(x, y);
            CascadingCut(y);
        }

        if (_comparer.Compare(x.Priority, _minNode!.Priority) < 0)
            _minNode = x;
    }

    public void Delete(Node x)
    {
        // force x to min then extract
        DecreaseKey(x, MinimumPriority);
        ExtractMin();
    }

    private void Consolidate()
    {
        int maxDeg = (int)Math.Log(_count, 2) + 1;
        var A = new Node?[maxDeg + 1];

        // snapshot of root list
        var roots = new List<Node>();
        var w = _minNode!;
        do
        {
            roots.Add(w);
            w = w.Right;
        } while (w != _minNode);

        for (var i = 0; i < roots.Count; i++)
        {
            var x = roots[i];
            int d = x.Degree;
            while (A[d] is Node y)
            {
                if (_comparer.Compare(x.Priority, y.Priority) > 0)
                    Swap(ref x, ref y);

                Link(y, x);
                A[d] = null;
                d++;
            }

            A[d] = x;
        }

        // rebuild root list, find new min
        _minNode = null;
        foreach (var node in A)
        {
            if (node is null) continue;

            if (_minNode is null)
            {
                node.Left = node;
                node.Right = node;
                _minNode = node;
            }
            else
            {
                node.Left = _minNode;
                node.Right = _minNode.Right;
                _minNode.Right.Left = node;
                _minNode.Right = node;

                if (_comparer.Compare(node.Priority, _minNode.Priority) < 0)
                    _minNode = node;
            }
        }
    }

    private static void Swap(ref Node a, ref Node b)
    {
        var tmp = a;
        a = b;
        b = tmp;
    }

    private static void Link(Node child, Node parent)
    {
        // remove child from root list
        child.Left.Right = child.Right;
        child.Right.Left = child.Left;

        child.Parent = parent;
        child.Mark = false;

        if (parent.Child is null)
        {
            parent.Child = child;
            child.Left = child;
            child.Right = child;
        }
        else
        {
            child.Left = parent.Child;
            child.Right = parent.Child.Right;
            parent.Child.Right.Left = child;
            parent.Child.Right = child;
        }

        parent.Degree++;
    }

    private void Cut(Node x, Node y)
    {
        // remove x from y's child list
        if (x.Right == x)
        {
            y.Child = null;
        }
        else
        {
            x.Left.Right = x.Right;
            x.Right.Left = x.Left;
            if (y.Child == x)
                y.Child = x.Right;
        }

        y.Degree--;

        // add x to root list
        x.Left = _minNode!;
        x.Right = _minNode!.Right;
        _minNode.Right.Left = x;
        _minNode.Right = x;

        x.Parent = null;
        x.Mark = false;
    }

    private void CascadingCut(Node y)
    {
        var z = y.Parent;

        if (z is null) return;

        if (!y.Mark)
        {
            y.Mark = true;
        }
        else
        {
            Cut(y, z);
            CascadingCut(z);
        }
    }

    public static FibonacciHeap<TKey, TPriority> Merge(
        FibonacciHeap<TKey, TPriority> h1,
        FibonacciHeap<TKey, TPriority> h2)
    {
        if (h1._minNode is null) return h2;
        if (h2._minNode is null) return h1;

        // splice root lists
        var n1 = h1._minNode;
        var n2 = h2._minNode;
        var t = n1.Right;

        n1.Right = n2.Right;
        n2.Right.Left = n1;
        n2.Right = t;
        t.Left = n2;

        if (h2._comparer.Compare(n2.Priority, n1.Priority) < 0)
            h1._minNode = n2;

        h1._count += h2._count;
        return h1;
    }
}