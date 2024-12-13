namespace AdventOfCode2024.Day13;

public sealed class ClawContraptionTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day13\input_test.txt");
        var result = ClawContraption.MinimumTokensForAllPrizes(input);
        Assert.Equal(480, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day13\input.txt");
        var result = ClawContraption.MinimumTokensForAllPrizes(input);
        Assert.Equal(37901, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day13\input_test.txt");
        var result = ClawContraption.MinimumTokensForAllDamnPrizes(input);
        Assert.Equal(875318608908, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day13\input.txt");
        var result = ClawContraption.MinimumTokensForAllDamnPrizes(input);
        Assert.Equal(77407675412647, result);
    }
}