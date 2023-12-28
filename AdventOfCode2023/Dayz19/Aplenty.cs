using IncaTechnologies.Collection.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode2023.Dayz19;
internal static class Aplenty
{
    public static long AllPossibleAccepted(string input)
    {
        var sections = input.Split(Environment.NewLine);

        var workflows = sections
            .TakeWhile(line => line != string.Empty)
            .Select(GetWorkflow)
            .ToArray();

        var workflow = workflows.First(x => x.Code == "in");

        var combo = AllPossibleAccepted(workflow, workflows);

        var result = combo
            .Select(x => ((long)x.XMax - (long)x.XMin - 1) * ((long)x.MMax - (long)x.MMin - 1) * ((long)x.AMax - (long)x.AMin - 1) * ((long)x.SMax - (long)x.SMin - 1))
            .Sum();

        return result;
    }

    static (int XMin, int XMax, int MMin, int MMax, int AMin, int AMax, int SMin, int SMax)[] AllPossibleAccepted((string Code,
        (char Part, char Comp, int Cond, string Dest)[] Rules) workflow,
        (string Code, (char Part, char Comp, int Cond, string Dest)[] Rules)[] workflows)
    {
        //no more rules to apply
        if (workflow.Rules.Any() is false) return Array.Empty<(int XMin, int XMax, int MMin, int MMax, int AMin, int AMax, int SMin, int SMax)>();

        var (part, comp, cond, dest) = workflow.Rules.First();

        //the rule is a final rule and has a result
        if (part == 't' && dest == "R") return Array.Empty<(int XMin, int XMax, int MMin, int MMax, int AMin, int AMax, int SMin, int SMax)>();

        if (part == 't' && dest == "A")
            //return new[] { (1, 4000, 1, 4000, 1, 4000, 1, 4000) };
            return new[] { (0, 4001, 0, 4001, 0, 4001, 0, 4001) };

        if (part == 't')
        {

            var nextWorkflow1 = AllPossibleAccepted(workflows.First(x => x.Code == dest), workflows);
            var nextRule1 = AllPossibleAccepted(workflow with { Rules = workflow.Rules.Skip(1).ToArray() }, workflows);

            return nextWorkflow1.Concat(nextRule1).ToArray();
        }

        //the rule is not final but has a result
        if (dest == "A")
        {
            var result = RestrictRange((part, comp, cond), (0, 4001, 0, 4001, 0, 4001, 0, 4001));
            //var result = RestrictRange((part, comp, cond), (1, 4000, 1, 4000, 1, 4000, 1, 4000));

            var nextRule1 = AllPossibleAccepted(workflow with { Rules = workflow.Rules.Skip(1).ToArray() }, workflows)
                .Select(x => RestrictRange(ReverseCondition((part, comp, cond)), x))
                .ToArray();

            return new[] { result }.Concat(nextRule1).ToArray();
        }

        if (dest == "R")
        {
            return AllPossibleAccepted(workflow with { Rules = workflow.Rules.Skip(1).ToArray() }, workflows)
                .Select(x => RestrictRange(ReverseCondition((part, comp, cond)), x))
                .ToArray();
        }

        //the rules is not final and do not has a result
        //true
        var nextWorkflow = AllPossibleAccepted(workflows.First(x => x.Code == dest), workflows)
            .Select(x => RestrictRange((part, comp, cond), x))
            .ToArray();

        //false
        var nextRule = AllPossibleAccepted(workflow with { Rules = workflow.Rules.Skip(1).ToArray() }, workflows)
            .Select(x => RestrictRange(ReverseCondition((part, comp, cond)), x))
            .ToArray();

        // remove the 0 and add this rule
        var allComb = nextWorkflow
            .Concat(nextRule)
            .ToArray();


        return allComb;

    }

    static (char Part, char Comp, int Cond) ReverseCondition((char Part, char Comp, int Cond) condition)
    {
        var(part, comp, cond) = condition;

        var newComp = comp == '<' ? '>' : '<';
        var newCond = newComp == '<' ? cond + 1 : cond - 1;

        return (part, newComp, newCond);
    }

    static (int XMin, int XMax, int MMin, int MMax, int AMin, int AMax, int SMin, int SMax) RestrictRange(
        (char Part, char Comp, int Cond) condition,
        (int XMin, int XMax, int MMin, int MMax, int AMin, int AMax, int SMin, int SMax) range)
    {
        var (part, comp, cond) = condition;

        //var restrictRange = comp switch
        //{
        //    '<' => part switch
        //    {
        //        'x' => (1, cond - 1, 1, 4000, 1, 4000, 1, 4000),
        //        'm' => (1, 4000, 1, cond - 1, 1, 4000, 1, 4000),
        //        'a' => (1, 4000, 1, 4000, 1, cond - 1, 1, 4000),
        //        's' => (1, 4000, 1, 4000, 1, 4000, 1, cond - 1),
        //        _ => throw new ArgumentException($"[{part}] is an invalid part category.")
        //    },
        //    '>' => part switch
        //    {
        //        'x' => (cond + 1, 4000, 1, 4000, 1, 4000, 1, 4000),
        //        'm' => (1, 4000, cond + 1, 4000, 1, 4000, 1, 4000),
        //        'a' => (1, 4000, 1, 4000, cond + 1, 4000, 1, 4000),
        //        's' => (1, 4000, 1, 4000, 1, 4000, cond + 1, 4000),
        //        _ => throw new ArgumentException($"[{part}] is an invalid part category.")
        //    },
        //    _ => throw new ArgumentException($"[{comp}] is an invalid comparison operator.")
        //};

        var restrictRange = comp switch
        {
            '<' => part switch
            {
                'x' => (0, cond, 0, 4001, 0, 4001, 0, 4001),
                'm' => (0, 4001, 0, cond, 0, 4001, 0, 4001),
                'a' => (0, 4001, 0, 4001, 0, cond, 0, 4001),
                's' => (0, 4001, 0, 4001, 0, 4001, 0, cond),
                _ => throw new ArgumentException($"[{part}] is an invalid part category.")
            },
            '>' => part switch
            {
                'x' => (cond, 4001, 0, 4001, 0, 4001, 0, 4001),
                'm' => (0, 4001, cond, 4001, 0, 4001, 0, 4001),
                'a' => (0, 4001, 0, 4001, cond, 4001, 0, 4001),
                's' => (0, 4001, 0, 4001, 0, 4001, cond, 4001),
                _ => throw new ArgumentException($"[{part}] is an invalid part category.")
            },
            _ => throw new ArgumentException($"[{comp}] is an invalid comparison operator.")
        };

        return Combine(range, restrictRange);
    }

    private static (int XMin, int XMax, int MMin, int MMax, int AMin, int AMax, int SMin, int SMax) Combine(
        (int XMin, int XMax, int MMin, int MMax, int AMin, int AMax, int SMin, int SMax) x,
        (int XMin, int XMax, int MMin, int MMax, int AMin, int AMax, int SMin, int SMax) y)
    {
        var xMin = x.XMin > y.XMin ? x.XMin : y.XMin;
        var xMax = x.XMax < y.XMax ? x.XMax : y.XMax;
        var mMin = x.MMin > y.MMin ? x.MMin : y.MMin;
        var mMax = x.MMax < y.MMax ? x.MMax : y.MMax;
        var aMin = x.AMin > y.AMin ? x.AMin : y.AMin;
        var aMax = x.AMax < y.AMax ? x.AMax : y.AMax;
        var sMin = x.SMin > y.SMin ? x.SMin : y.SMin;
        var sMax = x.SMax < y.SMax ? x.SMax : y.SMax;

        return (xMin, xMax, mMin, mMax, aMin, aMax, sMin, sMax);

    }

    public static int Accepted(string input)
    {
        var sections = input.Split(Environment.NewLine);

        var workflows = sections
            .TakeWhile(line => line != string.Empty)
            .Select(GetWorkflow)
            .ToArray();

        var ratings = sections
            .SkipWhile(line => line != string.Empty)
            .Skip(1)
            .Select(GetRating)
            .ToArray();

        var workflow = workflows.First(x => x.Code == "in");
        int result = 0;

        foreach (var rating in ratings)
        {
            while (true)
            {
                var next = rating.Apply(workflow);

                if (next == "R") break;

                if (next == "A")
                {
                    result += rating.X + rating.M + rating.A + rating.S;
                    break;
                }

                workflow = workflows.First(x => x.Code == next);
            }

            workflow = workflows.First(x => x.Code == "in");
        }

        return result;
    }

    private static string Apply(this (int X, int M, int A, int S) rating, (string Code, (char Part, char Comp, int Cond, string Dest)[]) workflow)
    {
        var (_, rules) = workflow;

        foreach (var (part, comp, cond, dest) in rules)
        {
            if (part == 't') return dest;

            var isCond = comp switch
            {
                '<' => part switch
                {
                    'x' => rating.X < cond,
                    'm' => rating.M < cond,
                    'a' => rating.A < cond,
                    's' => rating.S < cond,
                },
                '>' => part switch
                {
                    'x' => rating.X > cond,
                    'm' => rating.M > cond,
                    'a' => rating.A > cond,
                    's' => rating.S > cond,
                }
            };

            if (isCond) return dest;
        }

        throw new Exception("Cannot find a result!");
    }

    static (int X, int M, int A, int S) GetRating(string line)
    {
        var parts = line[1..^1]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Split('=', StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        var x = int.Parse(parts[0][1]);
        var m = int.Parse(parts[1][1]);
        var a = int.Parse(parts[2][1]);
        var s = int.Parse(parts[3][1]);

        return (x, m, a, s);
    }

    static (string Code, (char Part, char Comp, int Cond, string Dest)[] Rules) GetWorkflow(string line)
    {
        var code = line[..line.IndexOf('{')];
        var rules = line[(line.IndexOf('{') + 1)..line.IndexOf('}')]
            .Split(',')
            .Select(GetRule)
            .ToArray();

        return (code, rules);
    }

    static (char Part, char Comp, int Cond, string Dest) GetRule(string line)
    {
        if (line.Contains(':') is false)
        {
            return ('t', '=', 0, line);
        }

        var part = line[0];
        var comp = line[1];
        var cond = int.Parse(line[2..line.IndexOf(':')]);
        var dest = line[(line.IndexOf(':') + 1)..];

        return (part, comp, cond, dest);
    }


}
