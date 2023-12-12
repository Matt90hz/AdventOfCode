using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz11;
public static class CosmicExpansionTests
{
    [Fact]
    public static void Part1Test1()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz11\input_test1.txt");
        var result = CosmicExpansion.CosmicDistances(input);
        
        Assert.Equal(374, result);
    }

    [Fact]
    public static void Part1Solution() 
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz11\input.txt");
        var result = CosmicExpansion.CosmicDistances(input);

        Assert.Equal(9521776, result);
    }

    [Fact]
    public static void Part2Test1()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz11\input_test1.txt");
        var result = CosmicExpansion.MegaCosmicDistances(input, 10);

        Assert.Equal(1030, result);
    }

    [Fact]
    public static void Part2Test2()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz11\input_test1.txt");
        var result = CosmicExpansion.MegaCosmicDistances(input, 100);

        Assert.Equal(8410, result);
    }

    [Fact]
    public static void Part2Solution()
    {
        var input = File.ReadAllLines(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz11\input.txt");
        var result = CosmicExpansion.MegaCosmicDistances(input, 1_000_000);

        Assert.Equal(553224415344, result);
    }
}
