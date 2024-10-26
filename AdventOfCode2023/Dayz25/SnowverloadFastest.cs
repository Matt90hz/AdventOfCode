namespace AdventOfCode2023.Dayz25;

internal static class SnowverloadFastest
{
    public static int GroupSize(string input)
    {
        var connections = Snowverload.GetConnections(input);

        var remaningKeys = connections.Values
            .SelectMany(val => val)
            .Where(val => connections.ContainsKey(val) is false)
            .Distinct();

        var allKeys = connections.Keys
            .Concat(remaningKeys)
            .ToArray();

        var edges = connections.Keys
            .SelectMany(key => connections[key].Select(v2 => (key, v2, 1)))
            .ToArray();

        var (a, b) = GetMinimumGlobalCut(allKeys, edges);

        return a.Length * b.Length;
    }

    static (string[] A, string[] B) GetMinimumGlobalCut(this string[] vertices, (string V1, string V2, int W)[] edges)
    {
        if (vertices.Length == 0) return (vertices, vertices);
        if (vertices.Length == 1) return (vertices, Array.Empty<string>());

        int globalMinimumCutWeight = int.MaxValue;
        var globalMinimumCut = (vertices[..0], vertices[1..]);

        //global cut
        while (vertices.Length > 2)
        {
            string a = vertices[0];
            string b = GetMostTightlyConnectedVertexOnOrderedVertex(edges);
            var (mergedVertices, mergedEdges) = MergeVerticesAndEdges(a, b, vertices, edges);

            //cut phase
            while (mergedVertices.Length > 2)
            {
                a = mergedVertices[0];
                b = GetMostTightlyConnectedVertexOnOrderedVertex(mergedEdges);
                (mergedVertices, mergedEdges) = MergeVerticesAndEdges(a, b, mergedVertices, mergedEdges);
            }

            if (globalMinimumCutWeight > mergedEdges[0].W)
            {
                globalMinimumCut = (mergedVertices[0].UnwrapVertices(), mergedVertices[1].UnwrapVertices());
                globalMinimumCutWeight = mergedEdges[0].W;
            }

            (vertices, edges) = MergeVerticesAndEdges(mergedVertices[1], b, vertices, edges);
        }

        return globalMinimumCut;
    }

    static string GetMostTightlyConnectedVertexOnOrderedVertex(ReadOnlySpan<(string V1, string V2, int W)> edges)
    {
        string a = edges[0].V1;
        int maxW = 0;
        string mostTightlyConnectedVertex = string.Empty;

        foreach (var (v1, v2, w) in edges)
        {
            if (v1.Equals(a) is false) return mostTightlyConnectedVertex;

            if (w > maxW)
            {
                mostTightlyConnectedVertex = v2;
                maxW = w;
                continue;
            }
        }

        return mostTightlyConnectedVertex;
    }

    static (string[] Vertices, (string V1, string V2, int W)[] Edges) MergeVerticesAndEdges(string a, string b, string[] vertices, ReadOnlySpan<(string V1, string V2, int W)> edges)
    {
        string newVertex = a + b;
        List<(string V1, string V2, int W)> mergedEdges = new();
        int inserted = 0;

        foreach (var (v1, v2, w) in edges)
        {
            if (v1.Equals(a))
            {
                if (v2.Equals(b)) continue;

                mergedEdges.Insert(0, (newVertex, v2, w));
                inserted++;
                continue;
            }

            if (v2.Equals(a))
            {
                if (v1.Equals(b)) continue;

                mergedEdges.Insert(0, (newVertex, v1, w));
                inserted++;
                continue;
            }

            if (v1.Equals(b))
            {
                if (v2.Equals(a)) continue;

                mergedEdges.Insert(0, (newVertex, v2, w));
                inserted++;
                continue;
            }

            if (v2.Equals(b))
            {
                if (v1.Equals(a)) continue;

                mergedEdges.Insert(0, (newVertex, v1, w));
                inserted++;
                continue;
            }

            mergedEdges.Add((v1, v2, w));
        }

        for (int i = 0; i < inserted; i++)
        {
            var (_, insertedV2, insertedW) = mergedEdges[i];

            for (int j = i + 1; j < inserted; j++)
            {
                var (_, nextInsertedV2, nextInsertedW) = mergedEdges[j];

                if (insertedV2.Equals(nextInsertedV2) is false) continue;

                mergedEdges.RemoveAt(j);
                mergedEdges.RemoveAt(i);
                mergedEdges.Insert(i, (newVertex, insertedV2, insertedW + nextInsertedW));
                inserted--;
            }
        }

        var mergedVertices = vertices
            .Where(vertex => !vertex.Equals(a) && !vertex.Equals(b))
            .Prepend(newVertex)
            .ToArray();

        return (mergedVertices, mergedEdges.ToArray());
    }

    static string[] UnwrapVertices(this string wreppedVeritices) => wreppedVeritices
        .Chunk(3)
        .Select(x => new string(x))
        .ToArray();
}