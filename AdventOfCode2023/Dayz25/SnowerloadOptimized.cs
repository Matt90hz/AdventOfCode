using AdventOfCode2023.Day8;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
            var (s, t) = GetMinimumCutPhase(graph);
            var minimumCutPhaseWeight = graph.GetWeight(t);

            if (minimumCutPhaseWeight < globalMinimumCutWeight)
            {
                globalMinimumCut = t;
                globalMinimumCutWeight = minimumCutPhaseWeight;
            }

            graph.Merge(s, t, cypher, ++lastKey);
        }

        var vLength = cypher[globalMinimumCut].Length;
        var groupSize = (vertexCount - vLength) * vLength;

        return groupSize;
    }

    private static (int S, int T) GetMinimumCutPhase(this Dictionary<int, Dictionary<int, int>> graph)
    {
        FibonacciIntegerHeap<int> heap = new(graph.Keys, graph.Count);
        FibonacciIntegerHeap<int>.Node s;

        do
        {
            s = heap.ExtractMin();
            heap.UpdatePriorities(graph[s.Key]);
        } 
        while (heap.Count > 1);

        return (s.Key, heap.MinimumKey);
    }

    private static FibonacciIntegerHeap<int> UpdatePriorities(this FibonacciIntegerHeap<int> heap, Dictionary<int, int> edges)
    {
        foreach (var (x, w) in edges)
        {
            if (heap.Nodes.TryGetValue(x, out var node) is false) continue;

            heap.DecreaseKey(node, node.Priority - w);
        }

        return heap;
    }

    private static int GetWeight(this Dictionary<int, Dictionary<int, int>> graph, int v)
    {
        return graph[v].Sum(x => x.Value);
    }

    private static Dictionary<int, Dictionary<int, int>> Merge(
        this Dictionary<int, Dictionary<int, int>> graph,
        int v1, int v2,
        Dictionary<int, string[]> cypher,
        int lastKey)
    {
        // update cypher
        var v1Cypher = cypher[v1];
        var v2Cypher = cypher[v2];

        string[] newDecryption = [.. v1Cypher, .. v2Cypher];

        cypher.Add(lastKey, newDecryption);

        // merge
        var v1Edges = graph[v1];
        var v2Edges = graph[v2];

        Dictionary<int, int> newEdges = new(v1Edges.Count + v2Edges.Count);

        foreach (var (x, w) in v1Edges)
        {
            if (x == v2) continue;

            var edges = graph[x];

            edges.Remove(v1);

            edges[lastKey] = w;
            newEdges[x] = w;
        }

        foreach (var (x, w) in v2Edges)
        {
            if (x == v1) continue;

            var newW = newEdges.GetValueOrDefault(x) + w;
            var edges = graph[x];

            edges.Remove(v2);

            newEdges[x] = newW;
            edges[lastKey] = newW;
        }

        graph.Remove(v1);
        graph.Remove(v2);
        graph.Add(lastKey, newEdges);

        return graph;
    }

    public static (Dictionary<int, Dictionary<int, int>> Graph, Dictionary<int, string[]> Cypher) GetGraph(string input)
    {
        Dictionary<int, Dictionary<int, int>> graph = [];
        Dictionary<int, string[]> cypher = [];
        Dictionary<string, int> keysFound = [];

        var lines = input.Split(Environment.NewLine);
        var linesLength = lines.Length;

        for (int i = 0; i < linesLength; i++)
        {
            var line = lines[i];
            var key = line[..3];

            graph.Add(i, []);
            cypher.Add(i, [key]);
            keysFound.Add(key, i);
        }

        var lastKey = linesLength;

        for (int i = 0; i < linesLength; i++)
        {
            var line = lines[i];
            var key = line[..3];
            var connections = line[5..].Split(' ');
            var connectionsLength = connections.Length;

            for (int j = 0; j < connectionsLength; j++)
            {
                var connection = connections[j];

                if (keysFound.TryAdd(connection, lastKey))
                {
                    graph.Add(lastKey, []);
                    cypher.Add(lastKey, [connection]);

                    lastKey++;
                }

                var v1 = keysFound[key];
                var v2 = keysFound[connection];

                graph[v1].Add(v2, 1);
                graph[v2].Add(v1, 1);
            }
        }

        return (graph, cypher);
    }
}
