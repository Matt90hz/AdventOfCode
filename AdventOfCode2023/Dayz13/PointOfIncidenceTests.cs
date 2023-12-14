using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz13;
public static class PointOfIncidenceTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz13\input_test1.txt");
        var result = PointOfIncidence.Summarize(input);
        Assert.Equal(405, result);
    }

    [Fact]
    public static void Part1Test2()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz13\input_test2.txt");
        var result = PointOfIncidence.Summarize(input);
        Assert.Equal(709, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz13\input.txt");
        var result = PointOfIncidence.Summarize(input);
        Assert.Equal(43614, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz13\input_test3.txt");
        var result = PointOfIncidence.SummarizeSmudge(input);
        Assert.Equal(400, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz13\input.txt");
        var result = PointOfIncidence.SummarizeSmudge(input);
        Assert.Equal(36771, result);
    }
}
