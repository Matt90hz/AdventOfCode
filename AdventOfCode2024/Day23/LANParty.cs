
namespace AdventOfCode2024.Day23;
public static class LANParty
{
    public static int PossibleHistoriansComputers(string input)
    {
        var graph = ParseGraph(input);

        var connections = input
            .Split(Environment.NewLine)
            .Select(x => x.Split('-'))
            .Select(x => (x[0], x[1]));

        var comparer = EqualityComparer<IEnumerable<string>>.Create(
            equals: (x, y) => x!.SequenceEqual(y!),
            getHashCode: x => string.Join('\0', x).GetHashCode());

        var interconnected = connections
            .SelectMany(x => GetInterConnected(x, graph))
            .Distinct(comparer);

        var ts = interconnected.Where(x => x.Any(x => x.StartsWith('t')));

        return ts.Count();
    }

    public static string LanPartyPassword(string input)
    {
        var graph = ParseGraph(input);
        var cliques = BronKerbosch([], [..graph.Keys], [], graph, []);
        var max = cliques.MaxBy(x => x.Count)!.Order();
        return string.Join(',', max);
    }

    private static List<HashSet<string>> BronKerbosch(
        HashSet<string> R,
        HashSet<string> P,
        HashSet<string> X,
        Dictionary<string, HashSet<string>> graph,
        List<HashSet<string>> cliques)
    {
        if (P.Count == 0 && X.Count == 0) 
        {
            cliques.Add(R);
            return cliques;
        }

        foreach(var v in P)
        {
            var n = graph[v];

            BronKerbosch(
                R.Union([v]).ToHashSet(), 
                P.Intersect(n).ToHashSet(), 
                X.Intersect(n).ToHashSet(), 
                graph,
                cliques);

            P.Remove(v);
            X.Add(v);
        }

        return cliques;
    }

    private static Dictionary<string, HashSet<string>> ParseGraph(string input)
    {
        var vertices = input
            .Split(Environment.NewLine)
            .SelectMany(x => x.Split('-'))
            .Distinct();

        var edges = input
            .Split(Environment.NewLine)
            .Select(x => x.Split('-'))
            .Select(x => (x[0], x[1]))
            .ToArray();

        var graph = new Dictionary<string, HashSet<string>>();

        foreach(var v in vertices)
        {
            var conn = edges
                .Where(x => x.Item1 == v || x.Item2 == v)
                .Select(x => x.Item1 == v ? x.Item2 : x.Item1)
                .ToHashSet();

            graph.Add(v, conn);
        }

        return graph;
    }

    private static IEnumerable<IEnumerable<string>> GetInterConnected((string, string) x, Dictionary<string, HashSet<string>> graph)
    {
        var (a, b) = x;
        var ca = graph[a];
        var cb = graph[b];

        var intersect = ca.Intersect(cb);

        foreach (var item in intersect)
        {
            yield return ((IEnumerable<string>)[a, b]).Append(item).Order();
        }
    }
}