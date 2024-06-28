using AdventOfCode2023.Dayz20;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode2023.Dayz25;

record Partition<T>(Graph<T> Left, Graph<T> Right);

readonly record struct Edge<T>(Guid Source, Guid Target, int Weight) : IComparable<Edge<T>>
{
    public static Edge<T> Empty { get; } = new Edge<T>();

    public int CompareTo(Edge<T> other)
    {
        return Target.CompareTo(other.Target);
    }
}

readonly record struct Vertex<T>(Guid Id, T[] Value, Edge<T>[] Edges);

record Graph<T>(Vertex<T>[] Vertices);

internal static class SnowverloadFast
{
    internal static Graph<string> GetGraph(string input)
    {
        var connections = Snowverload.GetConnections(input);

        var remaningKeys = connections.Values
            .SelectMany(val => val)
            .Where(val => connections.ContainsKey(val) is false)
            .Distinct();

        var allKeys = connections.Keys
            .Concat(remaningKeys)
            .ToArray();

        var vertexIds = allKeys
            .Select(key => (Key: key, Id: Guid.NewGuid()))
            .ToDictionary(x => x.Key, x => x.Id);

        var vertices = allKeys
            .Select(key =>
            {
                var guid = vertexIds[key];
                var value = new[] { key };
                var edges = GetAllEdges(key);

                return new Vertex<string>(guid, value, edges);
            })
            .ToArray();

        return new(vertices);

        Edge<string>[] GetAllEdges(string key)
        {
            var hasClearConnections = connections.TryGetValue(key, out var clearConnections);

            var indirectConnection = connections
                .Where(kv => kv.Value.Contains(key))
                .Select(kv => kv.Key);

            var allTarget = hasClearConnections ? clearConnections!.Concat(indirectConnection) : indirectConnection;

            var edges = allTarget
                .Select(target => new Edge<string>(vertexIds[key], vertexIds[target], 1))
                .ToArray();

            return edges;
        }
    }

    internal static string ToFriendlyString<T>(this Graph<T> graph)
    {
        StringBuilder builder = new();

        foreach (var vertex in graph.Vertices)
        {
            builder.Append("{ ");
            builder.Append('[');
            foreach (var value in vertex.Value)
            {
                builder.Append(' ');
                builder.Append(value);
                builder.Append(' ');
            }
            builder.Append(']');

            builder.Append(" -> ");

            builder.Append('[');
            foreach (var edge in vertex.Edges)
            {
                builder.Append(' ');
                builder.Append('(');

                var connVertex = graph.Vertices.FirstOrDefault(v => v.Id == edge.Target);

                if (connVertex.Id != Guid.Empty)
                {
                    builder.Append('[');
                    foreach (var value in connVertex.Value)
                    {
                        builder.Append(' ');
                        builder.Append(value);
                        builder.Append(' ');
                    }
                    builder.Append(']');
                }
                else
                {
                    builder.Append("[ NULL ]");
                }

                builder.Append(" - ");
                builder.Append(edge.Weight);
                builder.Append(')');
                builder.Append(' ');
            }
            builder.Append(']');
            builder.Append(" }");
            builder.AppendLine();
        }

        return builder.ToString();
    }

    public static int GroupSize(string input)
    {
        var graph = GetGraph(input);

        var partition = MinimumGlobalCut(graph);

        return partition.Left.Vertices.Sum(vertex => vertex.Value.Length) * partition.Right.Vertices.Sum(vertex => vertex.Value.Length);
    }

    static Partition<T> MinimumGlobalCut<T>(Graph<T> graph)
    {
        List<Partition<T>> partitions = new();
        Partition<T> partition;

        while (graph.Vertices.Length > 2)
        {
            //PROGRESS
            //Console.WriteLine("STEP {0}", graph.Vertices.Length);

            (partition, graph) = MinimumCutPhase(graph);

            partitions.Add(partition);

        }

        var min = partitions.MinBy(Weight);

        return min ?? throw new Exception("No partitions found!");

        static int Weight(Partition<T> partition)
        {
            return partition.Left.Vertices
                .SelectMany(vertex => vertex.Edges)
                .Where(edge => partition.Right.Vertices.Any(vertex => vertex.Id == edge.Target))
                .Sum(edge => edge.Weight);
        }

    }

    static (Partition<T>, Graph<T>) MinimumCutPhase<T>(Graph<T> graph)
    {
        List<Vertex<T>> A = graph.Vertices
            .Take(1)
            .ToList();

        List<Vertex<T>> V = graph.Vertices
            .Skip(1)
            .ToList();

        while (V.Any())
        {
            var v = FindMaximumAdjacency(A, graph);

            V.Remove(v);
            A.Add(v);
        }

        var left = A.TakeLast(1).ToArray();
        var right = A.SkipLast(1).ToArray();

        var partition = new Partition<T>(new(left), new(right));

        var source = A.SkipLast(1).Last();
        var target = A.Last();

        var mergedGraph = MergeVertex(source, target, graph);

        return (partition, mergedGraph);

    }

    static Vertex<T> FindMaximumAdjacency<T>(IEnumerable<Vertex<T>> A, Graph<T> graph)
    {
        var inVertex = A.Select(a => a.Id).ToArray();

        var maxAdjecentId = A
            .SelectMany(a => a.Edges)
            .Where(edge => inVertex.Contains(edge.Target) is false)
            .GroupBy(edge => edge.Target)
            .MaxBy(group => group.Sum(edge => edge.Weight))!
            .Key;

        var maxAdjecent = graph.Vertices.First(v => v.Id == maxAdjecentId);

        return maxAdjecent;
    }

    static Graph<T> MergeVertex<T>(Vertex<T> source, Vertex<T> target, Graph<T> graph)
    {
        var newGuid = Guid.NewGuid();

        var newValue = source.Value
            .Concat(target.Value)
            .ToArray();

        var newSourceEdges = source.Edges.Where(edge => edge.Target != target.Id);
        var newTargetEdges = target.Edges.Where(edge => edge.Target != source.Id);

        var newEdges = newSourceEdges
            .Concat(newTargetEdges)
            .GroupBy(edge => edge.Target)
            .Select(group => new Edge<T>(newGuid, group.Key, group.Sum(edge => edge.Weight)))
            .ToArray();

        var newVertex = new Vertex<T>(newGuid, newValue, newEdges);

        var oppositeVertices = newEdges.ToDictionary(edge => edge.Target, edge => edge.Weight);

        var newVertices = graph.Vertices
            .Where(vertex => vertex.Id != source.Id && vertex.Id != target.Id)
            .Select(vertex => oppositeVertices.ContainsKey(vertex.Id) ? FixEdges(vertex) : vertex)
            .Append(newVertex)
            .ToArray();

        return new(newVertices);

        Vertex<T> FixEdges(Vertex<T> vertex) => vertex with
        {
            Edges = vertex.Edges
                .Where(edge => edge.Target != source.Id && edge.Target != target.Id)
                .Append(new(vertex.Id, newGuid, oppositeVertices[vertex.Id]))
                .ToArray()
        };
    }
}
