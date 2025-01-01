namespace AdventOfCode2024.Day24;

public sealed class CrossedWireTests
{
    [Fact]
    public void Part1_Test1()
    {
        var input = File.ReadAllText(@".\Day24\input_test1.txt");
        var result = CrossedWire.OutputOfZWires(input);
        Assert.Equal(4, result);
    }

    [Fact]
    public void Part1_Test2()
    {
        var input = File.ReadAllText(@".\Day24\input_test2.txt");
        var result = CrossedWire.OutputOfZWires(input);
        Assert.Equal(2024, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day24\input.txt");
        var result = CrossedWire.OutputOfZWires(input);
        Assert.Equal(48063513640678, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day24\input.txt");
        var result = CrossedWire.CorruptedGates(input);
        Assert.Equal("hqh,mmk,pvb,qdq,vkq,z11,z24,z38", result);
    }
}