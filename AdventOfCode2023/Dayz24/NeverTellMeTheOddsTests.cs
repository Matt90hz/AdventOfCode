namespace AdventOfCode2023.Dayz24;

public static class NeverTellMeTheOddsTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz24\\input_test1.txt");
        var result = NeverTellMeTheOdds.IntersectionsInTestArea(input,(7, 27));

        Assert.Equal(2, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz24\\input.txt");
        var result = NeverTellMeTheOdds.IntersectionsInTestArea(input);

        Assert.Equal(23760, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz24\\input_test1.txt");
        var result = NeverTellMeTheOdds.DwayneTheRockJohnson(input);

        Assert.Equal(47, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz24\\input.txt");
        var result = NeverTellMeTheOdds.DwayneTheRockJohnson(input);

        Assert.Equal(888708704663413, result);
    }
}