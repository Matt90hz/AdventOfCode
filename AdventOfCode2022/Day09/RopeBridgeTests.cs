using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day09;

public sealed class RopeBridgeTests
{
    [Fact]
    public void Part1_Test1()
    {
        string input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day09\\input_test1.txt");
        int result = RopeBridge.CountTailVisitedPositions(input);

        result.Should().Be(13);
    }

    [Fact]
    public void Part1_Solution()
    {
        string input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day09\\input.txt");
        int result = RopeBridge.CountTailVisitedPositions(input);

        result.Should().Be(6498);
    }

    [Fact]
    public void Part2_Test1()
    {
        string input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day09\\input_test1.txt");
        int result = RopeBridge.CountTailVisitedPositionsOfLastKnot(input);

        result.Should().Be(1);
    }

    [Fact]
    public void Part2_Test2()
    {
        string input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day09\\input_test2.txt");
        int result = RopeBridge.CountTailVisitedPositionsOfLastKnot(input);

        result.Should().Be(36);
    }
    [Fact]
    public void Part2_Solution()
    {
        string input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day09\\input.txt");
        int result = RopeBridge.CountTailVisitedPositionsOfLastKnot(input);

        result.Should().Be(2531);
    }
}
