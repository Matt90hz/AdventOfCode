namespace AdventOfCode2024.Day15;

public sealed class WarehouseWoesTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText(@".\Day15\input_test1.txt");
        var result = WarehouseWoes.SumGPSCoordinates(input);
        Assert.Equal(2028, result);
    }

    [Fact]
    public void Part1_Test2()
    {
        var input = File.ReadAllText(@".\Day15\input_test2.txt");
        var result = WarehouseWoes.SumGPSCoordinates(input);
        Assert.Equal(10092, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day15\input.txt");
        var result = WarehouseWoes.SumGPSCoordinates(input);
        Assert.Equal(1442192, result);
    }

    [Fact]
    public void Part2_Test1()
    {
        var input = File.ReadAllText(@".\Day15\input_test3.txt");
        var result = WarehouseWoes.SumGPSCoordinatesBigWarehouse(input);
        Assert.Equal(618, result);
    }

    [Fact]
    public void Part2_Test2()
    {
        var input = File.ReadAllText(@".\Day15\input_test2.txt");
        var result = WarehouseWoes.SumGPSCoordinatesBigWarehouse(input);
        Assert.Equal(9021, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day15\input.txt");
        var result = WarehouseWoes.SumGPSCoordinatesBigWarehouse(input);
        Assert.Equal(1448458, result);
    }
}
