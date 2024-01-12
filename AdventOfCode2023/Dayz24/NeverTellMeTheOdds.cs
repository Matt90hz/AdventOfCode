using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz24;

readonly record struct Hailstone(Coordinate Position, Coordinate Velocity);

readonly record struct Coordinate(double X, double Y, double Z);

internal static class NeverTellMeTheOdds
{

    public static int DwayneTheRockJohnson(string input)
    {
        var hailstones = GetHailsones(input);
        return 0;
    }

    public static int IntersectionsInTestArea(string input, (long Least, long Most)? testArea = null)
    {
        var (least, most) = testArea ??= (200000000000000, 400000000000000);

        var hailstones = GetHailsones(input);
        var collisions = hailstones.SelectMany((hailstone, i) => GetCollisions(hailstone, hailstones.Skip(i + 1)));
        var collisionsInTestArea = collisions.Where(IsInsideTestArea);

        return collisionsInTestArea.Count();

        bool IsInsideTestArea(Coordinate coordinate)
        {
            return coordinate.X >= least
                && coordinate.Y >= least
                && coordinate.X <= most
                && coordinate.Y <= most;
        }
    }

    static Coordinate[] GetCollisions(Hailstone hailstone, IEnumerable<Hailstone> hailstones)
    {
        var collisions = hailstones
            .Select(otherHailstone => GetCollision(hailstone, otherHailstone))
            .Where(collision => collision.HasValue)
            .Select(collisions => collisions!.Value)
            .ToArray();

        return collisions;

        static Coordinate? GetCollision(Hailstone a, Hailstone b)
        {
            var (x1, y1, _) = a.Position;
            var (b1, a1, _) = a.Velocity;
            var c1 = y1 * b1 - a1 * x1;

            var (x2, y2, _) = b.Position;
            var (b2, a2, _) = b.Velocity;
            var c2 = y2 * b2 - a2 * x2;

            //no collision if parallel
            if (a1 / b1 == a2 / b2) return null;

            var x = (-b1 * c2 - -b2 * c1) / (a1 * -b2 - a2 * -b1);
            var y = (c1 * a2 - c2 * a1) / (a1 * -b2 - a2 * -b1);

            //collision only in the future
            if ((b1 >= 0 && x - x1 < 0) 
                || (b1 <= 0 && x - x1 > 0) 
                || (a1 >= 0 && y - y1 < 0) 
                || (a1 <= 0 && y - y1 > 0) 
                || (b2 >= 0 && x - x2 < 0) 
                || (b2 <= 0 && x - x2 > 0) 
                || (a2 >= 0 && y - y2 < 0) 
                || (a2 <= 0 && y - y2 > 0)) return null;

            return new(x, y, 0);
        }
    }

    static Hailstone[] GetHailsones(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var coordinates = lines
            .Select(GetHailstone)
            .ToArray();

        return coordinates;

        static Hailstone GetHailstone(string line)
        {
            var position = line[..(line.IndexOf('@') - 1)];
            var velocity = line[(line.IndexOf('@') + 1)..];

            return new(GetCoordinate(position), GetCoordinate(velocity));
        }

        static Coordinate GetCoordinate(string coordinate)
        {
            var values = coordinate
                .Split(',', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries)
                .Select(long.Parse)
                .ToArray();

            return new(values[0], values[1], values[2]);
        }
    }

}
