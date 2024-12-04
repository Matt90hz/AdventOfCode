using Xunit;

namespace AdventOfCode2024.Day04;

public sealed class CeresSearchTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day04\input_test.txt");
        var result = CeresSearch.CountXMAS(input);

        Assert.Equal(18, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day04\input.txt");
        var result = CeresSearch.CountXMAS(input);

        Assert.Equal(2401, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day04\input_test.txt");
        var result = CeresSearch.CountX_MAS(input);

        Assert.Equal(9, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day04\input.txt");
        var result = CeresSearch.CountX_MAS(input);

        Assert.Equal(1822, result);
    }
}
