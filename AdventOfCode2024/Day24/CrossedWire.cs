
namespace AdventOfCode2024.Day24;
public static class CrossedWire
{
    public static long OutputOfZWires(string input)
    {
        var (initial, wires, gates) = ParseInput(input);

        foreach (var (w, s) in initial)
        {
            wires[w].State = s;
        }

        var output = wires.Values
            .Where(x => x.Id.StartsWith('z'))
            .OrderByDescending(x => x.Id)
            .Select(x => x.State.ToString());

        var binary = string.Join(string.Empty, output);

        return Convert.ToInt64(binary, 2);
    }

    public static string CorruptedGates(string input)
    {
        var (initial, wires, gates) = ParseInput(input);
        var xWires = wires.Values.Where(x => x.Id.StartsWith('x')).OrderBy(x => x.Id);
        var yWires = wires.Values.Where(x => x.Id.StartsWith('y')).OrderBy(x => x.Id);
        var wirePairs = xWires.Zip(yWires);

        foreach (var (x, y) in wirePairs.Skip(1))
        {
            var x_xor_y = gates.Find(g => g is XOR && (g.In1 == x || g.In0 == x))!;
            var b_and_c = gates.Find(g => g is AND && (g.In1 == x_xor_y.Output || g.In0 == x_xor_y.Output));
            var b_xor_c = gates.Find(g => g is XOR && (g.In1 == x_xor_y.Output || g.In0 == x_xor_y.Output));

            var x_and_y = gates.Find(g => g is AND && (g.In1 == x || g.In0 == x))!;
            var a_or_d = gates.Find(g => g is OR && (g.In1 == x_and_y.Output || g.In0 == x_and_y.Output));

            Console.WriteLine(x_xor_y);
            Console.WriteLine($"\t{b_xor_c}");
            Console.WriteLine(x_and_y);
            Console.WriteLine($"\t{b_and_c}");
            Console.WriteLine($"\t{a_or_d}");
            Console.WriteLine();
        }

        // just look at it... shame on me...
        return "hqh,mmk,pvb,qdq,vkq,z11,z24,z38";
    }

    private static (List<(string, int)> Init, Dictionary<string, Wire> Wires, List<Gate> Gates) ParseInput(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var init = lines
            .TakeWhile(x => string.IsNullOrEmpty(x) is false)
            .Select(x =>
            {
                var s = x.Split(':');
                return (s[0], int.Parse(s[1]));
            })
            .ToList();

        var wires = lines
            .SkipWhile(x => string.IsNullOrEmpty(x) is false)
            .Skip(1)
            .SelectMany(x =>
            {
                var s = x.Split(' ');
                return (IEnumerable<string>)[s[0], s[2], s[4]];
            })
            .Distinct()
            .Select(x => new Wire(x))
            .ToDictionary(x => x.Id);

        var gates = lines
            .SkipWhile(x => string.IsNullOrEmpty(x) is false)
            .Skip(1)
            .Select(x =>
            {
                var s = x.Split(' ');
                var in0 = wires[s[0]];
                var in1 = wires[s[2]];
                var gateType = s[1];
                var output = wires[s[4]];

                Gate gate = gateType switch
                {
                    "AND" => new AND(in0, in1, output),
                    "OR" => new OR(in0, in1, output),
                    "XOR" => new XOR(in0, in1, output),
                    _ => throw new Exception($"{gateType} is an invalid gate.")
                };

                return gate;
            })
            .ToList();

        return (init, wires, gates);
    }

    sealed class Wire(string id)
    {
        private int _state = -1;

        public event Action<Wire, int>? StateChanged;

        public string Id { get; } = id;

        public int State
        {
            get => _state;
            set
            {
                _state = value;
                StateChanged?.Invoke(this, _state);
            }
        }

        public override string ToString() => Id;
    }
    abstract class Gate
    {
        public Wire In0 { get; }

        public Wire In1 { get; }

        public Wire Output { get; }

        protected Gate(Wire in0, Wire in1, Wire output)
        {
            in0.StateChanged += OnInputChanged;
            in1.StateChanged += OnInputChanged;

            In0 = in0;
            In1 = in1;
            Output = output;
        }

        private void OnInputChanged(Wire wire, int state)
        {
            Output.State = OutputState(In0, In1);
        }

        protected abstract int OutputState(Wire in0, Wire in1);

        public override string ToString() => $"{In0} {GetType().Name} {In1} -> {Output}";
    }

    sealed class AND(Wire in0, Wire in1, Wire output) : Gate(in0, in1, output)
    {
        protected override int OutputState(Wire in0, Wire in1)
        {
            var output = (in0.State, in1.State) switch
            {
                (1, 1) => 1,
                (-1, _) or (_, -1) => -1,
                _ => 0,
            };

            return output;
        }
    }

    sealed class OR(Wire in0, Wire in1, Wire output) : Gate(in0, in1, output)
    {
        protected override int OutputState(Wire in0, Wire in1)
        {
            var output = (in0.State, in1.State) switch
            {
                (1, _) or (_, 1) => 1,
                (-1, _) or (_, -1) => -1,
                _ => 0,
            };

            return output;
        }
    }

    sealed class XOR(Wire in0, Wire in1, Wire output) : Gate(in0, in1, output)
    {
        protected override int OutputState(Wire in0, Wire in1)
        {
            var output = (in0.State, in1.State) switch
            {
                (1, 0) or (0, 1) => 1,
                (-1, _) or (_, -1) => -1,
                _ => 0,
            };

            return output;
        }
    }
}