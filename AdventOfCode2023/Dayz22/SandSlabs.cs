using AdventOfCode2023.Dayz17;
using AdventOfCode2023.Dayz18;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz22;

readonly record struct Pos(int X, int Y, int Z);

record Brick(Pos A, Pos B);

internal static class SandSlabs
{
    public static int ChainReaction(string input)
    {
        var bricks = GetBricks(input).LetBricksFall();

        var chainRaction = bricks.Select(toRemove => HowManyWouldFall(toRemove, bricks));

        return chainRaction.Sum();
    }

    static int HowManyWouldFall(Brick toRemove, IEnumerable<Brick> bricks)
    {
        if (bricks.Any() is false) return 0;

        var bricksAbove = bricks
            .Where(brick => brick.A.Z > toRemove.B.Z)
            .OrderBy(brick => brick.A.Z)
            .ToArray();

        var bottomBricks = bricks
            .Where(brick => brick.A.Z <= toRemove.B.Z && brick.B.Z >= toRemove.B.Z && brick != toRemove)
            .ToArray();

        if (bricksAbove.Any() is false) return 0;

        var bricksLayers = bricksAbove
            .GroupBy(brik => brik.A.Z)
            .ToArray();

        var howManyFall = CountCrumbledBricks(bottomBricks, bricksLayers);

        return howManyFall;
    }

    static int CountCrumbledBricks(IEnumerable<Brick> bottomBricks, IEnumerable<IEnumerable<Brick>> bricksLayers)
    {
        var bricksAbove = bricksLayers.FirstOrDefault();

        if (bricksAbove is null) return 0;

        var fallen = bricksAbove
            .Where(brick => brick.FallsThrough(bottomBricks))
            .ToArray();

        var notFallen = bricksAbove
            .Except(fallen)
            .ToArray();

        var nextLayers = bricksLayers
            .Skip(1)
            .ToArray();

        var nextLayer = nextLayers
            .FirstOrDefault()?
            .FirstOrDefault()?
            .A.Z ?? 0;

        var nextBottom = notFallen
            .Concat(bottomBricks)
            .Where(brick => brick.B.Z >= nextLayer - 1)
            .ToArray();

        return fallen.Length + CountCrumbledBricks(nextBottom, nextLayers);
    }

    public static int BricksSafelyDisintegrable(string input)
    {
        var bricks = GetBricks(input);

        var settledBricks = LetBricksFall(bricks);

        var disintegrableBricks = settledBricks.Where(brick => CanBeDisintegrated(brick, settledBricks));

        return disintegrableBricks.Count();
    }

    static bool CanBeDisintegrated(this Brick brick, IEnumerable<Brick> settledBricks)
    {
        if (settledBricks.Any() is false) return true;

        var bricksBelow = settledBricks
            .Where(sb => sb.B.Z == brick.B.Z && sb != brick)
            .ToArray();

        var bricksAbove = settledBricks
            .Where(sb => sb.A.Z == brick.B.Z + 1)
            .ToArray();

        if (bricksAbove.Any() is false) return true;
        if (bricksBelow.Any() is false) return false;

        return bricksAbove.Any(ba => ba.FallsThrough(bricksBelow)) is false;
    }

    static bool FallsThrough(this Brick brickAbove, IEnumerable<Brick> bricksBelow) => bricksBelow.Any(brickAbove.WillFallOn) is false;

    static IEnumerable<Brick> LetBricksFall(this IEnumerable<Brick> bricks)
    {
        if (bricks.Any() is false) return bricks;

        var orderedBricks = bricks.OrderBy(brick => brick.A.Z);

        Queue<Brick> toDrop = new(orderedBricks);
        List<Brick> dropped = new();

        while (toDrop.Any())
        {
            var brickToDrop = toDrop.Dequeue();
            var brickDroppped = brickToDrop.DropOn(dropped);

            dropped.Add(brickDroppped);
        }

        return dropped;
    }

    static Brick DropOn(this Brick brick, IEnumerable<Brick> fallens)
    {
        var brickBelow = fallens
            .OrderBy(x => x.B.Z)
            .LastOrDefault(brick.WillFallOn);

        return brickBelow is not null
            ? brick.FallTo(brickBelow.B.Z + 1)
            : brick.FallTo(1);
    }

    static bool WillFallOn(this Brick brick, Brick fallen)
    {
        var brickBlocks = brick.GetXYSpan();
        var fallenBlocks = fallen.GetXYSpan().ToArray();

        var isInTheWay = brickBlocks.Any(block => fallenBlocks.Contains(block));

        return isInTheWay;
    }

    static IEnumerable<(int X, int Y)> GetXYSpan(this Brick brick) => (brick.A.X, brick.A.Y, brick.B.X, brick.B.Y) switch
    {
        (var xa, _, var xb, _) when xa == xb => Enumerable.Range(brick.A.Y, brick.B.Y - brick.A.Y + 1).Select(x => (xa, x)),
        (_, var ya, _, var yb) when ya == yb => Enumerable.Range(brick.A.X, brick.B.X - brick.A.X + 1).Select(x => (x, ya)),
        _ => throw new ArgumentException($"{brick} is not as expected.")
    };

    static Brick FallTo(this Brick brick, int z)
    {
        var height = brick.B.Z - brick.A.Z;

        return brick with
        {
            A = brick.A with { Z = z },
            B = brick.B with { Z = z + height }
        };
    }

    static IEnumerable<Brick> GetBricks(string input) => input
        .Split(Environment.NewLine)
        .Select(GetBrick);

    static Brick GetBrick(string source)
    {
        var start = GetPosition(source[..source.IndexOf('~')]);
        var end = GetPosition(source[(source.IndexOf('~') + 1)..]);

        return new Brick(start, end);
    }

    static Pos GetPosition(string source)
    {
        var values = source
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        return new Pos(values[0], values[1], values[2]);
    }

}
