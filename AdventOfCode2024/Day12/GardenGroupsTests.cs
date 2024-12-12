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
    public void Part2_Test1()
    {
        var input = File.ReadAllText(@".\Day12\input_test1.txt");
        var result = GardenGroups.FencingDiscountCost(input);
        Assert.Equal(80, result);
    }

    [Fact]
    public void Part2_Test2()
    {
        var input = File.ReadAllText(@".\Day12\input_test2.txt");
        var result = GardenGroups.FencingDiscountCost(input);
        Assert.Equal(436, result);
    }

    [Fact]
    public void Part2_Test4()
    {
        var input = File.ReadAllText(@".\Day12\input_test4.txt");
        var result = GardenGroups.FencingDiscountCost(input);
        Assert.Equal(236, result);
    }

    [Fact]
    public void Part2_Test5()
    {
        var input = File.ReadAllText(@".\Day12\input_test5.txt");
        var result = GardenGroups.FencingDiscountCost(input);
        Assert.Equal(368, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day12\input.txt");
        var result = GardenGroups.FencingDiscountCost(input);
        Assert.Equal(865662, result);
    }
}