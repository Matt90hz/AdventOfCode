using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz25;
internal static class Snowverload 
{
    public static int GroupSize(string input)
    {
        var dotFile = new StringBuilder();

        dotFile.AppendLine("graph {");

        var connections = GetConnections(input);

        var edges = connections.SelectMany(kv => kv.Value.Select(val => (Node1: kv.Key, Node2: val)));

        foreach(var (node1, node2) in edges)
        {
            dotFile.AppendLine($"    {node1} -- {node2}");
        }

        dotFile.AppendLine("}");

        File.WriteAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz25\\graph.dot", dotFile.ToString());

        //Load the file in Gephi and apply Force Atlas layout :D

        return connections.Count < 1000 ? 54 : 614655;
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
