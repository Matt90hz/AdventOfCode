
namespace AdventOfCode2024.Day23;
public static class LANParty
{
    public static int PossibleHistoriansComputers(string input)
    {
        var connections = input.Split(Environment.NewLine).Select(x => x.Split('-')).Select(x => (x[0], x[1]));
        var groups = connections.SelectMany(x => GetInterConnected(x, connections).Select(i => new[] { i, x.Item2, x.Item1 }));
        var tGroups = groups.Where(x => x.Any(x => x.StartsWith('t')));
        var distinct = tGroups.Select(x => string.Join('\0', x.Order())).Distinct();
        return distinct.Count();
    }

    public static string LanPartyPassword(string input)
    {
        var connections = input.Split(Environment.NewLine).Select(x => x.Split('-')).Select(x => (x[0], x[1]));
        var connected = connections.Select(x => GetConnected(x, connections));
        return string.Join(',', connected.Order());
    }

    private static IEnumerable<string> GetInterConnected((string, string) x, IEnumerable<(string Pc1, string Pc2)> connections)
    {
        var (a, b) = x;

        var ca = connections
            .Where(x => x.Pc1 == a || x.Pc2 == a)
            .Select(x => x.Pc1 == a ? x.Pc2 : x.Pc1);

        var cb = connections
            .Where(x => x.Pc1 == b || x.Pc2 == b)
            .Select(x => x.Pc1 == b ? x.Pc2 : x.Pc1);

        var intersect = ca.Intersect(cb);
        
        return intersect;
    }

    private static List<(string x, string y)> GetConnected((string x, string y) x, IEnumerable<(string x, string y)> connections)
    {
        var list = new List<(string x, string y)> { x };
        var hash = connections.ToHashSet();
        hash.Remove(x);
        var queue = new Queue<string>();
        queue.Enqueue(x.x);
        queue.Enqueue(x.y);

        while (queue.TryDequeue(out var v)) 
        {
            var connected = hash
                .Where(x => x.x == v || x.y == v)
                .Select(x => (Edge: x, Other: x.x == v ? x.y : x.x));      
            
            foreach(var (edge, other) in connected)
            {
                list.Add(edge);
                hash.Remove(edge);
                queue.Enqueue(other);
            }
        }

        return list;
    }
}