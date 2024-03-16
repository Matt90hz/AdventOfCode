using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day03;

public sealed class RucksackReorganizationTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day03\input_test1.txt");
        var result = RucksackReorganization.SumDuplicatePriorities(input);

        result.Should().Be(157);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day03\input.txt");
        var result = RucksackReorganization.SumDuplicatePriorities(input);

        result.Should().Be(7878);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day03\input_test1.txt");
        var result = RucksackReorganization.SumGroupPriorities(input);

        result.Should().Be(70);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2022\Day03\input.txt");
        var result = RucksackReorganization.SumGroupPriorities(input);

        result.Should().Be(2760);
    }
}