using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day14;

public sealed class RegolithReservoirTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day14\\input_test1.txt");
        var result = RegolithReservoir.MaxSendToRest(input);

        result.Should().Be(24);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day14\\input.txt");
        var result = RegolithReservoir.MaxSendToRest(input);

        result.Should().Be(625);
    }

    [Fact]
    public void Part2_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day14\\input_test1.txt");
        var result = RegolithReservoir.CountSandToBlockInlet(input);

        result.Should().Be(93);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day14\\input.txt");
        var result = RegolithReservoir.CountSandToBlockInlet(input);

        result.Should().Be(25193);
    }
}
