using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz12;
public static class HotSpringsTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input_test1.txt");
        var x = HotSprings.GetConditionRecord(input[0]);
        var result = HotSprings.ConditionRecordCombinations(x.Springs, x.DamegedMap);

        Assert.Equal(1, result);
    }

    [Fact]
    public static void Part1Test2()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input_test1.txt");
        var x = HotSprings.GetConditionRecord(input[1]);
        var result = HotSprings.ConditionRecordCombinations(x.Springs, x.DamegedMap);

        Assert.Equal(4, result);
    }

    [Fact]
    public static void Part1Test3()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input_test1.txt");
        var x = HotSprings.GetConditionRecord(input[2]);
        var result = HotSprings.ConditionRecordCombinations(x.Springs, x.DamegedMap);

        Assert.Equal(1, result);
    }

    [Fact]
    public static void Part1Test4()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input_test1.txt");
        var x = HotSprings.GetConditionRecord(input[3]);
        var result = HotSprings.ConditionRecordCombinations(x.Springs, x.DamegedMap);

        Assert.Equal(1, result);
    }

    [Fact]
    public static void Part1Test5()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input_test1.txt");
        var x = HotSprings.GetConditionRecord(input[4]);
        var result = HotSprings.ConditionRecordCombinations(x.Springs, x.DamegedMap);

        Assert.Equal(4, result);
    }

    [Fact]
    public static void Part1Test6()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input_test1.txt");
        var x = HotSprings.GetConditionRecord(input[5]);
        var result = HotSprings.ConditionRecordCombinations(x.Springs, x.DamegedMap);

        Assert.Equal(10, result);
    }

    [Fact]
    public static void Part1Test7()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input_test1.txt");
        var result = HotSprings.ConditionRecordsCombinations(input);

        Assert.Equal(21, result);
    }

    [Fact]
    public static void Part1Solution()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input.txt");
        var result = HotSprings.ConditionRecordsCombinations(input);

        Assert.Equal(7236, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input_test1.txt");
        var result = HotSprings.ConditionRecordsCombinationsUnfolded(input);

        Assert.Equal(525152, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz12\input.txt");
        var result = HotSprings.ConditionRecordsCombinationsUnfolded(input);

        Assert.Equal(11607695322318, result);
    }
}
