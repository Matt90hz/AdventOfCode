using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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


internal static class PulsePropagation
{
    public static int HighLowPulses(string input)
    {
        var modules = GetModules(input);
        FillConjunctionConnection(modules);

        int high = 0;
        int low = 0;  

        for (int i = 0; i < 1000; i++)
        {
            var toProcess = new List<(IPulse Pulse, Module To, Module From)> { (new LowPulse(), modules["broad"], modules["broad"]) };
            low++;

            while (toProcess.Any())
            {
                var nextToProcess = new List<(IPulse Pulse, Module To, Module From)>();

                foreach (var item in toProcess)
                {
                    var (signal, to, from) = item;

                    var nextSignal = to.ProcessOutput(from, signal);

                    if (nextSignal is NoPulse) continue;

                    foreach (var conn in to.Connections)
                    {
                        if (modules.TryGetValue(conn, out var next))
                        {
                            nextToProcess.Add((nextSignal, next, to));

                        }

                        if (nextSignal is HighPulse) high++;
                        if (nextSignal is LowPulse) low++;

                        Console.WriteLine($"{to.Code}\t{nextSignal.GetType().Name}\t{next?.Code ?? "TEST"}");
                    } 
                }

                toProcess = nextToProcess;
            }
        }     

        return high * low;
    }

    static void FillConjunctionConnection(IDictionary<string, Module> modules)
    {
        var conjunctions = modules.Values.Where(x => x is Conjunction).Cast<Conjunction>();

        foreach(var conjunction in conjunctions)
        {
            var connections = modules.Values.Where(x => x.Connections.Contains(conjunction.Code));

            foreach(var connection in connections)
            {
                conjunction.Memory.Add(connection.Code, new LowPulse());
            }
        }
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
        conjunction.Memory[from.Code] = signal;

        return conjunction.Memory.Values.All(x => x is HighPulse) ? new LowPulse() : new HighPulse();
    }

    static IPulse ProcessOutput(this FlipFlop flipFlop, IPulse signal)
    {
        if(signal is HighPulse) return new NoPulse();

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

        return modules;
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