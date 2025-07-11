using PommaLabs.Hippie;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz25;

public static class SnowerloadOptimized
{
    public static int GroupSize(string input)
    {
        var (graph, cypher) = GetGraph(input);

        int lastKey = graph.Count;
        int vertexCount = graph.Count;
        var globalMinimumCut = 0;
        var globalMinimumCutWeight = int.MaxValue;

        while (graph.Keys.Count > 1)
        {
            var minimumCutPhase = GetMinimumCutPhase(graph);
            var minimumCutPhaseWeight = graph.GetWeight(minimumCutPhase);

            if (minimumCutPhaseWeight < globalMinimumCutWeight)
            {
                globalMinimumCut = minimumCutPhase.V;
                globalMinimumCutWeight = minimumCutPhaseWeight;
            }

            graph.Merge(minimumCutPhase.A[^1], minimumCutPhase.V, cypher, ++lastKey);
        }

        var vLength = cypher[globalMinimumCut].Count;

        var groupSize = (vertexCount - vLength) * vLength;

        return groupSize;
    }

    private static (List<int> A, int V) GetMinimumCutPhase(this Dictionary<int, List<(int V, int W)>> graph)
    {
        var vKeys = graph.Keys.ToArray();
        var a = new List<int>(graph.Count) { vKeys[0] };

        var v = CreateHeap(graph, vKeys);

        while (v.Count > 1)
        {
            var mostAdjacent = v.RemoveMin().Value;
            a.Add(mostAdjacent);
            v.UpdatePriorities(graph[mostAdjacent]);
        }

        return (a, v.Min.Value);
    }

    private static UniqueHeap<int, int> UpdatePriorities(this UniqueHeap<int, int> heap, List<(int V, int W)> items)
    {
        foreach (var (x, w) in items)
        {
            if (heap.Contains(x) is false) continue;
            heap.UpdatePriorityOf(x, heap.PriorityOf(x) + w);
        }

        return heap;
    }

    private static IComparer<int> Comparer { get; } = Comparer<int>.Create((x, y) => (-x).CompareTo(-y));

    private static UniqueHeap<int, int> CreateHeap(Dictionary<int, List<(int, int)>> graph, int[] vKeys)
    {
        var heap = HeapFactory.NewFibonacciHeap<int, int>(Comparer);

        foreach (var vertex in vKeys[1..])
        {
            heap.Add(vertex, 0);
        }

        foreach (var (target, weight) in graph[vKeys[0]])
        {
            heap.UpdatePriorityOf(target, weight);
        }

        return heap;
    }

    private static int GetWeight(this Dictionary<int, List<(int V, int W)>> graph, (List<int> A, int V) cut)
    {
        return graph[cut.V].Sum(x => x.W);
    }

    private static Dictionary<int, List<(int, int)>> Merge(
        this Dictionary<int, List<(int, int)>> graph,
        int v1, int v2,
        Dictionary<int, List<string>> cypher,
        int lastKey)
    {
        // update cypher
        var v1Cypher = cypher[v1];
        var v2Cypher = cypher[v2];

        List<string> newDecryption = new(v1Cypher.Count + v2Cypher.Count);
        newDecryption.AddRange(v1Cypher);
        newDecryption.AddRange(v2Cypher);

        cypher.Add(lastKey, newDecryption);

        // merge
        var v1Edges = graph[v1];
        var v2Edges = graph[v2];
        var v1EdgesCount = v1Edges.Count;
        var v2EdgesCount = v2Edges.Count;

        Dictionary<int, int> newEdges = new(v1Edges.Count + v2Edges.Count);

        for (int i = 0; i < v1EdgesCount; i++)
        {
            var (x, w) = v1Edges[i];

            if (x != v2) newEdges[x] = w;
        }

        for (int i = 0; i < v2EdgesCount; i++)
        {
            var (x, w) = v2Edges[i];

            if (x != v1) newEdges[x] = newEdges.GetValueOrDefault(x) + w;
        }

        foreach (var (key, value) in newEdges)
        {
            var edges = graph[key];

            for (int i = edges.Count - 1; i >= 0; i--) 
            {
                var (x, _) = edges[i];

                if (x == v1 || x == v2) edges.RemoveAt(i);
            }

            edges.Add((lastKey, value));
        }

        graph.Remove(v1);
        graph.Remove(v2);
        graph.Add(lastKey, newEdges.Select(x => (x.Key, x.Value)).ToList());

        return graph;
    }

    public static (Dictionary<int, List<(int, int)>> Graph, Dictionary<int, List<string>> Cypher) GetGraph(string input)
    {
        Dictionary<int, List<(int, int)>> graph = new();
        Dictionary<int, List<string>> cypher = new();
        Dictionary<string, int> keysFound = new();

        var lines = input.Split(Environment.NewLine);
        var linesLength = lines.Length;

        for (int i = 0; i < linesLength; i++)
        {
            var line = lines[i];
            var key = line[..3];

            graph.Add(i, new());
            cypher.Add(i, new() { key });
            keysFound.Add(key, i);
        }

        var lastKey = linesLength;

        for (int i = 0; i < linesLength; i++)
        {
            var line = lines[i];
            var key = line[..3];
            var connections = line[5..].Split(' ');
            var connectionsLength = connections.Length;

            for(int j = 0; j < connectionsLength; j++)
            {
                var connection = connections[j];

                if (keysFound.TryAdd(connection, lastKey))
                {
                    graph.Add(lastKey, new());
                    cypher.Add(lastKey, new() { connection });

                    lastKey++;
                }

                var v1 = keysFound[key];
                var v2 = keysFound[connection];

                graph[v1].Add((v2, 1));
                graph[v2].Add((v1, 1));
            }
        }

        return (graph, cypher);
    }
}
