using AdventOfCode2023.Dayz20;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCode2023.Dayz25;

sealed record Partition<T>(Graph<T> Graph, List<Vertex<T>> Left, List<Vertex<T>> Right, List<Edge<T>> Connections)
{
    public int Weight() => Connections.Sum(edge => edge.Weight);

    public override string ToString() => this.ToFriendlyString();
}

readonly record struct Edge<T>(Vertex<T> Source, Vertex<T> Target, int Weight)
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
    internal static Partition<T> MoveToLeft<T>(this Partition<T> partition, Vertex<T> vertex)
    {
        if (partition.Right.Contains(vertex) is false)
        {
            Debug.WriteLine($"Right does not contain {vertex.ToFriendlyString()}");
            return partition;
        }

        partition.Right.Remove(vertex);
        partition.Left.Add(vertex);

        partition.Connections.Clear();
        partition.Connections.AddRange(partition.Graph.GetConnections(partition.Left, partition.Right));

        return partition;
    }

    internal static Graph<T> MergeVertex<T>(this Graph<T> graph, Vertex<T> source, Vertex<T> target)
    {
        var (vertices, edges) = graph;

        vertices = new(vertices);
        edges = new(edges);

        var vertex = new Vertex<T>(Guid.NewGuid(), source.Values.Concat(target.Values).ToList());

        vertices.Add(vertex);
        vertices.Remove(source);
        vertices.Remove(target);

        var sourceId = source.Id;
        var targetId = target.Id;

        var edgesToFix = edges
            .Where(edge => edge.IsInvolved(source) || edge.IsInvolved(target))
            .ToArray();

        var edgesToAdd = new List<Edge<T>>();

        foreach (var edge in edgesToFix)
        {
            edges.Remove(edge);

            if (edge.IsBetween(source, target)) continue;

            var toAdd = edge switch
            {
                var e when e.IsInvolved(source) && e.Source.Id == sourceId => e with { Source = vertex },
                var e when e.IsInvolved(source) && e.Target.Id == sourceId => e with { Source = vertex, Target = e.Source },
                var e when e.IsInvolved(target) && e.Source.Id == targetId => e with { Source = vertex },
                var e when e.IsInvolved(target) && e.Target.Id == targetId => e with { Source = vertex, Target = e.Source },
                _ => new Edge<T>(),
            };

            if (toAdd.Source.Id == Guid.Empty)
            {
                Debug.WriteLine("I did not expect toAdd to be null");
                continue;
            }

            edgesToAdd.Add(toAdd);
        }

        var mergeEdges = edgesToAdd
            .GroupBy(edge => edge.Target)
            .Select(group => group.Count() > 1 ? new Edge<T>(vertex, group.Key, group.Sum(e => e.Weight)) : group.First());

        edges.AddRange(mergeEdges);

        var newGraph = new Graph<T>(vertices, edges);

        return newGraph;
    }
}

internal static class Connector
{
    internal static List<Edge<T>> GetConnections<T>(this Graph<T> graph, List<Vertex<T>> v1, List<Vertex<T>> v2)
    {
        var vertices = v1.Count < v2.Count ? v1 : v2;
        var edges = graph.Edges
            .Where(edge =>
            {
                var x = vertices.Contains(edge.Target);
                var y = vertices.Contains(edge.Source);

                return (x || y) && (x && y) is false;
            })
            .ToList();

        return edges;
    }

    internal static List<(Vertex<T> Vertex, int Weight)> GetConnectedVertices<T>(this Graph<T> graph, Vertex<T> vertex) => graph.Edges
        .Where(edge => edge.IsInvolved(vertex))
        .Select(edge => edge.Source.Id == vertex.Id ? (Vertex: edge.Target, edge.Weight) : (Vertex: edge.Source, edge.Weight))
        .Select(conn => (vertex, conn.Weight))
        .ToList();

    internal static List<Edge<T>> GetEdges<T>(this Graph<T> graph, Vertex<T> vertex) => graph.Edges
        .Where(edge => edge.IsInvolved(vertex))
        .ToList();

    internal static bool AreConnected<T>(this Graph<T> graph, Vertex<T> v1, Vertex<T> v2) => graph.Edges.Any(edge => edge.IsBetween(v1, v2));
}

internal static class Visualizer
{
    internal static string ToFriendlyString<T>(this Graph<T> graph)
    {
        StringBuilder builder = new();

        foreach (var vertex in graph.Vertices)
        {
            builder.Append('[');
            foreach (var value in vertex.Values)
            {
                builder.Append(' ');
                builder.Append(value);
                builder.Append(' ');
            }

            builder.Append("] -> [");

            foreach (var conn in graph.GetConnectedVertices(vertex))
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

        var remaningKeys = connections.Values
            .SelectMany(val => val)
            .Where(val => connections.ContainsKey(val) is false)
            .Distinct();

        var allKeys = connections.Keys
            .Concat(remaningKeys);

        var vertices = allKeys
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

        return new Graph<string>(vertices, edges);
    }
}

internal static class Cutter
{
    internal sealed record PhaseCut<U>(Partition<U> Partition, Vertex<U> S, Vertex<U> T);

    internal static Partition<T> MinimumGlobalCut<T>(this Graph<T> graph)
    {
        List<Partition<T>> partitions = new();
        Graph<T> mergedGraph = graph;

        while (mergedGraph.Vertices.Count > 1)
        {
            var (partition, s, t) = MinimumCutPhase(mergedGraph);

            partitions.Add(partition);

            mergedGraph = mergedGraph.MergeVertex(s, t);
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

        List<Edge<T>> connections = graph.GetEdges(A[0]);

        Partition<T> partition = new(graph, A, V, connections);

        Vertex<T> s = A[0];
        Vertex<T> t = partition.GetMostAdjecent();

        while (V.Count > 1)
        {
            partition.MoveToLeft(t);
            s = t;
            t = partition.GetMostAdjecent();
        }

        return new(partition, s, t);
    }

    internal static Vertex<T> GetMostAdjecent<T>(this Partition<T> partition)
    {
        var right = partition.Right;

        var vertices = right.ToDictionary(x=> x.Id, x => 0);

        foreach(var edge in partition.Connections)
        {
            int w;

            if(vertices.TryGetValue(edge.Target.Id, out w))
            {
                vertices[edge.Target.Id] = w + 1;
                continue;
            }

            if (vertices.TryGetValue(edge.Source.Id, out w))
            {
                vertices[edge.Source.Id] = w + 1;
                continue;
            }

            Debug.WriteLine("This should be unreachable");
        }

        var x = vertices.MaxBy(y => y.Value).Key;

        return partition.Graph[x];
    }   
}

internal static class SnowverloadFast
{
    public static int GroupSize(string input)
    {
        var graph = Parser.GetGraph(input);

        var partition = graph.MinimumGlobalCut();

        return partition.Left.Sum(vertex => vertex.Values.Count) * partition.Right.Sum(vertex => vertex.Values.Count);
    }
}