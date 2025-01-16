using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2023.Dayz23;

internal static class LongWalk
{
    //PART 2
    public static int LongestHikeNoSlopeFast(string input)
    {
        var island = GetIsland(input);
        var lastRow = island.GetLength(1) - 2;
        var graph = GetGraph(island);
        var toExplore = new Stack<(HashSet<(int Row, int Col)> Seen, (int Row, int Col) Head, int Len)>();
        int max = 0;

        toExplore.Push((new() { (1, 2) }, (1, 2), 0));

        while (toExplore.TryPop(out var x))
        {
            var (seen, head, len) = x;

            if (head.Row == lastRow)
            {
                if(len > max) max = len;
                continue;
            }

            foreach (var (node, l) in graph[head])
            {
                if (seen.Contains(node)) continue;

                var ns = new HashSet<(int Row, int Col)>(seen, seen.Comparer) { node };
                toExplore.Push((ns, node, len + l));
            }
        }

        return max;
    }

    static Dictionary<(int Row, int Col), Dictionary<(int Row, int Col), int>> GetGraph(char[,] island)
    {
        Dictionary<(int Row, int Col), Dictionary<(int Row, int Col), int>> graph = new();

        Stack<(int Row, int Col)> positions = new(); positions.Push((1, 2));

        while (positions.Any())
        {
            var position = positions.Pop();

            if (graph.ContainsKey(position)) continue;

            var connNodes = FindConnectedNodes(position, island);

            graph.Add(position, connNodes);

            foreach (var (row, col) in connNodes.Keys)
            {
                positions.Push((row, col));
            }
        }

        return graph;
    }

    static Dictionary<(int Row, int Col), int> FindConnectedNodes((int Row, int Col) position, char[,] island)
    {
        var (row, col) = position;

        var current = island.GetPosition(row, col);

        var connectedNodes = current
            .GetAdjacent()
            .Where(IsPath)
            .Select(a => FindConnectedNode(current, a))
            .ToDictionary(x => (x.Row, x.Col), x => x.Len);

        return connectedNodes;
    }

    static (int Row, int Col, int Len) FindConnectedNode(Position<char> position, Position<char> next)
    {
        int len = 1;

        var nextPossibleSteps = NextPossibleSteps(next, position).ToArray();

        while (nextPossibleSteps.Length == 1)
        {
            position = next;
            next = nextPossibleSteps.First();
            len++;

            nextPossibleSteps = NextPossibleSteps(next, position).ToArray();
        }

        return ((int)next.Row, (int)next.Column, len);
    }

    static IEnumerable<Position<char>> NextPossibleSteps(Position<char> position, Position<char> previous)
    {
        var nextPossibleSteps = position
            .GetAdjacent()
            .Where(a => a.IsPath() && a != previous);

        return nextPossibleSteps;
    }

    static bool IsPath(this Position<char> pos) => pos.Value != '#' && pos.Value != '@';

    //PART 1
    public static int LongestHike(string input)
    {
        char[,] island = GetIsland(input);

        var hikingPaths = GetHikingPaths(island);

        var longestPath = hikingPaths.Max(path => path.Count());

        return longestPath - 1;
    }

    static IEnumerable<IEnumerable<Position<char>>> GetHikingPaths(char[,] island)
    {
        var startPosition = island.GetPosition(1, 2);

        Stack<(HashSet<Position<char>> Seen, Position<char> Head)> pathsToExplore = new();
        List<HashSet<Position<char>>> pathsExplored = new();

        pathsToExplore.Push((new HashSet<Position<char>> { startPosition }, startPosition));

        while (pathsToExplore.TryPop(out var path))
        {
            var pathExplored = Explore(path.Seen, path.Head);

            foreach (var explored in pathExplored)
            {
                if(explored.Head.Row == island.GetLength(0) - 2)
                {
                    pathsExplored.Add(explored.Seen);
                    continue;
                }

                pathsToExplore.Push(explored);
            }
        }

        return pathsExplored;
    }

    static IEnumerable<(HashSet<Position<char>> Seen, Position<char> Head)> Explore(HashSet<Position<char>> path, Position<char> head)
    {
        var explored = head.Value switch
        {
            '@' or '#' => Enumerable.Empty<(HashSet<Position<char>> Seen, Position<char> Head)>(),
            '.' => ExplorePath(head, path),
            _ => ExploreSlope(head, path),
        };

        return explored;
    }

    static IEnumerable<(HashSet<Position<char>> Seen, Position<char> Head)> ExploreSlope(Position<char> head, HashSet<Position<char>> path)
    {
        var nextHead = head.Value switch
        {
            '>' => head.MoveRight(),
            '<' => head.MoveLeft(),
            'v' => head.MoveDown(),
            '^' => head.MoveUp(),
            _ => throw new Exception($"Unexpected step {head}")
        };

        return path.Add(nextHead) is false
            ? Enumerable.Empty<(HashSet<Position<char>> Seen, Position<char> Head)>()
            : Enumerable.Range(0, 1).Select(_ => (path, nextHead));
    }

    static IEnumerable<(HashSet<Position<char>> Seen, Position<char> Head)> ExplorePath(Position<char> head, HashSet<Position<char>> path)
    {
        var nextHeads = head
            .GetAdjacent()
            .Where(next => next.Value != '#' && next.Value != '@' && path.Contains(next) is false);

        var nextPaths = nextHeads.Select(nextHead =>
        {
            var seen = new HashSet<Position<char>>(path) { nextHead };
            return (seen, nextHead);
        });

        return nextPaths;
    }

    static char[,] GetIsland(string input)
    {
        var island = input
            .Split(Environment.NewLine)
            .ToMultidimensionalArray()
            .SurroundWith('@');

        return island;
    }
}