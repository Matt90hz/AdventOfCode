using System;
using System.Collections.Generic;
using System.Data;
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
    public static int LowToRx(string input)
    {
        var modules = GetModules(input);

        //var toProcess = new[] { (From: Button.Instance, To: modules["broad"], Signal: Pulser.LowPulse) };
        int press = 1;
        int high = 0;
        int low = 0;

        for (int i = 0; i < 1000; i++)
        {
            var toProcess = new[] { (From: Button.Instance, To: modules["broad"], Signal: Pulser.LowPulse) };

            while (toProcess.Any())
            {
                low += toProcess.Count(item => item.Signal is LowPulse);
                high += toProcess.Count(item => item.Signal is HighPulse);

                toProcess = ComputeSignals(toProcess, modules);

                if (toProcess.Any() is false)
                {
                    //Console.WriteLine("-----------------------");
                    press++;
                }
            } 
        }

        return high * low;
        throw new Exception("rx not found!");
    }

    public static int DbAllTrue(string input)
    {
        var modules = GetModules(input);

        var toProcess = new[] { (From: modules["pl"], To: modules["db"], Signal: Pulser.LowPulse) };
        int count = 0;

        while (true)
        {
            count++;

            toProcess = ComputeSignals(toProcess, modules);

            if (toProcess.Any(item => item.To.Code == "xf" && item.Signal == Pulser.LowPulse))
            {
                return count;
            }

            if (toProcess.Any() is false)
            {
                toProcess = new[] { (From: modules["pl"], To: modules["db"], Signal: Pulser.LowPulse) };
            }
        }

        throw new Exception("rx not found!");

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

        //Print(tuple);

        var nextSignals = to.Connections
            .Select(modulekey => GetModuleByKey(modulekey, modules))
            .Select(module => (to, module, nextSignal))
            .ToArray();

        return nextSignals;
    }

    static Module GetModuleByKey(string moduleKey, IDictionary<string, Module> modules)
    {
        var found = modules.TryGetValue(moduleKey, out var module);

        return found ? module! : new Test(moduleKey);
    }

    static bool SignalToRx((Module From, Module To, IPulse Signal) tuple)
    {
        var (_, to, _) = tuple;

        return to.Code == "rx";
    }

    static bool SignalLowToRx((Module From, Module To, IPulse Signal) tuple)
    {
        var (_, to, signal) = tuple;

        return to.Code == "rx" && signal is LowPulse;
    }

    public static int HighLowPulses(string input)
    {
        var modules = GetModules(input);

        int high = 0;
        int low = 0;

        for (int i = 0; i < 1000; i++)
        {
            var toProcess = new[] { (From: Button.Instance, To: modules["broad"], Signal: Pulser.LowPulse) }; 

            while (toProcess.Any())
            {
                low += toProcess.Count(item => item.Signal is LowPulse);
                high += toProcess.Count(item => item.Signal is HighPulse);

                toProcess = ComputeSignals(toProcess, modules);
            }
        }

        return high * low;
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