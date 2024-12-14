namespace AdventOfCode2024.Day14;

public sealed class RestroomRedoubtTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day14\input_test.txt");
        var result = RestroomRedoubt.SafetyFactor(input, wide: 11, tall: 7);
        Assert.Equal(12, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day14\input.txt");
        var result = RestroomRedoubt.SafetyFactor(input);
        Assert.Equal(230686500, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day14\input.txt");
        var result = RestroomRedoubt.EasterEggTime(input);
        Assert.Equal(7672, result);
    }
}
