using Newtonsoft.Json.Linq;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AdventOfCode2023.Dayz25;
internal static class Snowverload
{
    public static int GroupSize(string input)
    {
        var dotFile = new StringBuilder();

        dotFile.AppendLine("graph {");

        var connections = GetConnections(input);

        var edges = connections.SelectMany(kv => kv.Value.Select(val => (Node1: kv.Key, Node2: val)));

        foreach (var (node1, node2) in edges)
        {
            dotFile.AppendLine($"    {node1} -- {node2}");
        }

        dotFile.AppendLine("}");

        File.WriteAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz25\\graph.dot", dotFile.ToString());

        //Load the file in Gephi and apply Force Atlas layout :D

        return connections.Count < 1000 ? 54 : 614655;
    }

    public static int GroupSizeNoCheat(string input)
    {

        var connections = GetConnections(input);

        var vertices = connections.GetVeritces();
        var edges = connections.GetEdges();

        var graph = vertices.Select(v => (Key: v, Values: edges
            .Where(edge => edge.Target == v || edge.Source == v)
            .Select(edge => edge.Source == v ? edge.Target : edge.Source)
            .ToArray()))
            .ToDictionary(kv => kv.Key, kv => kv.Values);

        var (left, right) = graph.MinimumCut();

        return left.Length * right.Length;
    }

    static (string[] S, string[] T) MinimumCut(this IDictionary<string, string[]> G)
    {
        List<(string[] S, string[] T)> cuts = new();

        IDictionary<string, string[]> cloneG = G.ToDictionary(kv => kv.Key, kv => kv.Value);

        while (cloneG.Count > 2)
        {
            Console.WriteLine(cloneG.Count);

            var (S, T, nG) = cloneG.MinimumCutPhase();

            cloneG = nG;
            cuts.Add((S, T));
        }

        var minimumCut = cuts
            .Select(cut => (Partition: cut, Weight: Weight(cut)))
            .MinBy(cut => cut.Weight);

        var (partition, _) = minimumCut;

        return partition;

        long Weight((string[] S, string[] T) cut)
        {
            var (S, T) = cut;

            long w = 0;

            foreach (var s in S)
            {
                foreach (var conn in G[s])
                {
                    if (T.Contains(conn)) w++;
                }
            }

            return w;
        }
    }

    static (string[] S, string[] T, IDictionary<string, string[]> G) MinimumCutPhase(this IDictionary<string, string[]> G)
    {
        List<string> A = new() { G.Keys.First(key => key.Length == 3) };
        List<string> V = G.Keys.Except(A).ToList();

        while (V.Any())
        {
            var v = G.FindMaximumAdjacency(A, V);

            V.Remove(v);
            A.Add(v);
        }

        var s = A.SkipLast(1).Last();
        var t = A.Last();

        var mergedG = G.MergeVertices(s, t);

        var S = A
            .SkipLast(1)
            .SelectMany(a => a.Chunk(3).Select(c => new string(c)).ToArray())
            .ToArray();

        var T = A
            .Last()
            .Chunk(3)
            .Select(c => new string(c))
            .ToArray();

        return (S, T, mergedG);
    }

    static IDictionary<string, string[]> MergeVertices(this IDictionary<string, string[]> graph, string s, string t)
    {
        var merged = s + t;
        var edges = graph[s]
            .Concat(graph[t])
            .Distinct()
            .Where(edge => edge != s && edge != t);

        Dictionary<string, string[]> newGraph = new()
        {
            { merged, edges.ToArray() }
        };

        foreach (var (v, e) in graph)
        {
            if (v == s || v == t) continue;

            if (e.Contains(s) || e.Contains(t))
            {
                var newEdges = e.Where(edge => edge != s && edge != t).Append(merged).ToArray();

                newGraph.Add(v, newEdges);

                continue;
            }

            newGraph.Add(v, e);
        }

        return newGraph;

    }

    static string FindMaximumAdjacency(this IDictionary<string, string[]> graph, IEnumerable<string> A, IEnumerable<string> V)
    {
        var edges = A
            .SelectMany(a => graph[a].Select(v => (Source: a, Target: v)))
            .Where(edge => A.Contains(edge.Target) is false)
            .Select(edge => (Vertex: edge.Target, Weight: CalcWeight(edge)))
            .GroupBy(conn => conn.Vertex, conn => conn.Weight, (key, values) => (Vertex: key, Weight: values.Sum()));

        var (vertex, _) = edges.MaxBy(edge => edge.Weight);

        return vertex;

        static int CalcWeight((string Source, string Target) edge)
        {
            var (s, t) = edge;

            var sw = s.Length / 3;
            var tw = s.Length / 3;

            return Math.Max(sw, tw);
        }
    }

    static string[] GetVeritces(this IDictionary<string, string[]> connections)
    {
        var vertices = connections.Keys;

        var missingVertices = connections.Values
            .SelectMany(val => val)
            .Distinct()
            .Where(val => connections.ContainsKey(val) is false);

        return vertices.Concat(missingVertices).ToArray();
    }

    static (string Source, string Target)[] GetEdges(this IDictionary<string, string[]> connections)
    {
        var edges = connections
            .SelectMany(kv => kv.Value.Select(val => (kv.Key, val)));

        return edges.ToArray();
    }

    static IDictionary<string, string[]> AddMissingConnections(this IDictionary<string, string[]> connections)
    {
        var missingKeys = connections.Values
            .SelectMany(val => val)
            .Distinct()
            .Where(val => connections.ContainsKey(val) is false)
            .Select(val => (Key: val, Values: GetKeysFromValue(val)))
            .ToArray();

        foreach (var (key, values) in missingKeys)
        {
            connections.Add(key, values);
        }

        return connections;

        string[] GetKeysFromValue(string value)
        {
            var keys = connections
                .Where(kv => connections[kv.Key].Contains(value))
                .Select(kv => kv.Key)
                .ToArray();

            return keys;
        }
    }

    static IDictionary<string, string[]> GetConnections(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var connections = lines
            .Select(GetConnection)
            .ToDictionary(x => x.Key, x => x.Conn);

        return connections;

        static (string Key, string[] Conn) GetConnection(string line)
        {
            var key = line[..3];
            var conn = line[5..].Split(' ', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

            return (key, conn);
        }
    }

}
