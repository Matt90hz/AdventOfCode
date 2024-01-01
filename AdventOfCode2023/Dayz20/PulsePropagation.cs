using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Dayz20;

interface IPulse { };
readonly record struct HighPulse : IPulse;
readonly record struct LowPulse : IPulse;
readonly record struct NoPulse : IPulse;

abstract record Module(string Code, string[] Connections);

record FlipFlop(string Code, string[] Connections) : Module(Code, Connections)
{
    public bool IsOn { get; set; } = false;
}

record Conjunction(string Code, string[] Connections) : Module(Code, Connections)
{
    public IDictionary<string, IPulse> Memory { get; } = new Dictionary<string, IPulse>();
}

record Test(string Code) : Module(Code, Array.Empty<string>());

record Broadcaster(string Code, string[] Connections) : Module(Code, Connections);

record Button() : Module("btn", new[] { "broad" })
{
    public static Module Instance { get; } = new Button();
}


internal static class PulsePropagation
{
    public static long LowToRx(string input)
    {
        var modules = GetModules(input);

        var (xf, xn, zl, qn) = FindCyclesNeededToCreateTheConditionForRxLowGivenMyInput(modules);

        return xf * xn * zl * qn;
    }

    static (long XF_High, long XN_High, long ZL_High, long QN_High) FindCyclesNeededToCreateTheConditionForRxLowGivenMyInput(IDictionary<string, Module> modules)
    {
        /*
        * I know that th must send low to rx.
        * To do that xf, xn, zl, qn must send high to th one after the other.
        * So I must find after how many pushes all of these modules send high.
        * After that the number of pushes for rx to receive low will be the LCM.
        * Actually I found that the for my inputs all of these number are prime so I have just to multiply them togheter.
        */

        var toProcess = new[] { (From: Button.Instance, To: modules["broad"], Signal: Pulser.LowPulse) };

        long press = 1;
        long xf = 0, xn = 0, zl = 0, qn = 0;

        while (xf == default || xn == default || zl == default || qn == default)
        {
            if (toProcess.Any() is false)
            {
                toProcess = new[] { (From: Button.Instance, To: modules["broad"], Signal: Pulser.LowPulse) };
                press++;
            }

            if (toProcess.Any(item => item.ModuleSendsHigh("xf"))) xf = press;
            if (toProcess.Any(item => item.ModuleSendsHigh("xn"))) xn = press;
            if (toProcess.Any(item => item.ModuleSendsHigh("zl"))) zl = press;
            if (toProcess.Any(item => item.ModuleSendsHigh("qn"))) qn = press;

            toProcess = ComputeSignals(toProcess, modules);
        }

        return (xf, xn, zl, qn);
    }

    static bool ModuleSendsHigh(this (Module From, Module To, IPulse Signal) tuple, string moduleKey)
    {
        var (from, _, signal) = tuple;

        return from.Code == moduleKey && signal is HighPulse;
    }

    static Module GetModuleByKey(string moduleKey, IDictionary<string, Module> modules)
    {
        var found = modules.TryGetValue(moduleKey, out var module);

        return found ? module! : new Test(moduleKey);
    }

    public static int HighLowPulses(string input)
    {
        var modules = GetModules(input);

        var start = (From: Button.Instance, To: modules["broad"], Signal: Pulser.LowPulse);

        var (high, low) = Enumerable
            .Repeat(start, 1000)
            .Select(start => CountHighLowSent(start, modules))
            .Aggregate(ProductOfHigLow);

        return high * low;
    }

    static (int High, int Low) ProductOfHigLow((int High, int Low) agg, (int High, int Low) element) => (agg.High += element.High, agg.Low += element.Low);

    static (int High, int Low) CountHighLowSent((Module From, Module To, IPulse Signal) start, IDictionary<string, Module> modules)
    {
        var toProcess = new[] { start };
        int high = 0;
        int low = 0;

        while (toProcess.Any())
        {
            high += toProcess.Count(item => item.Signal is HighPulse);
            low += toProcess.Count(item => item.Signal is LowPulse);

            toProcess = ComputeSignals(toProcess, modules);
        }

        //var (high, low, _) = CountHighLowSent(new[] { start }, (0, 1), modules);

        return (high, low);
    }

    static (int High, int Low, (Module From, Module To, IPulse Signal)[] ToProcess) CountHighLowSent(
        (Module From, Module To, IPulse Signal)[] toProcess,
        (int High, int Low) count,
        IDictionary<string, Module> modules)
    {
        if (toProcess.Any() is false) return (count.High, count.Low, toProcess);

        var nextToProcess = ComputeSignals(toProcess, modules);

        var high = count.High += nextToProcess.Count(item => item.Signal is HighPulse);
        var low = count.Low += nextToProcess.Count(item => item.Signal is LowPulse);

        return CountHighLowSent(nextToProcess, (high, low), modules);
    }

    static (Module From, Module To, IPulse Signal) Print((Module From, Module To, IPulse Signal) toProcess)
    {
        var (from, to, signal) = toProcess;

        Console.WriteLine($"{from.Code}\t{signal.GetType().Name} ->\t{to.Code}");

        return toProcess;
    }

    static (Module From, Module To, IPulse Signal)[] ComputeSignals(
        (Module From, Module To, IPulse Signal)[] toProcess,
        IDictionary<string, Module> modules)
    {
        var nextToProcess = toProcess
            .SelectMany(item => ComputeSignal(item, modules))
            .Where(item => item.Signal is not NoPulse)
            .ToArray();

        return nextToProcess;
    }

    static IEnumerable<(Module From, Module To, IPulse Signal)> ComputeSignal(
        (Module From, Module To, IPulse Signal) tuple,
        IDictionary<string, Module> modules)
    {
        var (from, to, signal) = tuple;

        var nextSignal = to.ProcessOutput(from, signal);

        var nextSignals = to.Connections
            .Select(modulekey => GetModuleByKey(modulekey, modules))
            .Select(module => (to, module, nextSignal))
            .ToArray();

        return nextSignals;
    }

    static IPulse ProcessOutput(this Module module, Module from, IPulse signal)
    {
        var output = module switch
        {
            Broadcaster x => x.ProcessOutput(signal),
            FlipFlop x => x.ProcessOutput(signal),
            Conjunction x => x.ProcessOutput(from, signal),
            _ => new NoPulse(),
        };

        return output;
    }

    static IPulse ProcessOutput(this Conjunction conjunction, Module from, IPulse signal)
    {
        if (signal is NoPulse) return new NoPulse();

        conjunction.Memory[from.Code] = signal;

        return conjunction.Memory.Values.All(x => x is HighPulse) ? new LowPulse() : new HighPulse();
    }

    static IPulse ProcessOutput(this FlipFlop flipFlop, IPulse signal)
    {
        if (signal is HighPulse or NoPulse) return new NoPulse();

        flipFlop.IsOn = !flipFlop.IsOn;

        return flipFlop.IsOn ? new HighPulse() : new LowPulse();

    }

    static IPulse ProcessOutput(this Broadcaster _, IPulse signal)
    {
        return signal;
    }

    static IDictionary<string, Module> GetModules(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var modules = lines
            .Select(GetModule)
            .ToDictionary(module => module.Code);

        FillConjunctionConnection(modules);

        return modules;
    }

    static void FillConjunctionConnection(IDictionary<string, Module> modules)
    {
        var conjunctions = modules.Values.Where(x => x is Conjunction).Cast<Conjunction>();

        foreach (var conjunction in conjunctions)
        {
            var connections = modules.Values.Where(x => x.Connections.Contains(conjunction.Code));

            foreach (var connection in connections)
            {
                conjunction.Memory.Add(connection.Code, new LowPulse());
            }
        }
    }

    static Module GetModule(string line) => line[0] switch
    {
        '&' => GetConjuction(line),
        '%' => GetFlipFlop(line),
        'b' => GetBroadcaster(line),
        _ => throw new ArgumentException($"Invalid module key [{line[0]}].")
    };

    static Broadcaster GetBroadcaster(string line)
    {
        var connections = line[(line.IndexOf('>') + 1)..]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        return new Broadcaster("broad", connections);
    }

    static FlipFlop GetFlipFlop(string line)
    {
        var connections = line[(line.IndexOf('>') + 1)..]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        var code = line[1..line.IndexOf(' ')];

        return new FlipFlop(code, connections);
    }

    static Conjunction GetConjuction(string line)
    {
        var connections = line[(line.IndexOf('>') + 1)..]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        var code = line[1..line.IndexOf(' ')];

        return new Conjunction(code, connections);
    }
}

internal static class Pulser
{
    public static IPulse LowPulse => new LowPulse();

    public static IPulse HighPulse => new HighPulse();

    public static IPulse NoPulse => new NoPulse();
}