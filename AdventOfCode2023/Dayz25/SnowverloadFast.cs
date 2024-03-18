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

internal static class SnowverloadFastest
{
    public static int GroupSize(string input)
    {
        var graph = SnowverloadFast.GetGraph(input);

        var (a, b) = graph.GetMinimumGlobalCut();

        return a.Length * b.Length;
    }

    //no need to optimize
    static (T[] A, T[] B) GetMinimumGlobalCut<T>(this Graph<T> graph)
    {
        var vertices = graph.Vertices;
        int verticesLength = vertices.Length;

        List<Vertex<T>[]> cuts = new(vertices.Length - 1);

        for (int i = 1; i < verticesLength; i++)
        {
            //PROGRESS
            Console.WriteLine("STEP {0}", verticesLength - i);

            var cut = GetMinimumCutPhase(vertices.AsSpan());

            cuts.Add(cut);

            vertices = cut.MergeST();
        }

        var min = cuts.MinBy(GetWeight) ?? throw new Exception("cut not found");

        var A = min[0].Value;
        var B = min
            .Skip(1)
            .SelectMany(vertex => vertex.Value)
            .ToArray();

        return (A, B);
    }

    static Vertex<T>[] GetMinimumCutPhase<T>(Span<Vertex<T>> vertices)
    {
        for (int i = 1; i < vertices.Length; i++)
        {
            ReadOnlySpan<Vertex<T>> cuts = vertices[..i];
            ReadOnlySpan<Vertex<T>> others = vertices[i..];

            int max = GetMaximumAdjacenctIndex(cuts, others);

            var x = vertices[i]; 
            var y = vertices[max + i];

            vertices[i] = y; 
            vertices[max + i] = x;
        }

        vertices.Reverse();

        return vertices.ToArray();
    }

    static int GetMaximumAdjacenctIndex<T>(ReadOnlySpan<Vertex<T>> cuts, ReadOnlySpan<Vertex<T>> rest)
    {
        /* ALTERNATIVE
        int numOfEdges = 0;

        //flatten all edges
        for (int i = 0; i < cuts.Length; i++)
        {
            numOfEdges += cuts[i].Edges.Length;
        }

        Span<Edge<T>> allEdges = stackalloc Edge<T>[numOfEdges];

        for(int i = 0, k = 0; i < cuts.Length; i++)
        {
            ReadOnlySpan<Edge<T>> edges = cuts[i].Edges;

            for(int j = 0 ; j < edges.Length; j++, k++)
            {
                allEdges[k] = edges[j];
            }
        }

        //filter all edges that reference the same group of verices
        for (int i = 0; i < allEdges.Length; i++)
        {
            for(int j = 0 ; j < cuts.Length; j++)
            {
                if (allEdges[i].Target == cuts[j].Id) allEdges[i] = Edge<T>.Empty; 
            }
        }

        //sort the edges
        allEdges.Sort();

        //find maximum id
        var (_, currId, currWeight) = allEdges[0];
        int maxWeight = currWeight;
        Guid maxId = currId;

        for (int i = 1; i < numOfEdges; i++)
        {
            var(_, id, weight) = allEdges[i];

            if (id == currId)
            {
                currWeight += weight;
            }
            else
            {
                if (currWeight > maxWeight) 
                { 
                    maxWeight = currWeight;
                    maxId = currId;
                }

                currWeight = weight;
                currId = id;
            }
        }

        if (currWeight > maxWeight) maxId = currId;
        
        //retrive di id index
        for(int i = 0; i < rest.Length; i++)
        {
            if (rest[i].Id == maxId)
            {
                return i;
            }
        }

        throw new Exception("fuck");// they also reference each other fuuuuck
        */

        int maxIndex = 0;
        int max = rest[0].GetAdjacency(cuts);

        for (int i = 1, j = 0; i < rest.Length; i++, j++)
        {
            int weight = rest[i].GetAdjacency(cuts);

            if (weight > max)
            {
                maxIndex = i;
                max = weight;
            }
        }

        return maxIndex;
    }

    //obsolete
    static (Vertex<T>[] Cuts, Vertex<T>[] Others) CutMaximumAdjacency<T>(ReadOnlySpan<Vertex<T>> cuts, ReadOnlySpan<Vertex<T>> rest)
    {
        int maxIndex = 0;
        int max = rest[0].GetAdjacency(cuts);

        for (int i = 1, j = 0; i < rest.Length; i++, j++)
        {
            int weight = rest[i].GetAdjacency(cuts);

            if (weight > max)
            {
                maxIndex = i;
                max = weight;
            }
        }

        var newCuts = new Vertex<T>[cuts.Length + 1];
        var newRest = new Vertex<T>[rest.Length - 1];

        var oldRest = rest.ToArray();

        Array.Copy(oldRest, newRest, maxIndex);
        Array.Copy(oldRest, maxIndex + 1, newRest, maxIndex, newRest.Length - maxIndex);

        //put the max in the array
        newCuts[0] = rest[maxIndex];

        //fill the rest of new cuts with old cuts
        Array.Copy(cuts.ToArray(), 0, newCuts, 1, cuts.Length);

        return (newCuts, newRest);
    }

    //optimized
    static int GetAdjacency<T>(this Vertex<T> vertex, ReadOnlySpan<Vertex<T>> vertices)
    {
        int adjecency = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            ReadOnlySpan<Edge<T>> edges = vertices[i].Edges;

            adjecency += GetAdjacency(vertex.Id, edges);
        }

        return adjecency;
    }

    //optimized
    static int GetAdjacency<T>(Guid target, ReadOnlySpan<Edge<T>> edges)
    {
        int adjecency = 0;

        for (int i = 0; i < edges.Length; i++)
        {
            if (target == edges[i].Target) adjecency += edges[i].Weight;
        }

        return adjecency;
    }

    static Vertex<T>[] MergeST<T>(this Vertex<T>[] cutPhase)
    {
        //new vertices will have one less element after merging
        var newVertices = new Vertex<T>[cutPhase.Length - 1];
        var t = cutPhase[0];
        var s = cutPhase[1];

        //merge s and t
        var merged = MergeVertices(t, s);

        //the merged is placed in the first spot
        newVertices[0] = merged;

        //fill the vertices in the new array
        for (int i = 1, j = 2; i < newVertices.Length; i++, j++)
        {
            var vertex = cutPhase[j];
            var edges = vertex.Edges;

            int newEdgesCount = 0;
            var newEdges = new Edge<T>[vertex.Edges.Length];

            //find out if there is one or more edges that points to the new merged vertex
            for (int k = 0; k < edges.Length; k++)
            {
                var edge = edges[k];

                //ignore the edges that point to the old s and t
                if (edge.Target == t.Id || edge.Target == s.Id) continue;

                newEdges[newEdgesCount] = edge;
                newEdgesCount++;
            }

            //if the new edges are the lenght of the old edges than nothing has been found
            if (newEdgesCount == edges.Length)
            {
                //the vertex can be stored in the new vertices as it is nothing has changed
                newVertices[i] = vertex;
            }
            else
            {
                //resize the new edges so the last element will be the referece to the new edge
                Array.Resize(ref newEdges, newEdgesCount + 1);

                //find out the weight of the edge
                int weight = 0;

                for (int k = 0; k < merged.Edges.Length; k++)
                {
                    if (merged.Edges[k].Target == vertex.Id)
                    {
                        weight = merged.Edges[k].Weight;
                        break;
                    }
                }

                //put the new merged edge in the last spot
                newEdges[^1] = new(vertex.Id, merged.Id, weight);

                //store the vertex in the new vertices with the new edges
                newVertices[i] = vertex with
                {
                    Edges = newEdges
                };
            }
        }

        return newVertices;
    }

    static Vertex<T> MergeVertices<T>(Vertex<T> vertex1, Vertex<T> vertex2)
    {
        var id = Guid.NewGuid();
        var edges1 = vertex1.Edges;
        var edges2 = vertex2.Edges;

        //the combined edges between the vertices for sure wont have the edges that reference each others
        var combinedEdges = new Edge<T>[edges1.Length + edges2.Length - 2];

        //keeps track of the position for writing combinedEdges
        int k = 0;

        //for each edge of the vertex1 find matching edges on vertex2
        for (int i = 0; i < edges1.Length; i++)
        {
            var edge1 = edges1[i];
            var target = Guid.Empty;
            int weight = 0;

            //if the edge reference the opposite vertex ignore the edge
            if (edge1.Target == vertex2.Id) continue;

            //look for matching edges in the other vertex edges
            for (int j = 0; j < edges2.Length; j++)
            {
                if (edges1[i].Target == edges2[j].Target)
                {
                    weight = edge1.Weight + edges2[j].Weight;
                    target = edge1.Target;
                    break;
                }
            }

            //if weight is not 0 the a common edge was found else this edge has not to be merged
            if (weight != 0)
            {
                combinedEdges[k] = new(id, target, weight);
            }
            else
            {
                combinedEdges[k] = new(id, edge1.Target, edge1.Weight);
            }

            //move foreward the cursor of the combined edges
            k++;
        }

        //save the edges count found so far
        int combinedEdgesCount = k;

        //for each edge in vertex2 look for the edges that has not been merged
        for (int i = 0; i < edges2.Length; i++)
        {
            var edge2 = edges2[i];
            bool notFoundMatch = true;

            //if the edge target the other vertex ignore it
            if (edge2.Target == vertex1.Id) continue;

            //chek in the edge has already been combined
            for (int j = 0; j < combinedEdgesCount; j++)
            {
                if (edge2.Target == combinedEdges[j].Target)
                {
                    notFoundMatch = false;
                    break;
                }
            }

            //if it wasnt combined than add the edge to the combined edge
            if (notFoundMatch)
            {
                combinedEdges[k] = new(id, edge2.Target, edge2.Weight);
                k++;
            }
        }

        //merge the values of the vertices
        var value1 = vertex1.Value;
        var value2 = vertex2.Value;

        var combinedValues = new T[value1.Length + value2.Length];
        Array.Copy(value1, combinedValues, value1.Length);
        Array.Copy(value2, 0, combinedValues, value1.Length, value2.Length);

        //remove eventual empty edges
        for (int i = 0; i < combinedEdges.Length; i++)
        {
            if (combinedEdges[i].Source == Guid.Empty)
            {
                var newEdges = new Edge<T>[i];
                Array.Copy(combinedEdges, newEdges, newEdges.Length);

                return new(id, combinedValues, newEdges);
            }
        }

        //if there were no empty edges just use the combined edges
        return new(id, combinedValues, combinedEdges);
    }

    //no need to optimize
    static int GetWeight<T>(this Vertex<T>[] cut)
    {
        return cut[0].Edges.Sum(edge => edge.Weight);
    }
}