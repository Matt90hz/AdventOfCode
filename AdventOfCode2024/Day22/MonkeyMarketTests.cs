namespace AdventOfCode2024.Day22;

public sealed class MonkeyMarketTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day22\input_test1.txt");
        var result = MonkeyMarket.SumSecretNumbersGenerated(input);
        Assert.Equal(37327623, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day22\input.txt");
        var result = MonkeyMarket.SumSecretNumbersGenerated(input);
        Assert.Equal(14082561342, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day22\input_test2.txt");
        var result = MonkeyMarket.MaxBananasObtainable(input);
        Assert.Equal(23, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day22\input.txt");
        var result = MonkeyMarket.MaxBananasObtainable(input);
        Assert.Equal(1568, result);
    }
}
