using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2023.Dayz10;
using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2023.Dayz17;

readonly record struct Block(int HeatLoss);

record Path(Position<Block> Position, Direction Direction, int HeatLoss, Path? From)
{
    public override string ToString()
    {
        return $"{Position}, {Direction}";
    }
}

enum Direction { Up, Down, Left, Right, None }

internal static class ClumsyCrucible
{
    public static int HeatLoss(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var bloks = lines
            .Select(line => line.AsEnumerable())
            .Select(GetBlocks);

        var heatLoss = CalculateHeatLoss(bloks);

        return heatLoss;
    }

    public static int UltraHeatLoss(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var bloks = lines
            .Select(line => line.AsEnumerable())
            .Select(GetBlocks);

        var heatLoss = CalculateUltraHeatLoss(bloks);

        return heatLoss;
    }

    static int CalculateUltraHeatLoss(IEnumerable<IEnumerable<Block>> blocks)
    {
        var blocksArray = blocks
            .ToMultidimensionalArray()
            .SurroundWith(new Block(int.MaxValue));

        var start = blocksArray.GetPosition(1, 1);
        var end = blocksArray.GetPosition(blocksArray.GetLength(0) - 2, blocksArray.GetLength(1) - 2);

        var queue = new PriorityQueue<Path, int>();
        var seen = new List<Path>();
        var startPath = new Path(start, Direction.None, 0, null);

        queue.Enqueue(startPath, startPath.HeatLoss);

        while (queue.UnorderedItems.Any())
        {
            var curr = queue.Dequeue();
            var (pos, _, heat, _) = curr;

            //Console.WriteLine(curr.ToFriendlyString());

            if (pos.Equals(end) && curr.CountContinuousSteps() >= 4)
                return heat;

            if (curr.IsAmong(seen))
                continue;

            seen.Add(curr);

            var reachables = curr.GetUltraReachables();

            foreach (var path in reachables)
            {
                queue.Enqueue(path, path.HeatLoss);
            }
        }

        throw new Exception("notfound");
    }

    static IEnumerable<Path> GetUltraReachables(this Path from)
    {
        var reachable = from.Position
                .GetAdjecents()
                .Where(from.CanUltraReach)
                .Select(next => new Path(next, GetDirection(from.Position, next), from.HeatLoss + next.Value.HeatLoss, from));

        return reachable;
    }

    static bool CanUltraReach(this Path from, Position<Block> to)
    {
        //border
        if (to.Value.HeatLoss > 9) return false;

        //backwards
        if (from.From is not null && from.From.Position.Equals(to)) return false;

        //correct number of steps
        if(from.Direction == Direction.None) return true;

        var dir = GetDirection(from.Position, to);
        var lastStepsCount = from.CountContinuousSteps();

        if (dir == from.Direction && lastStepsCount > 9) return false;

        if (dir != from.Direction && lastStepsCount < 4) return false;

        //legal next position
        return true;

    }


    static int CalculateHeatLoss(IEnumerable<IEnumerable<Block>> blocks)
    {
        var blocksArray = blocks
            .ToMultidimensionalArray()
            .SurroundWith(new Block(int.MaxValue));

        var start = blocksArray.GetPosition(1, 1);
        var end = blocksArray.GetPosition(blocksArray.GetLength(0) - 2, blocksArray.GetLength(1) - 2);

        var queue = new PriorityQueue<Path, int>();
        var seen = new List<Path>();
        var startPath = new Path(start, Direction.None, 0, null);

        queue.Enqueue(startPath, startPath.HeatLoss);

        while (queue.UnorderedItems.Any())
        {
            var curr = queue.Dequeue();
            var (pos, _, heat, _) = curr;

            //Console.WriteLine(curr.ToFriendlyString());

            if (pos.Equals(end)) 
                return heat;

            if (curr.IsAmong(seen))
                continue;

            seen.Add(curr);

            var reachables = curr.GetReachables();

            foreach (var path in reachables)
            {
                queue.Enqueue(path, path.HeatLoss);
            }
        }

        throw new Exception("notfound");
    }

    static bool IsAmong(this Path path, IEnumerable<Path> seen)
    {
        var isAmong = seen.Any(path.IsEquivalent);

        return isAmong;
    }

    static bool IsEquivalent(this Path x, Path y)
    {
        return x.Position.Equals(y.Position)
            //&& x.HeatLoss == y.HeatLoss
            && x.Direction == y.Direction
            && x.CountContinuousSteps() == y.CountContinuousSteps();
    }

    static int CountContinuousSteps(this Path path)
    {
        if (path.Direction == Direction.None) return 0;

        var lastDirections = new[]
        {
            path.Direction,
            path.From?.Direction ?? Direction.None,
            path.From?.From?.Direction ?? Direction.None,
            path.From?.From?.From?.Direction ?? Direction.None,
            path.From?.From?.From?.From?.Direction ?? Direction.None,
            path.From?.From?.From?.From?.From?.Direction ?? Direction.None,
            path.From?.From?.From?.From?.From?.From?.Direction ?? Direction.None,
            path.From?.From?.From?.From?.From?.From?.From?.Direction ?? Direction.None,
            path.From?.From?.From?.From?.From?.From?.From?.From?.Direction ?? Direction.None,
            path.From?.From?.From?.From?.From?.From?.From?.From?.From?.Direction ?? Direction.None,
        };

        var count = lastDirections.TakeWhile(x => x == lastDirections[0]).Count();

        return count;
    }

    static IEnumerable<Path> GetReachables(this Path from)
    {
        var reachable = from.Position
                .GetAdjecents()
                .Where(from.CanReach)
                .Select(next => new Path(next, GetDirection(from.Position, next), from.HeatLoss + next.Value.HeatLoss, from));

        return reachable;
    }

    static bool CanReach(this Path from, Position<Block> to)
    {
        //border
        if (to.Value.HeatLoss > 9) return false;

        //loop or backwards
        if (from.GetPreviouses().Any(x => x.Position.Equals(to))) return false;

        //too many in the same direction
        var lastDirections = new[]
        {
            GetDirection(from.Position, to),
            from.Direction,
            from.From?.Direction ?? Direction.None,
            from.From?.From?.Direction ?? Direction.None,
        };

        if (lastDirections.All(dir => dir == lastDirections[0])) return false;

        //legal next position
        return true;
    }

    static IEnumerable<Path> GetPreviouses(this Path path)
    {
        while (path.From is not null)
        {
            yield return path.From;
            path = path.From;
        }
    }

    //make it generic
    static IEnumerable<Direction> GetDirections(this IEnumerable<Position<Block>> path) => path
        .Skip(1)
        .Select((x, i) => GetDirection(path.ElementAt(i), x));

    //make it generic
    static Direction GetDirection(Position<Block> first, Position<Block> second) =>
        (first.Column > second.Column, first.Row > second.Row, first.Column == second.Column, first.Row == second.Row) switch
        {
            (true, _, _, true) => Direction.Left,
            (false, _, _, true) => Direction.Right,
            (_, true, true, _) => Direction.Up,
            (_, false, true, _) => Direction.Down,
            _ => Direction.Right,
        };

    //make it generic
    static Direction Reverse(this Direction direction) => direction switch
    {
        Direction.Up => Direction.Down,
        Direction.Down => Direction.Up,
        Direction.Left => Direction.Left,
        Direction.Right => Direction.Right,
        _ => throw new NotImplementedException()
    };

    static IEnumerable<Block> GetBlocks(IEnumerable<char> enumerable)
    {
        return enumerable.Select(x => new Block(x - '0'));
    }

    static string ToFriendlyString(this Path path)
    {
        var x = path
            .GetPreviouses()
            .Append(path)
            .Select(x => x.Position);

        return x.ToFriendlyString();
    }

    static string ToFriendlyString(this IEnumerable<Position<Block>> path)
    {
        var blocks = path.First().Array;
        var pathPositions = path.Select(x => (x.Row, x.Column)).ToArray();

        var x = blocks.Select((x, p) => pathPositions.Contains(p)
            ? "*"
            : x.HeatLoss == int.MaxValue
                ? "@"
                : x.HeatLoss.ToString());

        return x.ToFriendlyString();
    }

    static string ToFriendlyString(this IEnumerable<IEnumerable<Position<Block>>> paths)
    {
        var sb = new StringBuilder();

        var friendlyStrings = paths
            .Select(x => x
                .ToFriendlyString()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            .ToMultidimensionalArray();

        for (int i = 0; i < friendlyStrings.GetLength(1); i++)
        {
            for (int j = 0; j < friendlyStrings.GetLength(0); j++)
            {
                sb.Append(friendlyStrings[j, i]);
                sb.Append(' ');
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
