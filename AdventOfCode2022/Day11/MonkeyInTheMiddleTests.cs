using FluentAssertions;
using System.Numerics;
using Xunit;

namespace AdventOfCode2022.Day11;

public sealed class MonkeyInTheMiddleTests
{
    [Fact]
    public void Part1_Test1()
    {
        string input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day11\\input_test1.txt");
        int result = MonkeyInTheMiddle.GetMonkeyBusiness(input);

        result.Should().Be(10605);
    }

    [Fact]
    public void Part1_Solution()
    {
        string input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day11\\input.txt");
        int result = MonkeyInTheMiddle.GetMonkeyBusiness(input);

        result.Should().Be(55216);
    }

    [Fact]
    public void Part2_Test1()
    {
        string input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day11\\input_test1.txt");
        ulong result = MonkeyInTheMiddle.GetLongMonkeyBusiness(input);

        result.Should().Be(2713310158);
    }

    [Fact]
    public void Part2_Solution()
    {
        string input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day11\\input.txt");
        ulong result = MonkeyInTheMiddle.GetLongMonkeyBusiness(input);

        result.Should().Be(12848882750);
    }
}