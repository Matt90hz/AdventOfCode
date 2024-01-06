namespace AdventOfCode2023.Dayz23;

public static class LongWalkTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz23\\input_test1.txt");
        var result = LongWalk.LongestHike(input);

        Assert.Equal(94, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz23\\input.txt");
        var result = LongWalk.LongestHike(input);

        Assert.Equal(0, result);
    }
}
