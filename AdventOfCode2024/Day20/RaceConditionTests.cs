namespace AdventOfCode2024.Day20;

public sealed class RaceConditionTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day20\input_test.txt");
        var result = RaceCondition.CountGoodCheats(input);
        Assert.Equal(0, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day20\input.txt");
        var result = RaceCondition.CountGoodCheats(input);
        Assert.Equal(1375, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day20\input_test.txt");
        var result = RaceCondition.CountLegalCheats(input);
        Assert.Equal(16, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day20\input.txt");
        var result = RaceCondition.CountLegalCheats(input);
        Assert.Equal(616957151871345, result);
    }
}
