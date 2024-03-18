using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day05;
static class SupplyStacks
{
    public static string GetTopCrates(string input)
    {
        var stacks = GetStacks(input);
        var moves = GetMovements(input);

        foreach (var move in moves)
        {
            stacks.MoveContainer(move);
        }

        return stacks
            .Select(stack => stack.Peek())
            .Aggregate(string.Empty, (line, c) => line += c);
    }

    public static string GetTopCrates9001(string input)
    {
        var stacks = GetStacks(input);
        var moves = GetMovements(input);

        foreach (var move in moves)
        {
            stacks.MoveContainer9001(move);
        }

        return stacks
            .Select(stack => stack.Peek())
            .Aggregate(string.Empty, (line, c) => line += c);
    }

    static Stack<char>[] GetStacks(string input) => input
        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        .Where(line => line.StartsWith("move") is false && line.Any(char.IsDigit) is false)
        .Reverse()
        .Aggregate((Stack<char>[]?)default!,
        (stack, line) =>
        {
            line = "  " + line;
            stack ??= new Stack<char>[line.Length / 4];

            for (int i = 0; i < line.Length; i++)
            {
                if (char.IsLetter(line[i])) 
                {
                    var stackIndex = (i + 1) / 4 - 1;
                    stack[stackIndex] ??= new Stack<char>();            
                    stack[stackIndex].Push(line[i]); 
                }
            } 

            return stack;
        });

    static IEnumerable<(int Moves, int From, int To)> GetMovements(string input) => input
        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        .Where(line => line.StartsWith("move"))
        .Select(line => line
            .Replace("move", string.Empty)
            .Replace("from", string.Empty)
            .Replace("to", string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries))
        .Select(values => (int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2])));

    static Stack<char>[] MoveContainer(this Stack<char>[] stacks, (int Moves, int From, int To) moves) => Enumerable
        .Range(0, moves.Moves)
        .Aggregate(stacks, 
        (_, _) =>
        {
            var (_, from, to) = moves;
            var crate = stacks[from - 1].Pop();
            stacks[to - 1].Push(crate);
            return stacks;
        });

    static Stack<char>[] MoveContainer9001(this Stack<char>[] stacks, (int Moves, int From, int To) moves) => Enumerable
        .Range(0, moves.Moves)
        .Select(_ => stacks[moves.From - 1].Pop())
        .Reverse()
        .Aggregate(stacks,
        (_, crate) =>
        {
            stacks[moves.To - 1].Push(crate);
            return stacks;
        });

}
