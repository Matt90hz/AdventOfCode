using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day9;
public static class MirageMaintenanceTests
{
    [Fact]
    public static void TestPart1()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day9\input_test.txt");
        var analisys = MirageMaintenance.OasisAnalisys(input);

        var values = input.Select(MirageMaintenance.ParseValues).ToArray();
        var prediction1 = MirageMaintenance.PredictAnalisys(values[0]);
        var prediction2 = MirageMaintenance.PredictAnalisys(values[1]);
        var prediction3 = MirageMaintenance.PredictAnalisys(values[2]);

        Assert.Equal(18, prediction1);
        Assert.Equal(28, prediction2);
        Assert.Equal(68, prediction3);
        Assert.Equal(114, analisys);
    }

    [Fact]
    public static void SolutionPart1()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day9\input.txt");
        var analisys = MirageMaintenance.OasisAnalisys(input);

        Assert.Equal(1681758908, analisys);
    }

    [Fact]
    public static void TestPart2()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day9\input_test.txt");
        var input2 = new[] { "10  13  16  21  30  45" }; 
        var analisys = MirageMaintenance.OasisBackwardAnalisys(input);

        var values = input2.Select(MirageMaintenance.ParseValues).ToArray();
        var prediction1 = MirageMaintenance.PredictPastAnalisys(values[0]);

        Assert.Equal(5, prediction1);
        Assert.Equal(2, analisys);
    }

    [Fact]
    public static void SolutionPart2()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day9\input.txt");
        var analisys = MirageMaintenance.OasisBackwardAnalisys(input);

        Assert.Equal(803, analisys);
    }

}
