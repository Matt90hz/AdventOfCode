using IncaTechnologies.Collection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz23;

internal static class LongWalk
{
    public static int LongestHikeNoSlopeFast(string input)
    {
        var island = GetIsland(input);

        var graph = GetGraph(island);

        var start = graph[(0, 0)];

        var toExplore = new Stack<(int Row, int Col, int Len)[]>(); toExplore.Push(start);
        var explored = new List<(int Row, int Col, int Len)[]>();

        while (toExplore.Any())
        {
            var path = toExplore.Pop();

            if (path.Last().Row == island.GetLength(1) - 2)
            {
                explored.Add(path);
                continue;
            }

            var (row, col, _) = path.Last();

            var nextsToExplore = graph[(row, col)]
                .Where(x => NodeIsNotAlreadySeen(x, path))
                .Select(x => path.Append(x).ToArray());

            foreach (var next in nextsToExplore)
            {
                toExplore.Push(next);
            }
        }

        var x = explored.Select(x => x.Sum(node => node.Len)).ToArray();

        return explored.Max(path => path.Sum(node => node.Len));

        static bool NodeIsNotAlreadySeen((int Row, int Col, int Len) step, (int Row, int Col, int Len)[] path)
        {
            var (row, col, _) = step;

            var isIt = path.Any(x => (x.Row, x.Col) == (row, col));

            return isIt is false;
        }
    }


    static IDictionary<(int Row, int Col), (int Row, int Col, int Len)[]> GetGraph(char[,] island)
    {
        Dictionary<(int Row, int Col), (int Row, int Col, int Len)[]> graph = new()
        {
            { (0, 0), new[]{ (1, 2, 0) } }
        };

        Stack<(int Row, int Col)> positions = new();

        positions.Push((1, 2));

        while(positions.Any())
        {
            var position = positions.Pop();
            var connNodes = FindConnectedNodes(position, island);

            graph.Add(position, connNodes);

            var positionToChek = connNodes.Where(x => 
                graph.ContainsKey((x.Row, x.Col)) is false
                && positions.Contains((x.Row, x.Col)) is false);

            foreach (var (row, col, _) in positionToChek)
            {
                positions.Push((row, col));
            }
        }    

        return graph;
    }

    static (int Row, int Col, int Len)[] FindConnectedNodes((int Row, int Col) position, char[,] island)
    {
        var (row, col) = position;

        var islandPosition = island.GetPosition(row, col);

        var connectedNodes = islandPosition
            .GetAdjecents()
            .Where(IsPath)
            .Select(x => FindConnectedNode(islandPosition, x))
            .ToArray();

        return connectedNodes;
    }

    static (int Row, int Col, int Len) FindConnectedNode(IPosition<char> position, IPosition<char> next)
    {
        int len = 1;

        var adjecents = next
            .GetAdjecents()
            .Where(x => x.IsPath() && x.Equals(position) is false)
            .ToArray();

        while (adjecents.Length == 1)
        {
            position = next;
            next = adjecents.First();
            len++;

            adjecents = next
                .GetAdjecents()
                .Where(x => x.IsPath() && x.Equals(position) is false)
                .ToArray();
        }

        return ((int)next.Row, (int)next.Column, len);
    }

    static bool IsPath(this IPosition<char> pos) => pos.Value != '#' && pos.Value != '@';

    public static int LongestHikeNoSlope(string input)
    {
        char[,] island = GetIsland(input);

        var hikingPaths = GetHikingPathsNoSlopes(island);

        var longestPath = hikingPaths.Max(path => path.Count());

        return longestPath - 1;
    }

    static IEnumerable<IEnumerable<IPosition<char>>> GetHikingPathsNoSlopes(char[,] island)
    {
        var startPosition = island.GetPosition(1, 2);

        Queue<IEnumerable<IPosition<char>>> pathsToExplore = new();
        List<IEnumerable<IPosition<char>>> pathsExplored = new();

        pathsToExplore.Enqueue(new[] { startPosition });

        while (pathsToExplore.Any())
        {
            var pathToExplore = pathsToExplore.Dequeue();
            var pathExplored = ExplorePath(pathToExplore.Last(), pathToExplore);

            if (pathExplored.Any() is false) continue;

            var pathFinished = pathExplored.Where(island.ExitFound);
            var pathNotFinished = pathExplored.Except(pathFinished);

            pathsToExplore.EnqueueRange(pathNotFinished);
            pathsExplored.AddRange(pathFinished);

        }

        return pathsExplored;
    }

    public static int LongestHike(string input)
    {
        char[,] island = GetIsland(input);

        var hikingPaths = GetHikingPaths(island);

        var longestPath = hikingPaths.Max(path => path.Count());

        return longestPath - 1;
    }

    static IEnumerable<IEnumerable<IPosition<char>>> GetHikingPaths(char[,] island)
    {
        var startPosition = island.GetPosition(1, 2);

        Queue<IEnumerable<IPosition<char>>> pathsToExplore = new();
        List<IEnumerable<IPosition<char>>> pathsExplored = new();

        pathsToExplore.Enqueue(new[] { startPosition });

        while (pathsToExplore.Any())
        {
            var pathExplored = Explore(pathsToExplore.Dequeue());

            if (pathExplored.Any() is false) continue;

            var pathFinished = pathExplored.Where(island.ExitFound);
            var pathNotFinished = pathExplored.Except(pathFinished);

            pathsToExplore.EnqueueRange(pathNotFinished);
            pathsExplored.AddRange(pathFinished);
        }

        return pathsExplored;
    }

    static bool ExitFound(this char[,] island, IEnumerable<IPosition<char>> path) => path.Last().Row == island.GetLength(0) - 2;

    static IEnumerable<IEnumerable<IPosition<char>>> Explore(IEnumerable<IPosition<char>> path)
    {
        var currStep = path.Last();

        return currStep.Value == '.' ? ExplorePath(currStep, path) : ExploreSlope(currStep, path);
    }

    static IEnumerable<IEnumerable<IPosition<char>>> ExploreSlope(IPosition<char> currStep, IEnumerable<IPosition<char>> path)
    {
        var direction = path.GetLastDirection();

        var nextStep = currStep.Value switch
        {
            '>' => currStep.MoveRight(),
            '<' => currStep.MoveLeft(),
            'v' => currStep.MoveDown(),
            '^' => currStep.MoveUp(),
        };

        if (path.Contains(nextStep))
            return Enumerable.Empty<IEnumerable<IPosition<char>>>();

        var nextPath = path
            .Append(nextStep)
            .ToArray();

        return new[] { nextPath };

    }

    static IEnumerable<IEnumerable<IPosition<char>>> ExplorePath(IPosition<char> currStep, IEnumerable<IPosition<char>> path)
    {
        var nextPaths = currStep
            .GetAdjecents()
            .Where(next => next.Value != '#' && next.Value != '@' && path.Contains(next) is false)
            .Select(next => path.Append(next))
            .ToArray();

        return nextPaths;

    }

    static char[,] GetIsland(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var island = lines
            .ToMultidimensionalArray()
            .SurroundWith('@');

        return island;
    }

}
