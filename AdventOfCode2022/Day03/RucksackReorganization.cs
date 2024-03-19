using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day03;
static class RucksackReorganization
{
    public static int SumDuplicatePriorities(string input) => input
        .Split(Environment.NewLine)
        .Select(line => (Comp1: line[..(line.Length / 2)], Comp2: line[(line.Length / 2)..]))
        .Select(rucksac => rucksac.Comp1.Intersect(rucksac.Comp2).First())
        .Select(GetPriority)
        .Sum();

    public static int SumGroupPriorities(string input) => input
        .Split(Environment.NewLine)
        .Chunk(3)
        .Select(group => group[0]
            .Intersect(group[1])
            .Intersect(group[2])
            .First())
        .Select(GetPriority)
        .Sum();

    static int GetPriority(char c) => char.IsUpper(c) 
        ? c - 64 + 26
        : c - 96;

}
