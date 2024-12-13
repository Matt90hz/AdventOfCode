
namespace AdventOfCode2024.Day13;
public static class ClawContraption
{
    readonly record struct Machine((long X, long Y) A, (long X, long Y) B, (long X, long Y) P);

    public static long MinimumTokensForAllPrizes(string input)
    {
        var machines = ParseMachines(input);
        var tokens = machines.Sum(TokensToWin);
        return tokens;
    }

    public static long MinimumTokensForAllDamnPrizes(string input)
    {
        var machines = ParseMachines(input).Select(x => x with { P = (x.P.X + 10000000000000, x.P.Y + 10000000000000) });
        var tokens = machines.Sum(TokensToWinCrazy);
        return tokens;
    }

    private static long TokensToWinCrazy(Machine machine)
    {
        var ((ax, ay), (bx, by), (px, py)) = machine;

        if (ax == 0 || ax == ay || (ax * by) == (ay * bx)) return 0;

        var (j, r1) = Math.DivRem((ax * py) - (ay * px), (ax * by) - (ay * bx));
        var (i, r2) = Math.DivRem(((by - bx) * j) - py + px, ax - ay);

        var tokens = r1 + r2 == 0 ? (i * 3) + j : 0;

        return tokens;
    }

    private static long TokensToWin(Machine machine)
    {
        var ((ax, ay), (bx, by), (px, py)) = machine;

        long t = 0;

        for (var i = 1; i <= 100; i++)
        {
            for (var j = 1; j <= 100; j++)
            {
                var x = (ax * i) + (bx * j);
                var y = (ay * i) + (by * j);

                if (px != x || py != y) continue;

                t = (i * 3) + j;
                break;
            }
        }

        return t;
    }

    private static Machine[] ParseMachines(string input)
    {
        var machine = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Chunk(3)
            .Select(x =>
            {
                // father forgive me...
                var a = (int.Parse(x[0][(x[0].IndexOf('X') + 1)..x[0].IndexOf(',')]), int.Parse(x[0][(x[0].IndexOf('Y') + 1)..]));
                var b = (int.Parse(x[1][(x[1].IndexOf('X') + 1)..x[1].IndexOf(',')]), int.Parse(x[1][(x[1].IndexOf('Y') + 1)..]));
                var p = (int.Parse(x[2][(x[2].IndexOf('X') + 2)..x[2].IndexOf(',')]), int.Parse(x[2][(x[2].IndexOf('Y') + 2)..]));

                return new Machine(a, b, p);
            });

        return machine.ToArray();
    }
}