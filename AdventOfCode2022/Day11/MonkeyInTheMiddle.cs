using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day11;

record Monkey(int Id, ulong[] StaringItems, Operation Operation, Test Test);
record Operation(char Operator, uint Value);
record Test(string Comparison, uint Value, int IfTrue, int IfFalse);

static class MonkeyInTheMiddle
{
    public static int GetMonkeyBusiness(string input) => Enumerable
        .Range(1, 20)
        .Aggregate(GetMonkeys(input).Select(monkey => (Inspections: 0, Monkey: monkey)).ToArray(),
        (monkeysInspections, _) => monkeysInspections.Aggregate(monkeysInspections,
        (round, data) =>
        {
            var (inspections, monkey) = data;

            var nextRound = monkey.StaringItems
                .Select(item =>
                {
                    ulong worryLevelPostInspection = monkey.Inspect(item) / 3;
                    inspections++;
                    int throwTo = monkey.Test(worryLevelPostInspection);
                    return (worryLevelPostInspection, throwTo);
                })
                .Aggregate(round,
                (_, itemTrown) =>
                {
                    var (worryLevel, monkeyId) = itemTrown;

                    var receiver = round[monkeyId].Monkey;

                    round[monkeyId] = round[monkeyId] with
                    {
                        Monkey = receiver with
                        {
                            StaringItems = [.. receiver.StaringItems, worryLevel]
                        }
                    };

                    return round;
                });

            nextRound[monkey.Id] = (inspections, monkey with { StaringItems = [] });

            return nextRound;

        }))
        .Select(x => x.Inspections)
        .OrderDescending()
        .Take(2)
        .Aggregate((total, inspection) => total *= inspection);

    public static ulong GetLongMonkeyBusiness(string input) => Enumerable
        .Range(1, 10000)
        .Aggregate(GetMonkeys(input).Select(monkey => (Inspections: 0ul, Monkey: monkey)).ToArray(),
        (monkeysInspections, _) => monkeysInspections.Aggregate(monkeysInspections,
        (round, data) =>
        {
            var (inspections, monkey) = data;

            var globalModulo = round
                .Select(data => data.Monkey.Test.Value)
                .Aggregate((modulo, divisor) => modulo * divisor);

            var nextRound = monkey.StaringItems
                .Select(item =>
                {
                    inspections++;
                    ulong worryLevelPostInspection = monkey.Inspect(item) % globalModulo;
                    int throwTo = monkey.Test(worryLevelPostInspection);
                    return (worryLevelPostInspection, throwTo);
                })
                .Aggregate(round,
                (_, itemTrown) =>
                {
                    var (worryLevel, monkeyId) = itemTrown;

                    var receiver = round[monkeyId].Monkey;

                    round[monkeyId] = round[monkeyId] with
                    {
                        Monkey = receiver with
                        {
                            StaringItems = [.. receiver.StaringItems, worryLevel]
                        }
                    };

                    return round;
                });

            nextRound[monkey.Id] = (inspections, monkey with { StaringItems = [] });

            return nextRound;
        }))
        .Select(x => x.Inspections)
        .OrderDescending()
        .Take(2)
        .Aggregate((total, inspection) => total *= inspection);

    static Monkey[] GetMonkeys(string input) => input
        .Split(Environment.NewLine)
        .Chunk(7)
        .Select(lines => new Monkey(
            Id: lines[0][7] - '0',
            StaringItems: lines[1].Split(':')[1].Split(',').Select(ulong.Parse).ToArray(),
            Operation: new Operation(
                Operator: lines[2].Contains('*') ? '*' : '+',
                Value: uint.TryParse(lines[2].Split('+', '*')[1], out uint val) ? val : 0),
            Test: new Test(
                Comparison: string.Empty,
                Value: uint.Parse(lines[3].Split("by")[1]),
                IfTrue: lines[4][^1] - '0',
                IfFalse: lines[5][^1] - '0')))
        .ToArray();

    static ulong Inspect(this Monkey monkey, ulong item) => monkey.Operation switch
    {
        ('*', > 0) => item * monkey.Operation.Value,
        ('+', > 0) => item + monkey.Operation.Value,
        ('*', _) => item * item,
        ('+', _) => item + item,
        _ => 0
    };

    static int Test(this Monkey monkey, ulong worryLevel) => worryLevel % monkey.Test.Value == 0
        ? monkey.Test.IfTrue
        : monkey.Test.IfFalse;

}
