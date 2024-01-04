namespace AdventOfCode2023.Dayz22;

public static class SandSlabsTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz22\\input_test1.txt");
        var result = SandSlabs.BricksSafelyDisintegrable(input);

        Assert.Equal(5, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz22\\input.txt");
        var result = SandSlabs.BricksSafelyDisintegrable(input);

        Assert.Equal(393, result);
    }
}
