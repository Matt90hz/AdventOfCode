using Microsoft.Z3;
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
    public static long DwayneTheRockJohnson(string input)
    {
        var hailstones = GetHailsones(input);

        var ctx = new Context();
        var solver = ctx.MkSolver();

        // Coordinates of the stone
        var x = ctx.MkIntConst("x");
        var y = ctx.MkIntConst("y");
        var z = ctx.MkIntConst("z");

        // Velocity of the stone
        var vx = ctx.MkIntConst("vx");
        var vy = ctx.MkIntConst("vy");
        var vz = ctx.MkIntConst("vz");

        // For each iteration, we will add 3 new equations and one new condition to the solver.
        // We want to find 9 variables (x, y, z, vx, vy, vz, t0, t1, t2) that satisfy all the equations, so a system of 9 equations is enough.
        for (var i = 0; i < 3; i++)
        {
            var t = ctx.MkIntConst($"t{i}"); // time for the stone to reach the hail
            var hail = hailstones[i];

            var px = ctx.MkInt(Convert.ToInt64(hail.Position.X));
            var py = ctx.MkInt(Convert.ToInt64(hail.Position.Y));
            var pz = ctx.MkInt(Convert.ToInt64(hail.Position.Z));

            var pvx = ctx.MkInt(Convert.ToInt64(hail.Velocity.X));
            var pvy = ctx.MkInt(Convert.ToInt64(hail.Velocity.Y));
            var pvz = ctx.MkInt(Convert.ToInt64(hail.Velocity.Z));

            var xLeft = ctx.MkAdd(x, ctx.MkMul(t, vx)); // x + t * vx
            var yLeft = ctx.MkAdd(y, ctx.MkMul(t, vy)); // y + t * vy
            var zLeft = ctx.MkAdd(z, ctx.MkMul(t, vz)); // z + t * vz

            var xRight = ctx.MkAdd(px, ctx.MkMul(t, pvx)); // px + t * pvx
            var yRight = ctx.MkAdd(py, ctx.MkMul(t, pvy)); // py + t * pvy
            var zRight = ctx.MkAdd(pz, ctx.MkMul(t, pvz)); // pz + t * pvz

            solver.Add(t >= 0); // time should always be positive - we don't want solutions for negative time
            solver.Add(ctx.MkEq(xLeft, xRight)); // x + t * vx = px + t * pvx
            solver.Add(ctx.MkEq(yLeft, yRight)); // y + t * vy = py + t * pvy
            solver.Add(ctx.MkEq(zLeft, zRight)); // z + t * vz = pz + t * pvz
        }

        solver.Check();
        var model = solver.Model;

        var rx = model.Eval(x);
        var ry = model.Eval(y);
        var rz = model.Eval(z);

        return Convert.ToInt64(rx.ToString()) + Convert.ToInt64(ry.ToString()) + Convert.ToInt64(rz.ToString());
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
