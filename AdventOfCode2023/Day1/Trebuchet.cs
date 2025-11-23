using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day1;

internal static class Trebuchet
{
    public static int Calibration(string input)
        => input
        .Split(Environment.NewLine)
        .Sum(line =>
        {
            var firstDigit = line.First(char.IsDigit);
            var lastDigit = line.Last(char.IsDigit);
            return int.Parse($"{firstDigit}{lastDigit}");
        });

    public static int Calibration2(string input) 
    {
        input = input
            .Replace("one", "o1e")
            .Replace("two", "t2")
            .Replace("three", "t3e")
            .Replace("four", "4")
            .Replace("five", "5e")
            .Replace("six", "6")
            .Replace("seven", "7n")
            .Replace("eight", "e8t")
            .Replace("nine", "n9e");

        return Calibration(input);
    }
}