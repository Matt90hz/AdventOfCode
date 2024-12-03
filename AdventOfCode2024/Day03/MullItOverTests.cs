using Xunit;

namespace AdventOfCode2024.Day03;

public sealed class MullItOverTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(".\\Day03\\input_test.txt");
        var result = MullItOver.SumAllMultiplications(input);
        Assert.Equal(161, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(".\\Day03\\input.txt");
        var result = MullItOver.SumAllMultiplications(input);
        Assert.Equal(170068701, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(".\\Day03\\input_test.txt");
        var result = MullItOver.SumAllEnabledMultiplications(input);
        Assert.Equal(48, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(".\\Day03\\input.txt");
        var result = MullItOver.SumAllEnabledMultiplications(input);
        Assert.Equal(78683433, result);
    }
}
