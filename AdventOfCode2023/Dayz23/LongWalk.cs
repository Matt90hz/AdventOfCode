using IncaTechnologies.Collection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2023.Dayz23;

internal static class LongWalk
{
    //PART 2 TAKE 2
    public static int LongestHikeNoSlopeFast(string input)
    {
        var island = GetIsland(input);
        var lastRow = island.GetLength(1) - 2;

        var graph = GetGraph(island);

        var start = graph[(0, 0)];

        Stack<(int Row, int Col, int Len)[]> toExplore = new(); toExplore.Push(start);
        List<(int Row, int Col, int Len)[]> explored = new();

        while (toExplore.Any())
        {
            var path = toExplore.Pop();
            var (row, col, _) = path.Last();

            if (row == lastRow)
            {
                explored.Add(path);
                continue;
            }

            var connectedNodes = graph[(row, col)];

            foreach (var node in connectedNodes)
            {
                if (path.IsAlreadySeen(node)) continue;

                toExplore.Push(path.Append(node).ToArray());
            }
        }

        return explored.Max(path => path.Sum(node => node.Len));

    }

    static bool IsAlreadySeen(this (int Row, int Col, int Len)[] path, (int Row, int Col, int Len) node)
    {
        var (row, col, _) = node;

        var isAreadySeen = path.Any(x => x.Row == row && x.Col == col);

        return isAreadySeen;
    }

    static IDictionary<(int Row, int Col), (int Row, int Col, int Len)[]> GetGraph(char[,] island)
    {
        Dictionary<(int Row, int Col), (int Row, int Col, int Len)[]> graph = new()
        {
            { (0, 0), new[]{ (1, 2, 0) } }
        };

        Stack<(int Row, int Col)> positions = new(); positions.Push((1, 2));

        while (positions.Any())
        {
            var position = positions.Pop();

            if (graph.ContainsKey(position)) continue;

            var connNodes = FindConnectedNodes(position, island);

            graph.Add(position, connNodes);

            foreach (var (row, col, _) in connNodes)
            {
                positions.Push((row, col));
            }
        }

        return graph;
    }

    static (int Row, int Col, int Len)[] FindConnectedNodes((int Row, int Col) position, char[,] island)
    {
        var (row, col) = position;

        var current = island.GetPosition(row, col);

        var connectedNodes = current
            .GetAdjecents()
            .Where(IsPath)
            .Select(adjecent => FindConnectedNode(current, adjecent))
            .ToArray();

        return connectedNodes;
    }

    static (int Row, int Col, int Len) FindConnectedNode(IPosition<char> position, IPosition<char> next)
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

    static IEnumerable<IPosition<char>> NextPossibleSteps(IPosition<char> position, IPosition<char> previous)
    {
        var nextPossibleSteps = position
            .GetAdjecents()
            .Where(adjecent => adjecent.IsPath() && adjecent.Equals(previous) is false);

        return nextPossibleSteps;
    }

    static bool IsPath(this IPosition<char> pos) => pos.Value != '#' && pos.Value != '@';

    //PART 2 TAKE 1
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

    //PART 1
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
