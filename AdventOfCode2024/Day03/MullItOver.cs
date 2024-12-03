using AdventOfCode2024.Day02;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day03;
public static class MullItOver
{
    public static int SumAllMultiplications(string input)
    {
        var regex = @"(?<=mul\()\d+,\d+(?=\))";

        var matches = Regex.Matches(input, regex);

        var products = matches.Select(match =>
        {
            var values = match.Value
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var product = values[0] * values[1];

            return product;
        });

        var result = products.Sum();

        return result;
    }

    public static int SumAllEnabledMultiplications(string input)
    {
        var regex = @"do\(\).*?don\'t\(\)";

        var matches = Regex.Matches("do()" + input + "don't()", regex, RegexOptions.Singleline);

        var products = matches.Select(match => SumAllMultiplications(match.Value));

        var result = products.Sum();

        return result;
    }
}
