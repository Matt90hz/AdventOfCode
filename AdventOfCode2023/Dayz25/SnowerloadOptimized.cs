using PommaLabs.Hippie;
using System;
using System.Buffers;
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

        var vertexCount = graph.Count;
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

            graph.Merge(minimumCutPhase.A[^1], minimumCutPhase.V, cypher);
        }

        var vLength = cypher[globalMinimumCut].Length;

        var groupSize = (vertexCount - vLength) * vLength;

        return groupSize;
    }

    private static (List<int> A, int V) GetMinimumCutPhase(this Dictionary<int, List<(int V, int W)>> graph)
    {
        ReadOnlySpan<int> keys = graph.Keys.ToArray();

        var a = new List<int>(graph.Count)
        {
            keys[0]
        };

        var v = CreateHeap(graph, keys);

        while (v.Count > 1)
        {
            var mostAdjacent = v.RemoveMin().Value;
            a.Add(mostAdjacent);
            v.UpdatePriorities(CollectionsMarshal.AsSpan(graph[mostAdjacent]));
        }

        return (a, v.Min.Value);
    }

    private static UniqueHeap<int, int> CreateHeap(Dictionary<int, List<(int, int)>> graph, ReadOnlySpan<int> keys)
    {
        var heap = HeapFactory.NewFibonacciHeap<int, int>(Comparer<int>.Create((x, y) => (-x).CompareTo(-y)));

        foreach (var vertex in keys[1..])
        {
            heap.Add(vertex, 0);
        }

        ReadOnlySpan<(int, int)> conn = CollectionsMarshal.AsSpan(graph[keys[0]]);

        foreach (var (target, weight) in conn)
        {
            heap.UpdatePriorityOf(target, weight);
        }

        return heap;
    }

    private static UniqueHeap<int, int> UpdatePriorities(this UniqueHeap<int, int> heap, ReadOnlySpan<(int V, int W)> items)
    {
        foreach (var (x, w) in items)
        {
            if (heap.Contains(x) is false) continue;
            heap.UpdatePriorityOf(x, heap.PriorityOf(x) + w);
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
        Dictionary<int, string[]> cypher)
    {
        var newKey = cypher.Keys.Count;

        cypher.Merge(v1, v2, newKey);

        /*
        ReadOnlySpan<(int V, int W)> v1Edges = CollectionsMarshal.AsSpan(graph[v1]);
        Span<(int V, int W)> v2Edges = CollectionsMarshal.AsSpan(graph[v2]);

        var newEdges = new List<(int, int)>(v1Edges.Length + v2Edges.Length);

        foreach (var (v, w) in v1Edges)
        {
            if (v == v2) continue;

            var finalW = w;

            for (int i = 0; i < v2Edges.Length; i++)
            {
                var (x, y) = v2Edges[i];

                if (v != x) continue;

                finalW += y;

                v2Edges[i] = (v, 0);
            }

            newEdges.Add((v, finalW));

            var edges = graph[v];

            for (int i = edges.Count - 1; i >= 0; i--)
            {
                var (ev, _) = edges[i];

                if (ev == v1 || ev == v2) edges.RemoveAt(i);
            }

            edges.Add((newKey, finalW));
        }

        foreach (var (v, w) in v2Edges)
        {
            if (v == v1 || w == 0) continue;

            newEdges.Add((v, w));

            var edges = graph[v];

            for (int i = edges.Count - 1; i >= 0; i--)
            {
                var (ev, _) = edges[i];

                if (ev == v1 || ev == v2) edges.RemoveAt(i);
            }

            edges.Add((newKey, w));
        }
        */

        var newEdges = graph[v1]
            .Concat(graph[v2])
            .Where(x => x.Item1 != v1 && x.Item1 != v2)
            .GroupBy(x => x.Item1, x => x.Item2, (k, v) => (k, v.Sum()))
            .ToList();

        newEdges.ForEach(x =>
        {
            var (key, w) = x;

            var edges = graph[key];

            for (int i = edges.Count - 1; i >= 0; i--)
            {
                var (ev, _) = edges[i];

                if (ev == v1 || ev == v2) edges.RemoveAt(i);
            }

            edges.Add((newKey, w));
        });

        graph.Remove(v1);
        graph.Remove(v2);
        graph.Add(newKey, newEdges);

        return graph;
    }

    private static Dictionary<int, string[]> Merge(this Dictionary<int, string[]> cypher, int v1, int v2, int newKey)
    {
        var cypherV1 = cypher[v1];
        var cypherV2 = cypher[v2];
        var cypherV1Length = cypher[v1].Length;
        var cypherV2Length = cypher[v2].Length;

        var cypherArray = new string[cypherV1Length + cypherV2Length];
        Array.Copy(cypherV1, cypherArray, cypherV1.Length);
        Array.Copy(cypherV2, sourceIndex: 0, cypherArray, destinationIndex: cypherV1Length, cypherV2Length);
        cypher.Add(newKey, cypherArray);

        return cypher;
    }

    private static (Dictionary<int, List<(int, int)>> Graph, Dictionary<int, string[]> Cypher) GetGraph(string input)
    {
        var connections = Snowverload.GetConnections(input);

        var cypher = connections
            .Values
            .SelectMany(x => x)
            .Concat(connections.Keys)
            .Distinct()
            .Select((x, i) => (i, new[] { x }))
            .ToDictionary(x => x.i, x => x.Item2);

        var counterCypher = cypher
            .Select(x => (x.Value[0], x.Key))
            .ToDictionary(x => x.Item1, x => x.Key);

        var graph = cypher.Keys.ToDictionary(x => x, _ => new List<(int, int)>());

        var edges = connections.Keys
            .SelectMany(key => connections[key].Select(conn => (counterCypher[key], counterCypher[conn])))
            .ToList();

        foreach (var (key, value) in graph)
        {
            foreach (var (v1, v2) in edges)
            {
                if (key == v1)
                {
                    value.Add((v2, 1));
                    continue;
                }

                if (key == v2)
                {
                    value.Add((v1, 1));
                    continue;
                }
            }
        }

        return (graph, cypher);
    }
}
