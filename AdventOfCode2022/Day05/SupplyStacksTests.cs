using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day05;

public sealed class SupplyStacksTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day05\\input_test1.txt");
        var result = SupplyStacks.GetTopCrates(input);

        result.Should().Be("CMZ");
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day05\\input.txt");
        var result = SupplyStacks.GetTopCrates(input);

        result.Should().Be("RFFFWBPNS");
    }

    [Fact]
    public void Part2_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day05\\input_test1.txt");
        var result = SupplyStacks.GetTopCrates9001(input);

        result.Should().Be("MCD");
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day05\\input.txt");
        var result = SupplyStacks.GetTopCrates9001(input);

        result.Should().Be("CQQBBJFCS");
    }
}