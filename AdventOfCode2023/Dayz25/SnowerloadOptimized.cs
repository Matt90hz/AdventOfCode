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

        var globalMinimumCut = (new List<int>(), 0);
        var globalMinimumCutWeight = int.MaxValue;

        while (graph.Keys.Count > 1)
        {
            var minimumCutPhase = GetMinimumCutPhase(graph);
            var minimumCutPhaseWeight = graph.GetWeight(minimumCutPhase);

            if (minimumCutPhaseWeight < globalMinimumCutWeight)
            {
                globalMinimumCut = minimumCutPhase;
                globalMinimumCutWeight = minimumCutPhaseWeight;
            }

            graph.Merge(minimumCutPhase.Item1[^1], minimumCutPhase.Item2, cypher);
        }

        var groupSize = globalMinimumCut.Item1.Select(x => cypher[x].Length).Sum() * cypher[globalMinimumCut.Item2].Length;

        return groupSize;
    }

    private static (List<int>, int) GetMinimumCutPhase(this Dictionary<int, List<(int V, int W)>> graph)
    {
        var a = graph.Keys.Take(1).ToList();
        var v = graph.Keys.Skip(1).ToList();

        a.Capacity = graph.Count;
        v.Capacity = graph.Count;

        var heap = GetHeap(a, v, graph);

        while (v.Count > 1)
        {
            var mostAdjecent = heap.RemoveMin().Value;

            a.Add(mostAdjecent);
            v.Remove(mostAdjecent);

            foreach (var (x, w) in graph[mostAdjecent])
            {
                if(heap.Contains(x) is false) continue;
                heap.UpdatePriorityOf(x, heap.PriorityOf(x) + w);
            }
        }

        return (a, v[0]);
    }

    internal static UniqueHeap<int, int> GetHeap(List<int> a, List<int> v, Dictionary<int, List<(int, int)>> graph)
    {
        var heap = HeapFactory.NewFibonacciHeap<int, int>(Comparer<int>.Create((x, y) => (-x).CompareTo(-y)));

        foreach (var vertex in v)
        {
            heap.Add(vertex, 0);
        }

        foreach (var (target, weight) in graph[a[0]])
        {
            heap.UpdatePriorityOf(target, weight);
        }

        return heap;
    }

    private static int GetWeight(this Dictionary<int, List<(int, int)>> graph, (List<int>, int) cut)
    {
        return graph[cut.Item2].Sum(x => x.Item2);
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
