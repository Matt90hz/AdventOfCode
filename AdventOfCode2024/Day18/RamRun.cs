using AdventOfCode2024.Day17;

namespace AdventOfCode2024.Day18;
public static class RamRun
{
    public static int MinimumStepToExit(string input, int size = 71, int bytes = 1024)
    {
        var memory = new char[size, size];
        var corruptedBits = ParseCorruptedBits(input).Take(bytes);
        var corruptedMemory = memory.Select((_, p) => corruptedBits.Contains(((int)p.Column, (int)p.Row)) ? '#' : '.');
        var minimumSteps = CalculateMinimumSteps(corruptedMemory);
        return minimumSteps;
    }

    private static int CalculateMinimumSteps(char[,] corruptedMemory)
    {
        var distances = corruptedMemory.Select(_ => -1);
        var start = corruptedMemory.GetPosition(0, 0);
        var end = corruptedMemory.GetPosition(corruptedMemory.GetLength(0) - 1, corruptedMemory.GetLength(1) - 1);
        var queue = new Queue<Position<char>>();

        queue.Enqueue(start);
        distances[start.Row, start.Column] = 0;

        while(queue.TryDequeue(out var current))
        {
            var adjacent = current.GetAdjacent().Where(x => distances[x.Row, x.Column] == -1 && corruptedMemory[x.Row, x.Column] == '.');
            var (currentRow, currentCol) = current;

            foreach (var a in adjacent)
            {
                distances[a.Row, a.Column] = distances[currentRow, currentCol] + 1;
                queue.Enqueue(a);
            }
        }

        return distances[end.Row, end.Column];
    }

    private static (int X, int Y)[] ParseCorruptedBits(string input)
    {
        var corruptedBits = input
            .Split(Environment.NewLine)
            .Select(line => line.Split(','))
            .Select(x => (int.Parse(x[0]),int.Parse(x[1])))
            .ToArray();

        return corruptedBits;
    }
}
