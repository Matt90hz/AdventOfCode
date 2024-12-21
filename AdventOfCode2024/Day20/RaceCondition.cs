using AdventOfCode2024.Day19;

namespace AdventOfCode2024.Day20;
public static class RaceCondition
{
    public static int CountGoodCheats(string input)
    {
        var track = ParseTrack(input);
        var distancesToExit = CalculateDistancesToExit(track);
        var trackPositions = distancesToExit.GetPositions().Where(x => x.Value > 0);
        var shortCuts = trackPositions.SelectMany(x =>
        {
            var adjacent = x.GetAdjacent().Where(x => x.Value == -1);
            var shortCuts = adjacent.Select(a =>
            {
                var shortcut = a.Move(x.GetDirection(a));
                if (shortcut.IsOutOfBound() || shortcut.Value == -1) return -1;
                var saved = x.Value - shortcut.Value - 2;
                return saved;
            });
            return shortCuts;
        });
        var goodShortcuts = shortCuts.Where(x => x >= 100);
        return goodShortcuts.Count();
    }

    public static int CountLegalCheats(string input)
    {
        var track = ParseTrack(input);
        var distancesToExit = CalculateDistancesToExit(track);
        var trackPositions = distancesToExit.GetPositions().Where(x => x.Value > 0);
        var shortCuts = trackPositions.SelectMany(x =>
        {
            var adjacent = x.GetAdjacent().Where(x => x.Value == -1);
            var shortCuts = adjacent.Select(a =>
            {
                var shortcut = a.Move(x.GetDirection(a));
                if (shortcut.IsOutOfBound() || shortcut.Value == -1) return -1;
                var saved = x.Value - shortcut.Value - 2;
                return saved;
            });
            return shortCuts;
        });
        var goodShortcuts = shortCuts.Where(x => x >= 100);
        return goodShortcuts.Count();
    }

    private static int[,] CalculateDistancesToExit(char[,] track)
    {
        var (exitRow, exitCol) = track.FindPosition('E') ?? throw new Exception("Exit not found");
        var distances = track.Select(_ => -1);
        var exit = distances.GetPosition(exitRow, exitCol);
        var queue = new Queue<Position<int>>();
        exit.Value = 0;
        queue.Enqueue(exit);

        while(queue.TryDequeue(out var position))
        {
            var a = position.GetAdjacent().Where(x => x.Value == -1 && track[x.Row, x.Column] != '#');

            foreach(var x in a)
            {
                x.Value = position.Value + 1;
                queue.Enqueue(x);
            }
        }

        return distances;
    }

    private static char[,] ParseTrack(string input)
    {
        var track = input.Split(Environment.NewLine).ToMultidimensionalArray();
        return track;
    }
}