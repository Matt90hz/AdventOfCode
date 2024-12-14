using AdventOfCode2024.Day10;

namespace AdventOfCode2024.Day14;
public static class RestroomRedoubt
{
    readonly record struct Robot((int X, int Y) P, (int X, int Y) V);

    public static int SafetyFactor(string input, int time = 100, int wide = 101, int tall = 103) 
    {
        var robots = ParseRobots(input);
        var moved = robots.Select(x => x.Move(time, wide, tall));
        var quadrants = moved.GroupBy(x => x.Quadrant(wide, tall)).Where(g => g.Key != 0);
        var safetyFactor = quadrants.Aggregate(1, (sf, r) => sf * r.Count());
        return safetyFactor;
    }

    public static int EasterEggTime(string input, int wide = 101, int tall = 103)
    {
        var robots = ParseRobots(input);
        var robotMap = robots.ToRobotsMap();
        int i = 1;
        var dx = wide / 2;
        var dy = tall / 2;

        var baseLine = robots.Average(x => Distance(x.P, (dx, dy)));

        for (; i < 10000; i++)
        {
            robots = robots.Select(x => x.Move(wide, tall)).ToArray();

            var averageDistance = robots.Average(x => Distance(x.P, (dx, dy)));

            if (averageDistance / baseLine > 0.7) continue;

            break;
        }

        return i;
    }

    private static double Distance((int X, int Y) A, (int X, int Y) B)
    {
        var (ax, ay) = A;
        var (bx, by) = B;

        var c1 = Math.Sqrt(Math.Pow(Math.Abs(ax - bx), 2) + Math.Pow(Math.Abs(ay - by), 2));

        return c1;
    }

    private static char[,] ToRobotsMap(this IEnumerable<Robot> robots, int wide = 101, int tall = 103)
    {
        var map = new char[tall, wide].Select(_ => '.');

        foreach (var ((x, y), _) in robots) 
        {
            map[y, x] = 'X';
        }

        return map;
    }

    private static int Quadrant(this Robot robot, int wide, int tall)
    {
        var (px, py) = robot.P;

        var dx = wide / 2;
        var dy = tall / 2;

        if (px < dx && py < dy) return 1;
        if (px >= wide - dx && py < dy) return 2;
        if (px < dx && py >= tall - dy) return 3;
        if (px >= wide - dx && py >= tall - dy) return 4;

        return 0;
    }

    private static Robot Move(this Robot r, int time, int wide, int tall)
    {
        for (int i = 0; i < time; i++)
        {
            r = r.Move(wide, tall);
        }

        return r;
    }

    private static Robot Move(this Robot r, int wide, int tall)
    {
        var ((px, py), (vx, vy)) = r;

        var x = px + vx;
        var y = py + vy;

        if(x < 0) x = wide + x;
        if(x >= wide) x -= wide;
        if (y < 0) y = tall + y;
        if (y >= tall) y -= tall;

        return r with { P = (x, y) };
    }

    private static Robot[] ParseRobots(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var robots = lines.Select(x =>
        {
            var str_p = x[(x.IndexOf('p') + 2)..(x.IndexOf('v') - 1)].Split(',');
            var str_v = x[(x.IndexOf('v') + 2)..].Split(',');

            var p = (int.Parse(str_p[0]), int.Parse(str_p[1]));
            var v = (int.Parse(str_v[0]), int.Parse(str_v[1]));

            return new Robot(p, v);
        });

        return robots.ToArray();
    }
}
