using Xunit;

namespace AdventOfCode2024.Day05;

public sealed class PrintQueueTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day05\input_test.txt");
        var result = PrintQueue.SumMiddlePageOfCorrectUpdates(input);

        Assert.Equal(143, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day05\input.txt");
        var result = PrintQueue.SumMiddlePageOfCorrectUpdates(input);

        Assert.Equal(4462, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day05\input_test.txt");
        var result = PrintQueue.SumMiddlePageOfCorrectedUpdates(input);

        Assert.Equal(123, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day05\input.txt");
        var result = PrintQueue.SumMiddlePageOfCorrectedUpdates(input);

        Assert.Equal(6767, result);
    }
}