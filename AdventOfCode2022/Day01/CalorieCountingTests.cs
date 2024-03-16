using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day01;

public sealed class CalorieCountingTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day01\input_test1.txt");
        var result = CalorieCounting.MaxCalorie(input);

        result.Should().Be(24000);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day01\input.txt");
        var result = CalorieCounting.MaxCalorie(input);

        result.Should().Be(71934);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day01\input_test1.txt");
        var result = CalorieCounting.MaxCalorieTop3(input);

        result.Should().Be(45000);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day01\input.txt");
        var result = CalorieCounting.MaxCalorieTop3(input);

        result.Should().Be(211447);
    }

}
