
namespace AdventOfCode2024.Day24;
public static class CrossedWire
{
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
        private readonly Wire _in0;
        private readonly Wire _in1;
        private readonly Wire _output;

        protected Gate(Wire in0, Wire in1, Wire output)
        {
            in0.StateChanged += OnInputChanged;
            in1.StateChanged += OnInputChanged;

            _in0 = in0;
            _in1 = in1;
            _output = output;
        }

        private void OnInputChanged(Wire wire, int state)
        {
            _output.State = OutputState(_in0, _in1);
        }

        protected abstract int OutputState(Wire in0, Wire in1);

        public override string ToString() => GetType().Name;
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
}