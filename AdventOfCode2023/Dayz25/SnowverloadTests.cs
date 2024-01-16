namespace AdventOfCode2023.Dayz25;

public static class SnowverloadTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz25\\input_test1.txt");
        var result = Snowverload.GroupSize(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public static void Part1Solution() 
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz25\\input.txt");
        var result = Snowverload.GroupSize(input);

        Assert.Equal(614655, result);
    }

    [Fact]
    public static void Part1Test1NoCheat()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz25\\input_test1.txt");
        var result = Snowverload.GroupSizeNoCheat(input);

        Assert.Equal(54, result);
    }

    [Fact]
    public static void Part1SolutionNoCheat()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz25\\input.txt");
        var result = Snowverload.GroupSizeNoCheat(input);

        Assert.Equal(614655, result);
    }
}
