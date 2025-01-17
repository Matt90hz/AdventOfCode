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
        var toExplore = new Stack<(int Level, (int Row, int Col) Head, int Len)>(50);
        var seenSet = new HashSet<(int Row, int Col)>(50);
        var seenList = new List<(int Row, int Col)>(50);
        int max = 0;

        toExplore.Push((0, (1, 2), 0));

        while (toExplore.TryPop(out var x))
        {
            var (level, head, len) = x;

            if (head.Row == lastRow)
            {
                if (len > max) max = len;
                continue;
            }

            if (level < seenSet.Count)
            {
                for (int i = seenSet.Count - 1; i >= level; i--)
                {
                    seenSet.Remove(seenList[i]);
                    seenList.RemoveAt(i);
                }
            }

            seenSet.Add(head);
            seenList.Add(head);

            foreach (var (node, l) in graph[head])
            {
                if (seenSet.Contains(node)) continue;

                toExplore.Push((level + 1, node, len + l));
            }
        }

        return max;
    }

    static Dictionary<(int Row, int Col), ((int Row, int Col), int)[]> GetGraph(char[,] island)
    {
        Dictionary<(int Row, int Col), ((int Row, int Col), int)[]> graph = new();

        Stack<(int Row, int Col)> positions = new(); positions.Push((1, 2));

        while (positions.Any())
        {
            var position = positions.Pop();

            if (graph.ContainsKey(position)) continue;

            var connNodes = FindConnectedNodes(position, island);

            graph.Add(position, connNodes);

            foreach (var (p, _) in connNodes)
            {
                positions.Push(p);
            }
        }

        return graph;
    }

    static ((int Row, int Col) Node, int Len)[] FindConnectedNodes((int Row, int Col) position, char[,] island)
    {
        var (row, col) = position;

        var current = island.GetPosition(row, col);

        var connectedNodes = current
            .GetAdjacent()
            .Where(IsPath)
            .Select(a => FindConnectedNode(current, a));

        return connectedNodes.ToArray();
    }

    static ((int Row, int Col) Pos, int Len) FindConnectedNode(Position<char> position, Position<char> next)
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

        return (((int)next.Row, (int)next.Column), len);
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
                if (explored.Head.Row == island.GetLength(0) - 2)
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