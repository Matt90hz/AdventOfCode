using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day08;

public sealed class TreetopTreeHouseTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day08\\input_test1.txt");
        var result = TreetopTreeHouse.CountTreesVisible(input);

        result.Should().Be(21);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day08\\input.txt");
        var result = TreetopTreeHouse.CountTreesVisible(input);

        result.Should().Be(1715);
    }
}