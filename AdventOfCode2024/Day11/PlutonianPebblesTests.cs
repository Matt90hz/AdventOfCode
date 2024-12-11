namespace AdventOfCode2024.Day11;

public sealed class PlutonianPebblesTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText(@".\Day11\input_test.txt");
        var result = PlutonianPebbles.StonesAfterBlinks("125 17", 6);
        Assert.Equal(22, result);
    }

    [Fact]
    public void Part1_Test2()
    {
        var input = File.ReadAllText(@".\Day11\input_test.txt");
        var result = PlutonianPebbles.StonesAfterBlinks(input, 25);
        Assert.Equal(55312, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day11\input.txt");
        var result = PlutonianPebbles.StonesAfterBlinks(input, 25);
        Assert.Equal(207683, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day11\input.txt");
        var result = PlutonianPebbles.StonesAfterBlinks(input, 75);
        Assert.Equal(244782991106220, result);
    }
}