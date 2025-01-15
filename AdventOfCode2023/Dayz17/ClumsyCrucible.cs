using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2023.Dayz17;

internal static class ClumsyCrucible
{
    public static int HeatLoss(string input)
    {
        var blocks = ParseBlocks(input);
        var min = MinHeatLoss(1, 3, blocks);
        return min;
    }

    public static int UltraHeatLoss(string input)
    {
        var blocks = ParseBlocks(input);
        var min = MinHeatLoss(4, 10, blocks);
        return min;
    }

    static int MinHeatLoss(int least, int most, Dictionary<(int x, int y), int> blocks)
    {
        var queue = new PriorityQueue<(int x, int y, int px, int py), int>();
        var seen = new HashSet<(int x, int y, int px, int py)>();
        var directions = new (int dx, int dy)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
        var end = (blocks.Keys.Max(k => k.x), blocks.Keys.Max(k => k.y));

        queue.Enqueue((0, 0, 0, 0), 0);

        while (queue.TryDequeue(out var current, out var heat))
        {
            var (x, y, px, py) = current;

            if ((x, y) == end) return heat;
            if (seen.Add(current) is false) continue;

            foreach (var (dx, dy) in directions)
            {
                if ((dx == px && dy == py) || (dx == -px && dy == -py)) continue;

                int a = x, b = y, h = heat;

                for (int i = 1; i <= most; i++)
                {
                    a += dx;
                    b += dy;

                    if (blocks.TryGetValue((a, b), out var bh) is false) continue;

                    h += bh;

                    if (i < least) continue;

                    queue.Enqueue((a, b, dx, dy), h);
                }
            }
        }

        throw new Exception("Minimal heat not found.");
    }

    static Dictionary<(int x, int y), int> ParseBlocks(string input)
    {
        var blocks = input
            .Split(Environment.NewLine)
            .SelectMany((line, i) => line.Select((c, j) => ((i, j), c - '0')))
            .ToDictionary(x => x.Item1, x => x.Item2);

        return blocks;
    }
}