using Xunit;

namespace AdventOfCode2022.Day13;

public sealed class DistressSignalTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day13\\input_test1.txt");
        var result = DistressSignal.SumOrderedPairIndex(input);

        Assert.Equal(13, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day13\\input.txt");
        var result = DistressSignal.SumOrderedPairIndex(input);

        Assert.Equal(5605, result);
    }

    [Fact]
    public void Part2_Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day13\\input_test1.txt");
        var result = DistressSignal.GetDecoderKey(input);

        Assert.Equal(140, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2022\\Day13\\input.txt");
        var result = DistressSignal.GetDecoderKey(input);

        Assert.Equal(24969, result);
    }
}