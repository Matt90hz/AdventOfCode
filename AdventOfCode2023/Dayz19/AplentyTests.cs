namespace AdventOfCode2023.Dayz19;

public static class AplentyTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz19\\input_test1.txt");
        var result = Aplenty.Accepted(input);
        Assert.Equal(19114, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz19\\input.txt");
        var result = Aplenty.Accepted(input);
        Assert.Equal(373302, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz19\\input_test1.txt");
        var result = Aplenty.AllPossibleAccepted(input);
        Assert.Equal(167409079868000, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz19\\input.txt");
        var result = Aplenty.AllPossibleAccepted(input);
        Assert.Equal(130262715574114, result);
    }

}
