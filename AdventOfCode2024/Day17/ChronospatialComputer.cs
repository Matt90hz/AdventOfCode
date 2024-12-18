using AdventOfCode2024.Day16;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day17;
public static class ChronospatialComputer
{
    private static long _a;
    private static long _b;
    private static long _c;
    private static List<int> _output = [];
    private static readonly Dictionary<(int A, int B, int C, int I, int O, int OP), List<int>> _cache = [];
    private static int _instruction = -1;

    public static string Execute(string input)
    {
        var (a, b, c, program) = ParseState(input);

        var output = Run(a, b, c, program);

        var str_output = new StringBuilder().AppendJoin(',', output).ToString();

        return str_output;
    }

    public static long ExecuteDebug(string input)
    {
        (_, var b, var c, var program) = ParseState(input);

        return FindA(program, 0, b, c, 1);
    }

    public static long FindA(int[] program, long a, long b, long c, int prg_pos)
    {
        if (prg_pos > program.Length) return a;

        for (int i = 0; i < 8; i++)
        {
            var possible_a = a * 8 + i;

            int first_digit_out = Run(possible_a, b, c, program)[0];

            if (first_digit_out != program[^prg_pos]) continue;

            var e = FindA(program, possible_a, b, c, prg_pos + 1);

            if (e != -1) return e;
        }

        return -1;
    }

    static List<int> Run(long a, long b, long c, int[] program)
    {
        int instruction = 0;
        var output = new List<int>();
        int last = program.Length - 1;

        while (instruction < last)
        {
            var opcode = program[instruction];
            var literal = program[instruction + 1];
            var combo = literal switch
            {
                < 4 => literal,
                4 => a,
                5 => b,
                6 => c,
                _ => throw new InvalidOperationException("Reserved operand")
            };

            instruction += 2;

            switch (opcode)
            {
                case 0: a /= (long)Math.Pow(2, combo); break;
                case 1: b ^= literal; break;
                case 2: b = combo % 8; break;
                case 3 when a != 0: instruction = literal; break;
                case 4: b ^= c; break;
                case 5: output.Add((int)(combo % 8)); break;
                case 6: b = a / (long)Math.Pow(2, combo); break;
                case 7: c = a / (long)Math.Pow(2, combo); break;
            }
        }

        return output;
    }

    private static (int A, int B, int C, int[] Program) ParseState(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var a = int.Parse(lines[0][12..]);
        var b = int.Parse(lines[1][12..]);
        var c = int.Parse(lines[2][12..]);
        var program = lines[3][9..].Split(',').Select(int.Parse).ToArray();

        return (a, b, c, program);
    }
}
