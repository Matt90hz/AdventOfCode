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

        var vLength = cypher[globalMinimumCut].Length;

        var groupSize = (vertexCount - vLength) * vLength;

        return groupSize;
    }

    private static (List<int> A, int V) GetMinimumCutPhase(this Dictionary<int, List<(int V, int W)>> graph)
    {
        var a = new List<int>(graph.Count)
        {
            graph.Keys.First()
        };

        var v = CreateHeap(graph);

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

    private static UniqueHeap<int, int> CreateHeap(Dictionary<int, List<(int, int)>> graph)
    {
        var heap = HeapFactory.NewFibonacciHeap<int, int>(Comparer);

        foreach (var vertex in graph.Keys.Skip(1))
        {
            heap.Add(vertex, 0);
        }

        foreach (var (target, weight) in graph[graph.Keys.First()])
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
        Dictionary<int, string[]> cypher)
    {
        var newKey = cypher.Keys.Max() + 1;

        cypher.Add(newKey, cypher[v1].Concat(cypher[v2]).ToArray());

        var v1Edges = graph[v1];
        var v2Edges = graph[v2];

        var newEdges = v1Edges
            .Concat(v2Edges)
            .Where(x => x.Item1 != v1 && x.Item1 != v2)
            .GroupBy(x => x.Item1, x => x.Item2)
            .Select(group => (group.Key, group.Sum()))
            .ToList();

        foreach (var (key, value) in newEdges)
        {
            var edges = graph[key];

            var toRemove = edges.Where(x => x.Item1 == v1 || x.Item1 == v2).ToArray();

            foreach (var edge in toRemove)
            {
                edges.Remove(edge);
            }

            edges.Add((newKey, value));
        }

        graph.Remove(v1);
        graph.Remove(v2);
        graph.Add(newKey, newEdges);

        return graph;
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
