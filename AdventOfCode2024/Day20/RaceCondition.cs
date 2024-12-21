using AdventOfCode2024.Day19;
using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2024.Day20;
public static class RaceCondition
{
    public static int CountGoodCheats(string input, int cheatPicosecond = 20)
    {
        var track = ParseTrack(input);
        var distancesToExit = CalculateDistancesToExit(track);
        var trackPositions = distancesToExit.GetPositions().Where(x => x.Value > 0);
        var shortCuts = trackPositions.Sum(x =>
        {
            var adjacent = x.GetReachable(cheatPicosecond).Where(x => x.Value != -1);
            var shortCuts = adjacent.Where(shortcut =>
            {
                var distance = Math.Abs(x.Row - shortcut.Row) + Math.Abs(x.Column - shortcut.Column);
                return (x.Value - shortcut.Value - distance) >= 100;
            });
            return shortCuts.Count();
        });
        return shortCuts;
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