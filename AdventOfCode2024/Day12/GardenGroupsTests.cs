namespace AdventOfCode2024.Day12;

public sealed class GardenGroupsTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText(@".\Day12\input_test1.txt");
        var result = GardenGroups.FencingCost(input);
        Assert.Equal(140, result);
    }

    [Fact]
    public void Part1_Test2()
    {
        var input = File.ReadAllText(@".\Day12\input_test2.txt");
        var result = GardenGroups.FencingCost(input);
        Assert.Equal(772, result);
    }

    [Fact]
    public void Part1_Test3()
    {
        var input = File.ReadAllText(@".\Day12\input_test3.txt");
        var result = GardenGroups.FencingCost(input);
        Assert.Equal(1930, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day12\input.txt");
        var result = GardenGroups.FencingCost(input);
        Assert.Equal(1450816, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day12\input.txt");
        var result = GardenGroups.FencingCost(input);
        Assert.Equal(0, result);
    }
}