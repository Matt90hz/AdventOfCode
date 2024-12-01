using Xunit;

namespace AdventOfCode2024.Day01;

public class HistorianHysteriaTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(".\\Day01\\input_test.txt");
        var result = HistorianHysteria.TotalDistanceBetweenLists(input);

        Assert.Equal(11, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(".\\Day01\\input.txt");
        var result = HistorianHysteria.TotalDistanceBetweenLists(input);

        Assert.Equal(2742123, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(".\\Day01\\input_test.txt");
        var result = HistorianHysteria.TotalDistanceBetweenLists(input);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(".\\Day01\\input.txt");
        var result = HistorianHysteria.TotalDistanceBetweenLists(input);

        Assert.Equal(0, result);
    }
}