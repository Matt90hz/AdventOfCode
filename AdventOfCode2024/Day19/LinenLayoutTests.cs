namespace AdventOfCode2024.Day19;

public sealed class LinenLayoutTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day19\input_test.txt");
        var result = LinenLayout.CountPossibleLayouts(input);
        Assert.Equal(6, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day19\input.txt");
        var result = LinenLayout.CountPossibleLayouts(input);
        Assert.Equal(251, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day19\input_test.txt");
        var result = LinenLayout.CountAllPossibleLayouts(input);
        Assert.Equal(16, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day19\input.txt");
        var result = LinenLayout.CountAllPossibleLayouts(input);
        Assert.Equal(616957151871345, result);
    }
}