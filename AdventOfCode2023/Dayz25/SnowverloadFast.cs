using AdventOfCode2023.Dayz20;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PommaLabs.Hippie;

namespace AdventOfCode2023.Dayz25;

internal static class SnowverloadFast
{
    public static int GroupSize(string input)
    {
        var graph = Parser.GetGraph(input);

        var partition = graph.MinimumGlobalCut();

        return partition.Left.Sum(vertex => vertex.Values.Count) * partition.Right.Sum(vertex => vertex.Values.Count);
    }
}

sealed record Partition<T>(Graph<T> Graph, List<Vertex<T>> Left, List<Vertex<T>> Right)
{
    public override string ToString() => this.ToFriendlyString();
}

sealed record Edge<T>(Vertex<T> Source, Vertex<T> Target, int Weight)
{
    internal bool IsInvolved(Vertex<T> vertex) => Source.Id == vertex.Id || Target.Id == vertex.Id;

    internal bool IsBetween(Vertex<T> v1, Vertex<T> v2) => (Source.Id == v1.Id && Target.Id == v2.Id) || (Target.Id == v1.Id && Source.Id == v2.Id);

    public override string ToString() => this.ToFriendlyString();
}

sealed record Vertex<T>(Guid Id, List<T> Values)
{
    public override string ToString() => this.ToFriendlyString();
}

sealed record Graph<T>(List<Vertex<T>> Vertices, List<Edge<T>> Edges)
{
    readonly Dictionary<Guid, Vertex<T>> _vertexDictionary = Vertices.ToDictionary(vertex => vertex.Id);

    public Vertex<T> this[Guid id] => _vertexDictionary[id];

    public override string ToString() => this.ToFriendlyString();
}

internal static class Merger
{
    internal static Graph<T> MergeVertex<T>(this Graph<T> graph, Vertex<T> source, Vertex<T> target)
    {
        var (vertices, edges) = graph;

        var newVertex = new Vertex<T>(Guid.NewGuid(), source.Values.Concat(target.Values).ToList());

        vertices.Add(newVertex);
        vertices.Remove(source);
        vertices.Remove(target);

        var sourceId = source.Id;
        var targetId = target.Id;

        var newEdges = new List<Edge<T>>();
        var edgesToAdd = new Dictionary<Guid, int>();

        foreach(var edge in edges)
        {
            var ((edgeSource, _), (edgeTarget, _), weight) = edge;

            bool isSource = edgeSource == sourceId || edgeSource == targetId;
            bool isTarget = edgeTarget == targetId || edgeTarget == sourceId;

            if (isTarget is false && isSource is false)
            {
                newEdges.Add(edge);
                continue;
            }

            if (isSource && isTarget) continue;     

            var (idToAdd, weightToAdd) = isSource 
                ? (edgeTarget, weight)
                : (edgeSource, weight);

            if (edgesToAdd.TryAdd(idToAdd, weightToAdd)) continue;

            var newWeight = edgesToAdd[idToAdd] + weightToAdd;

            edgesToAdd[idToAdd] = newWeight;
        }

        foreach(var (id, weight) in edgesToAdd)
        {
            newEdges.Add(new(newVertex, graph[id], weight));
        }

        var newGraph = new Graph<T>(vertices, newEdges);

        return newGraph;
    }
}

internal static class Connector
{
    internal static UniqueHeap<Vertex<T>, int> GetHeap<T>(this Partition<T> partition)
    {
        var heap = HeapFactory.NewFibonacciHeap<Vertex<T>, int>(Comparer<int>.Create((x, y) => (-x).CompareTo(-y)));

        var (graph, left, right) = partition;

        foreach(var vertex in right)
        {
            heap.Add(vertex, 0);
        }

        var leftVertexId = left[0].Id;

        foreach(var (source, target, weight) in graph.Edges)
        {
            bool isTargetLeft = leftVertexId == target.Id;
            bool isSourceLeft = leftVertexId == source.Id;

            if (isTargetLeft)
            {
                heap.UpdatePriorityOf(source, weight);
            }
            else if (isSourceLeft)
            {
                heap.UpdatePriorityOf(target, weight);
            }
        }

        return heap;
    }

    internal static Dictionary<Guid, List<(Vertex<T> Vertex, int Weight)>> GetConnections<T>(this Graph<T> graph)
    {
        var connections = new Dictionary<Guid, List<(Vertex<T> Vertex, int Weight)>>();

        foreach (var (target, source, weight) in graph.Edges)
        {
            var (targetId, sourceId) = (target.Id, source.Id);

            connections.TryAdd(sourceId, new());
            connections[sourceId].Add((target, weight));

            connections.TryAdd(targetId, new());
            connections[targetId].Add((source, weight));
        }

        return connections;
    }
}

internal static class Weighter
{
    internal static int Weight<T>(this Partition<T> partition)
    {
        var connections = partition.Graph.GetConnections();

        var weight = partition.Right
            .SelectMany(x => connections[x.Id])
            .Where(x => partition.Left.Contains(x.Vertex))
            .Sum(x => x.Weight);

        return weight;
    }
}

internal static class Visualizer
{
    internal static string ToFriendlyString<T>(this Graph<T> graph)
    {
        StringBuilder builder = new();

        foreach (var (key, connections) in graph.GetConnections())
        {
            builder.Append('[');
            foreach (var value in graph[key].Values)
            {
                builder.Append(' ');
                builder.Append(value);
                builder.Append(' ');
            }

            builder.Append("] -> [");

            foreach (var conn in connections)
            {
                builder.Append(" ([");

                foreach (var value in conn.Vertex.Values)
                {
                    builder.Append(' ');
                    builder.Append(value);
                    builder.Append(' ');
                }

                builder.Append("] - ");
                builder.Append(conn.Weight);
                builder.Append(')');
                builder.Append(' ');
            }
            builder.Append(']');
            builder.AppendLine();
        }

        return builder.ToString();
    }

    internal static string ToFriendlyString<T>(this Partition<T> partition)
    {
        var sb = new StringBuilder();

        sb.Append('[');

        var leftValues = partition.Left.SelectMany(x => x.Values);

        sb.AppendJoin(' ', leftValues);
        sb.Append(']');
        sb.Append("->");
        sb.Append('[');

        var rightValues = partition.Right.SelectMany(x => x.Values);

        sb.AppendJoin(' ', rightValues);
        sb.Append(']');
        sb.Append('=');
        sb.Append(partition.Weight());

        return sb.ToString();
    }

    internal static string ToFriendlyString<T>(this Vertex<T> vertex)
    {
        var sb = new StringBuilder();
        sb.Append('[');
        sb.AppendJoin(' ', vertex.Values);
        sb.Append(']');

        return sb.ToString();
    }

    internal static string ToFriendlyString<T>(this Edge<T> edge)
    {
        var sb = new StringBuilder();
        sb.Append(edge.Weight);
        sb.Append(" = ");
        sb.Append(edge.Source.ToFriendlyString());
        sb.Append(" -> ");
        sb.Append(edge.Target.ToFriendlyString());

        return sb.ToString();
    }
}

internal static class Parser
{
    internal static Graph<string> GetGraph(string input)
    {
        var connections = Snowverload.GetConnections(input);

        var vertices = connections
            .Values
            .SelectMany(x => x)
            .Concat(connections.Keys)
            .Distinct()
            .Select(key => new Vertex<string>(Guid.NewGuid(), new() { key }))
            .ToList();

        var edges = connections.Keys
            .SelectMany(key => connections[key]
                .Select(conn =>
                {
                    var source = vertices.First(x => x.Values[0] == key);
                    var target = vertices.First(x => x.Values[0] == conn);

                    return new Edge<string>(source, target, 1);
                }))
            .ToList();

        var graph = new Graph<string>(vertices, edges);

        return graph;
    }
}

internal static class Cutter
{
    internal sealed record PhaseCut<U>(Partition<U> Partition, Vertex<U> S, Vertex<U> T);

    internal static Partition<T> MinimumGlobalCut<T>(this Graph<T> graph)
    {
        List<Partition<T>> partitions = new();

        while (graph.Vertices.Count > 1)
        {
            var (partition, s, t) = MinimumCutPhase(graph);

            partitions.Add(partition);

            graph = graph.MergeVertex(s, t);
        }

        var min = partitions.MinBy(partition => partition.Weight());

        return min!;
    }

    internal static PhaseCut<T> MinimumCutPhase<T>(this Graph<T> graph)
    {
        List<Vertex<T>> A = graph.Vertices
            .Take(1)
            .ToList();

        List<Vertex<T>> V = graph.Vertices
            .Skip(1)
            .ToList();

        var partition = new Partition<T>(graph, A, V);
        var heap = partition.GetHeap();
        var connections = graph.GetConnections();

        Vertex<T> s = A[0];
        Vertex<T> t = heap.RemoveMin().Value;

        while (V.Count > 1)
        {
            partition.Right.Remove(t);
            partition.Left.Add(t);

            foreach (var (v, p) in connections[t.Id])
            {
                if (heap.Contains(v) is false) continue;

                var newPriority = heap.PriorityOf(v) + p;
                heap.UpdatePriorityOf(v, newPriority);
            }

            s = t;
            t = heap.RemoveMin().Value;
        }

        return new(partition, s, t);
    }
}

