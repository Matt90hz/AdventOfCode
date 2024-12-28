namespace AdventOfCode2024.Day23;

public sealed class LANPartyTests
{
    [Fact]
    public void Part1_Test()
    {
        var input = File.ReadAllText(@".\Day23\input_test.txt");
        var result = LANParty.PossibleHistoriansComputers(input);
        Assert.Equal(7, result);
    }

    [Fact]
    public void Part1_Solution()
    {
        var input = File.ReadAllText(@".\Day23\input.txt");
        var result = LANParty.PossibleHistoriansComputers(input);
        Assert.Equal(1378, result);
    }

    [Fact]
    public void Part2_Test()
    {
        var input = File.ReadAllText(@".\Day23\input_test.txt");
        var result = LANParty.LanPartyPassword(input);
        Assert.Equal("co,de,ka,ta", result);
    }

    [Fact]
    public void Part2_Solution()
    {
        var input = File.ReadAllText(@".\Day23\input.txt");
        var result = LANParty.LanPartyPassword(input);
        Assert.Equal("bs,ey,fq,fy,he,ii,lh,ol,tc,uu,wl,xq,xv", result);
    }
}
