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
    public static int BricksSafelyDisintegrable(string input)
    {
        var bricks = GetBricks(input);

        var settledBricks = LetBricksFall(bricks);

        var disintegrableBricks = settledBricks.Where(brick => CanBeDisintegrated(brick, settledBricks));

        return disintegrableBricks.Count();
    }

    static bool CanBeDisintegrated(Brick brick, IEnumerable<Brick> settledBricks)
    {
        if (settledBricks.Any() is false) return true;

        var bricksBelow = settledBricks.Where(sb => sb.B.Z == brick.B.Z && sb != brick).ToArray();
        var bricksAbove = settledBricks.Where(sb => sb.A.Z == brick.B.Z + 1);

        if (bricksAbove.Any() is false) return true;
        if (bricksBelow.Any() is false) return false;

        return bricksAbove.Any(ba => ba.FallsThrough(bricksBelow)) is false;
    }

    static bool FallsThrough(this Brick ba, IEnumerable<Brick> bricksBelow) =>
        bricksBelow.Any(ba.IsInTheWayOf) is false;

    static IEnumerable<Brick> LetBricksFall(IEnumerable<Brick> bricks)
    {
        if (bricks.Any() is false) return bricks;

        var toFall = new Queue<Brick>(bricks.OrderBy(brick => brick.A.Z));
        var fallen = new List<Brick>();

        while (toFall.Any())
        {
            var brick = toFall.Dequeue();
            var fallenBrick = brick.FallOn(fallen);

            fallen.Add(fallenBrick);
        }

        return fallen;
    }

    static Brick FallOn(this Brick brick, IEnumerable<Brick> fallens)
    {
        var bottomPiece = fallens
            .OrderBy(x => x.B.Z)
            .LastOrDefault(brick.IsInTheWayOf);

        return bottomPiece is not null
            ? brick.FallTo(bottomPiece.B.Z + 1)
            : brick.FallTo(1);
    }

    static bool IsInTheWayOf(this Brick brick, Brick fallen)
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
        _ => throw new ArgumentException($"{brick} is not as expexted.")
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

    static IEnumerable<(int X, int Y, int Z)> GetCubes(Brick brick) => (brick.A.X, brick.A.Y, brick.A.Z, brick.B.X, brick.B.Y, brick.B.Z) switch
    {
        (_, _, var za, _, _, var zb) when za == zb => (brick.A.X, brick.A.Y, brick.B.X, brick.B.Y) switch
        {
            (var xa, _, var xb, _) when xa == xb => Enumerable.Range(brick.A.Y, brick.B.Y - brick.A.Y + 1).Select(x => (xa, x, za)),
            (_, var ya, _, var yb) when ya == yb => Enumerable.Range(brick.A.X, brick.B.X - brick.A.X + 1).Select(x => (x, ya, za)),
            _ => throw new ArgumentException($"{brick} is not as expexted.")
        },
        _ => Enumerable.Range(brick.A.Z, brick.B.Z - brick.A.Z + 1).Select(x => (brick.A.X, brick.A.Y, x)),
    };
}
