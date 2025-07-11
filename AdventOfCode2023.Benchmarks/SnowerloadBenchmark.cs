using BenchmarkDotNet.Attributes;
using AdventOfCode2023.Dayz25;

namespace AdventOfCode2023.Benchmarks;

[MemoryDiagnoser(false)]
public class SnowerloadBenchmark
{
    private string _input = string.Empty;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz25\input.txt");
    }
    
    [Benchmark]
    public int Optimized() => SnowerloadOptimized.GroupSize(_input);

}
