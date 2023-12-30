namespace AdventOfCode2023.Dayz20;

public static class PulsePropagationTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz20\\input_test1.txt");
        var result = PulsePropagation.HighLowPulses(input);
        Assert.Equal(32000000, result);
    }

    [Fact]
    public static void Part1Test2()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz20\\input_test2.txt");
        var result = PulsePropagation.HighLowPulses(input);
        Assert.Equal(11687500, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Dayz20\\input.txt");
        var result = PulsePropagation.HighLowPulses(input);
        Assert.Equal(856482136, result);
    }
}
