using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day1;

internal static class Trebuchet
{
    public static int Calibration(string input)
    {
        var lines = input.Split('\n');

        var coordinates = lines.Select(line => CombineDigits(FirstDigit(line), LastDigit(line)));

        var sum = coordinates.Sum();

        return sum;
    }

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

    static char FirstDigit(string line) => line.First(char.IsDigit);

    static char LastDigit(string line) => line.Last(char.IsDigit);

    static int CombineDigits(char first, char last) => int.Parse($"{first}{last}");

}


