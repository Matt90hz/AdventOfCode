namespace AdventOfCode2024.Day10;
public sealed class HoofItTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day10\input_test.txt");
        var result = HoofIt.TrailheadsScore(input);
        Assert.Equal(36, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day10\input.txt");
        var result = HoofIt.TrailheadsScore(input);
        Assert.Equal(798, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day10\input_test.txt");
        var result = HoofIt.TrailheadsRating(input);
        Assert.Equal(81, result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day10\input.txt");
        var result = HoofIt.TrailheadsRating(input);
        Assert.Equal(1816, result);
    }
}