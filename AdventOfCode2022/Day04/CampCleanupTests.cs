using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day04;

public sealed class CampCleanupTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day04\\input_test1.txt");
        var result = CampCleanup.CountRedundants(input);

        result.Should().Be(2);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day04\\input.txt");
        var result = CampCleanup.CountRedundants(input);

        result.Should().Be(542);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day04\\input_test1.txt");
        var result = CampCleanup.CountPartiallyRedundants(input);

        result.Should().Be(4);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day04\\input.txt");
        var result = CampCleanup.CountPartiallyRedundants(input);

        result.Should().Be(900);
    }
}