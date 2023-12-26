using AdventOfCode2023.Day5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2023.Dayz18;
internal static class LavaductLagoon
{
    public static long CorrectCapacity(string input)
    {
        var digPlan = DigPlanParser.CorrectParse(input);

        var perimeter = digPlan.Select(x => x.Steps).Sum();

        var trench = GetTrenchVertices(digPlan);

        long area = 0;

        for(int i = 1; i < trench.Count; i++)
        {
            long a = trench[i - 1].Row;
            long b = trench[i].Col;
            long c = trench[i - 1].Col;
            long d = trench[i].Row;
            
            var par = (a * b) - (c * d);

            area += (a * b) - (c * d);
        }

        area += (trench.Last().Row * trench.First().Col) - (trench.Last().Col * trench.First().Row);

        area = Math.Abs(area / 2);

        long result = area + (perimeter / 2) + 1;

        return result;    
    } 

    public static int Capacity(string input)
    {
        var digPlan = DigPlanParser.Parse(input);

        var trench = GetTrench(digPlan);

        DigLagoon(trench);

        var count = trench.Count;

        return (int)count;
    }

    static List<(int Row, int Col)> GetTrenchVertices((char Dir, int Steps)[] digPlan)
    {
        var trench = new List<(int Row, int Col)>();
        var curr = (Row: 0, Col: 0);

        for (int i = 0; i < digPlan.Length; i++)
        {
            var (dir, steps) = digPlan[i];

            var next = dir switch
            {
                'R' => (curr.Row, curr.Col + steps),
                'L' => (curr.Row, curr.Col - steps),
                'U' => (curr.Row - steps, curr.Col),
                'D' => (curr.Row + steps, curr.Col),
                _ => throw new ArgumentException($"Direction {dir} invalid."),
            };

            trench.Add(next);
            curr = trench.Last();
        }

        return trench;
    }

    static List<(int Row, int Col)> GetTrench((char Dir, int Steps)[] digPlan)
    {
        var trench = new List<(int Row, int Col)>();
        var curr = (0, 0);

        for (int i = 0; i < digPlan.Length; i++)
        {
            var (dir, steps) = digPlan[i];

            var next = Move(curr, dir);

            for (int j = 1; j <= steps; j++)
            {
                trench.Add(next);
                next = Move(next, dir);
            }

            curr = trench.Last();
        }

        return trench;
    }

    static List<(int Row, int Col)> DigLagoon(List<(int Row, int Col)> trench)
    {
        var minRow = trench.Select(x => x.Row).Min();

        var start = (minRow + 1, trench.First(x => x.Row == minRow).Col + 1);
        var toDo = new Stack<(int Row, int Col)>();
        toDo.Push(start);

        while (toDo.Any())
        {
            var (row, col) = toDo.Pop();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var pos = (row + i, col + j);

                    if (trench.Contains(pos)) continue;

                    trench.Add(pos);
                    toDo.Push(pos);
                }
            }
        }

        return trench;
    }

    static (int Row, int Col) Move((int Row, int Col) pos, char dir)
    {
        return dir switch
        {
            'R' => (pos.Row, pos.Col + 1),
            'L' => (pos.Row, pos.Col - 1),
            'U' => (pos.Row - 1, pos.Col),
            'D' => (pos.Row + 1, pos.Col),
            _ => throw new ArgumentException($"Direction {dir} invalid."),
        };
    }

}
