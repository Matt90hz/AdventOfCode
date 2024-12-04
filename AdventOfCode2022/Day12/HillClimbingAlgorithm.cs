using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2022.Day12;

static class HillClimbingAlgorithm
{
    public static int GetShortestPathToExit(string input)
    {
        var map = input
            .Split(Environment.NewLine)
            .ToMultidimensionalArray();

        var distances = map.Select(c => c.IsStartPosition() ? 0 : -1);

        Queue<IPosition<char>> positionsToEvaluate = new (map.GetPositions().Where(pos => pos.Value.IsStartPosition()));

        while (positionsToEvaluate.Count > 0)
        {
            var positionToEvalute = positionsToEvaluate.Dequeue();

            var nextPositionsToEvaluate = positionToEvalute
                .GetAdjacent()
                .Where(adjecent =>
                    CanClimb(positionToEvalute.Value, adjecent.Value)
                    && distances[adjecent.Row, adjecent.Column] == -1)
                .ToArray();

            foreach (var nextPositionToEvaluate in nextPositionsToEvaluate)
            {
                distances[nextPositionToEvaluate.Row, nextPositionToEvaluate.Column] = 
                    distances[positionToEvalute.Row, positionToEvalute.Column] + 1;

                positionsToEvaluate.Enqueue(nextPositionToEvaluate);
            }
        }

        var exit = map
            .GetPositions()
            .First(position => position.Value.IsBestSignalSpot());

        return distances[exit.Row, exit.Column];
    }

    public static int GetShortestScenicPath(string input)
    {
        var map = input
            .Split(Environment.NewLine)
            .ToMultidimensionalArray();

        var distances = map.Select(c => c.IsBestSignalSpot() ? 0 : -1);

        Queue<IPosition<char>> positionsToEvaluate = new(map.GetPositions().Where(pos => pos.Value.IsBestSignalSpot()));

        while (positionsToEvaluate.Count > 0)
        {
            var positionToEvalute = positionsToEvaluate.Dequeue();

            var nextPositionsToEvaluate = positionToEvalute
                .GetAdjacent()
                .Where(adjecent =>
                    CanClimbDown(positionToEvalute.Value, adjecent.Value)
                    && distances[adjecent.Row, adjecent.Column] == -1)
                .ToArray();

            foreach (var nextPositionToEvaluate in nextPositionsToEvaluate)
            {
                distances[nextPositionToEvaluate.Row, nextPositionToEvaluate.Column] =
                    distances[positionToEvalute.Row, positionToEvalute.Column] + 1;

                positionsToEvaluate.Enqueue(nextPositionToEvaluate);
            }
        }

        var starts = map
            .GetPositions()
            .Where(position => position.Value == 'a');

        return starts
            .Select(start => distances[start.Row, start.Column])
            .Where(distance => distance != -1)
            .Min();
    }

    static int GetHeight(this char c) => c switch
    {
        'S' => GetHeight('a'),
        'E' => GetHeight('z'),
        _ => c - 'a'
    };

    static bool IsStartPosition(this char c) => c == 'S';

    static bool IsBestSignalSpot(this char c) => c == 'E';

    static bool CanClimb(char from, char to) => (to.GetHeight() - from.GetHeight()) < 2;

    static bool CanClimbDown(char from , char to) => (from.GetHeight() - to.GetHeight()) < 2;
}
