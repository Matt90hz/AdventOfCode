using FluentAssertions.Equivalency.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day09;
static class RopeBridge
{
    public static int CountTailVisitedPositions(string input) => input
        .Split(Environment.NewLine)
        .SelectMany(line => Enumerable.Repeat(line[0], int.Parse(line[2..])))
        .FollowPath()
        .FollowPath()
        .Distinct()
        .Count();

    public static int CountTailVisitedPositionsOfLastKnot(string input)
    {
        var headPath = input
            .Split(Environment.NewLine)
            .SelectMany(line => Enumerable.Repeat(line[0], int.Parse(line[2..])))
            .FollowPath();

        return Enumerable.Range(1, 9)
            .Aggregate(headPath, (nextPath, _) => nextPath.FollowPath())
            .Distinct()
            .Count();
    }

    static List<(int Row, int Col)> FollowPath(this IEnumerable<char> steps) => steps
        .Aggregate(new List<(int Row, int Col)>() { (Row: 0, Col: 0) },
        (path, step) =>
        {
            var last = path[^1];

            var next = step switch
            {
                'D' => last with { Row = last.Row + 1 },
                'U' => last with { Row = last.Row - 1 },
                'R' => last with { Col = last.Col + 1 },
                'L' => last with { Col = last.Col - 1 },
                _ => last
            };

            path.Add(next);

            return path;
        });

    static List<(int Row, int Col)> FollowPath(this List<(int Row, int Col)> pathToFollow) => pathToFollow
        .Aggregate(new List<(int Row, int Col)>() { (Row: 0, Col: 0) },
        (tailPath, headPos) =>
        {
            var tailPos = tailPath[^1];

            (int Row, int Col) nextTailPos = (tailPos.Row - headPos.Row, tailPos.Col - headPos.Col) switch
            {
                (-1 or 0 or 1, -1 or 0 or 1) => tailPos,
                (0, var dCol) => tailPos with { Col = dCol < 0 ? tailPos.Col + 1 : tailPos.Col - 1 },
                (var dRow, 0) => tailPos with { Row = dRow < 0 ? tailPos.Row + 1 : tailPos.Row - 1 },
                (var dRow, var dCol) => tailPos with
                {
                    Col = dCol < 0 ? tailPos.Col + 1 : tailPos.Col - 1,
                    Row = dRow < 0 ? tailPos.Row + 1 : tailPos.Row - 1
                }
            };

            tailPath.Add(nextTailPos);

            return tailPath;
        });

}
