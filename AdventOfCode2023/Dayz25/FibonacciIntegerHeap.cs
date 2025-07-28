using System.Buffers;
using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode2023.Dayz25;
internal class FibonacciIntegerHeap<TKey> where TKey : notnull
{
    public sealed class Node
    {
        internal Node? _parent;
        internal Node? _child;
        internal Node _left;
        internal Node _right;
        internal int _degree;
        internal bool _mark;

        public TKey Key { get; internal set; }
        public int Priority { get; internal set; }

        internal Node(TKey key, int priority)
        {
            Key = key;
            Priority = priority;
            _left = this;
            _right = this;
        }
    }

    private Node _minNode;
    private Node[] _degreeTable = [];
    private Node[] _roots = [];
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
                _left = _minNode,
                _right = _minNode._right
            };

            _minNode._right._left = node;
            _minNode._right = node;
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
            node._left = _minNode;
            node._right = _minNode._right;
            _minNode._right._left = node;
            _minNode._right = node;

            if (priority < _minNode.Priority) _minNode = node;
        }

        _nodes.Add(node.Key, node);

        return node;
    }

    public Node ExtractMin()
    {
        var z = _minNode;

        // bring children of z to root list
        if (z._child is Node child)
        {
            var start = child;
            do
            {
                var next = child._right;
                child._parent = null;

                // splice into root list
                child._left = _minNode;
                child._right = _minNode._right;
                _minNode._right._left = child;
                _minNode._right = child;

                child = next;
            }
            while (child != start);
        }

        // remove z from root list
        z._left._right = z._right;
        z._right._left = z._left;

        _minNode = z._right;
        Consolidate();

        _nodes.Remove(z.Key);

        return z;
    }

    public void DecreaseKey(Node x, int newPriority)
    {
        //if (_comparer.Compare(newPriority, x.Priority) > 0)
        //    throw new ArgumentException("New priority must be ≤ current priority.");

        x.Priority = newPriority;
        var y = x._parent;

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
        var w = _minNode;
        int rootsCount = 0;

        do
        {
            w = w._right;
            rootsCount++;
        }
        while (w != _minNode);

        if (_degreeTable.Length < rootsCount) Array.Resize(ref _roots, rootsCount);
        var roots = _roots;
        w = _minNode;

        for (var i = 0; i < rootsCount; i++)
        {
            roots[i] = w;
            w = w._right;
        }

        int maxDeg = BitOperations.Log2((uint)Count) + 2;
        if (_degreeTable.Length < maxDeg) Array.Resize(ref _degreeTable, maxDeg);
        var degreeTable = _degreeTable;

        for (var i = 0; i < rootsCount; i++)
        {
            var x = roots[i];
            int d = x._degree;

            while (degreeTable[d] is Node y)
            {
                if (x.Priority > y.Priority) (x, y) = (y, x);

                Link(y, x);
                degreeTable[d++] = null!;
            }

            degreeTable[d] = x;
        }

        // rebuild root list, find new min
        //RebuildRootListFrom(degreeTable, maxDeg);
        int aCount = 0;

        for (int i = 0; i < maxDeg; i++)
        {
            var n = degreeTable[i];
            if (n is null) continue;
            degreeTable[i] = null!;
            degreeTable[aCount++] = n;
        }

        _minNode = degreeTable[0];
        _minNode._left = _minNode;
        _minNode._right = _minNode;
        degreeTable[0] = null!;

        for (int i = 1; i < aCount; i++)
        {
            var node = degreeTable[i];

            node._left = _minNode;
            node._right = _minNode._right;
            _minNode._right._left = node;
            _minNode._right = node;

            if (node.Priority < _minNode.Priority) _minNode = node;

            degreeTable[i] = null!;
        }
    }

    private void RebuildRootListFrom(Node[] A, int maxDeg)
    {
        Node newMin = null;
        for (int i = 0; i < maxDeg; i++)
        {
            Node n = A[i];
            if (n == null) continue;
            A[i] = null;            

            if (newMin == null)
            {
                newMin = n;
                n._left = n._right = n;
            }
            else
            {
                n._right = newMin._right;
                n._left = newMin;
                newMin._right._left = n;
                newMin._right = n;

                if (n.Priority < newMin.Priority)
                    newMin = n;
            }
        }

        _minNode = newMin;
    }

    private static void Link(Node child, Node parent)
    {
        // remove child from root list
        child._left._right = child._right;
        child._right._left = child._left;

        child._parent = parent;
        child._mark = false;

        if (parent._child is null)
        {
            parent._child = child;
            child._left = child;
            child._right = child;
        }
        else
        {
            child._left = parent._child;
            child._right = parent._child._right;
            parent._child._right._left = child;
            parent._child._right = child;
        }

        parent._degree++;
    }

    private void Cut(Node x, Node y)
    {
        // remove x from y's child list
        if (x._right == x)
        {
            y._child = null;
        }
        else
        {
            x._left._right = x._right;
            x._right._left = x._left;
            if (y._child == x) y._child = x._right;
        }

        y._degree--;

        // add x to root list
        x._left = _minNode;
        x._right = _minNode._right;
        _minNode._right._left = x;
        _minNode._right = x;

        x._parent = null;
        x._mark = false;
    }

    private void CascadingCut(Node y)
    {
        var z = y._parent;

        if (z is null) return;

        if (y._mark)
        {
            Cut(y, z);
            CascadingCut(z);
        }
        else
        {
            y._mark = true;
        }
    }
}
