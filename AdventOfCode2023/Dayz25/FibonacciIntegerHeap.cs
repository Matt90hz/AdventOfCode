using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz25;
internal class FibonacciIntegerHeap<TKey>
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
        public int Priority { get; internal set; }

        internal Node(TKey key, int priority)
        {
            Key = key;
            Priority = priority;
            Left = this;
            Right = this;
        }
    }

    private Node _minNode;
    private readonly Dictionary<TKey, Node> _nodes;

    public bool IsEmpty => _minNode is null;

    public int Count => _nodes.Count;

    public IReadOnlyDictionary<TKey, Node> Nodes => _nodes;

    public int MinimumPriority => _minNode.Priority;

    public TKey MinimumKey => _minNode.Key;

    public FibonacciIntegerHeap(IEnumerable<TKey> keys, int capacity = 0)
    {
        _nodes = new Dictionary<TKey, Node>(capacity);

        var enumerator = keys.GetEnumerator();

        enumerator.MoveNext();
        _minNode = new Node(enumerator.Current, 0);
        _nodes.Add(_minNode.Key, _minNode);

        while (enumerator.MoveNext())
        {
            // splice node into root list
            Node node = new(enumerator.Current, 0)
            {
                Left = _minNode,
                Right = _minNode.Right
            };

            _minNode.Right.Left = node;
            _minNode.Right = node;
            _nodes.Add(node.Key, node);
        }
    }

    public Node Insert(TKey key, int priority)
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

            if (priority < _minNode.Priority) _minNode = node;
        }

        _nodes.Add(node.Key, node);

        return node;
    }

    public Node ExtractMin()
    {
        var z = _minNode;

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
                child.Right = _minNode.Right;
                _minNode.Right.Left = child;
                _minNode.Right = child;

                child = next;
            }
            while (child != start);
        }

        // remove z from root list
        z.Left.Right = z.Right;
        z.Right.Left = z.Left;

        _minNode = z.Right;
        Consolidate();

        _nodes.Remove(z.Key);

        return z;
    }

    public void DecreaseKey(Node x, int newPriority)
    {
        //if (_comparer.Compare(newPriority, x.Priority) > 0)
        //    throw new ArgumentException("New priority must be ≤ current priority.");

        x.Priority = newPriority;
        var y = x.Parent;

        if (y is not null && x.Priority < y.Priority)
        {
            Cut(x, y);
            CascadingCut(y);
        }

        if (x.Priority < _minNode.Priority) _minNode = x;
    }

    public void Delete(Node x)
    {
        // force x to min then extract
        DecreaseKey(x, MinimumPriority);
        ExtractMin();
    }

    private void Consolidate()
    {
        int maxDeg = (int)Math.Log(Count, 2) + 1;
        var A = new Node?[maxDeg + 1];

        // snapshot of root list
        var roots = new List<Node>();
        var w = _minNode;
        do
        {
            roots.Add(w);
            w = w.Right;
        } 
        while (w != _minNode);

        for (var i = 0; i < roots.Count; i++)
        {
            var x = roots[i];
            int d = x.Degree;
            while (A[d] is Node y)
            {
                if (x.Priority > y.Priority) Swap(ref x, ref y);

                Link(y, x);
                A[d] = null;
                d++;
            }

            A[d] = x;
        }

        // rebuild root list, find new min
        _minNode = null!;
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

                if (node.Priority < _minNode.Priority) _minNode = node;
            }
        }
    }

    private static void Swap(ref Node a, ref Node b)
    {
        (b, a) = (a, b);
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
            if (y.Child == x) y.Child = x.Right;
        }

        y.Degree--;

        // add x to root list
        x.Left = _minNode;
        x.Right = _minNode.Right;
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

}
