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

    private static int _lastKey;

    private static Dictionary<int, List<(int, int)>> Merge(
        this Dictionary<int, List<(int, int)>> graph,
        int v1, int v2,
        Dictionary<int, List<string>> cypher)
    {
        // update cypher
        var newKey = _lastKey++;

        var newDecryption = cypher[v1];
        newDecryption.AddRange(cypher[v2]);

        cypher.Add(newKey, newDecryption);

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

            edges.Add((newKey, value));
        }

        graph.Remove(v1);
        graph.Remove(v2);
        graph.Add(newKey, newEdges.Select(x => (x.Key, x.Value)).ToList());

        return graph;
    }

    private static (Dictionary<int, List<(int, int)>> Graph, Dictionary<int, List<string>> Cypher) GetGraph(string input)
    {
        var connections = Snowverload.GetConnections(input);
        
        var keys = connections
            .Values
            .SelectMany(x => x)
            .Concat(connections.Keys)
            .Distinct();

        var keysCount = keys.Count();

        _lastKey = keysCount;

        var cypher = keys
            .Select((x, i) => (i, new List<string>(keysCount) { x }))
            .ToDictionary(x => x.i, x => x.Item2);

        var counterCypher = cypher
            .Select(x => (x.Value[0], x.Key))
            .ToDictionary(x => x.Item1, x => x.Key);

        var graph = cypher.Keys.ToDictionary(x => x, _ => new List<(int, int)>());

        var edges = connections.Keys
            .SelectMany(key => connections[key].Select(conn => (counterCypher[key], counterCypher[conn])))
            .ToList();

        foreach(var (key, value) in graph)
        {
            foreach(var (v1, v2) in edges)
            {
                if(key == v1)
                {
                    value.Add((v2, 1));
                    continue;
                }

                if(key == v2)
                {
                    value.Add((v1, 1));
                    continue;
                }
            }       
        }

        return (graph, cypher);
    }
}
