namespace AdventOfCode2023.Day8;

public static class HauntedWastelandTests
{
    private static readonly string _input = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Day8\\input.txt");
    private static readonly string _input_test1 = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Day8\\input_test1.txt");
    private static readonly string _input_test2 = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Day8\\input_test2.txt");
    private static readonly string _input_test3 = File.ReadAllText("D:\\VisualStudio\\AdventOfCode\\AdventOfCode2023\\Day8\\input_test3.txt");

    [Fact]
    public static void Test1()
    {
        var x = HauntedWasteland.HowFarTheExit(_input_test1);

        Assert.Equal(2, x);
    }


    [Fact]
    public static void Test2()
    {
        var x = HauntedWasteland.HowFarTheExit(_input_test2);

        Assert.Equal(6, x);
    }

    [Fact]
    public static void Part1Solution()
    {
        var x = HauntedWasteland.HowFarTheExit(_input);

        Assert.Equal(17263, x);
    }


    [Fact]
    public static void Test3()
    {
        var x = HauntedWasteland.HowFarTheExitForGhosts(_input_test3);

        Assert.Equal(6, x);
    }

    [Fact]
    public static void Part2Solution()
    {      
        var x = HauntedWasteland.HowFarTheExitForGhosts(_input);
        
        Assert.Equal(14631604759649, x);
    }
}
