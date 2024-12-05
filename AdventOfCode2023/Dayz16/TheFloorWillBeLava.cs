using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2023.Dayz16;

abstract record ContraptionObject 
{
    public bool IsEnergized { get; set; } = false;
};

record EmptySpace : ContraptionObject;
record Bound : ContraptionObject;

abstract record Mirror : ContraptionObject;
record FortyFive : Mirror;
record MinusFortyFive : Mirror;

abstract record Splitter : ContraptionObject;
record Vertical : Splitter;
record Horizontal : Splitter;

record Beam(IPosition<ContraptionObject> Position, Direction Direction);

internal static class TheFloorWillBeLava
{
    public static int MaxEnergized(string input)
    {      
        var layout = GetLayout(input);

        var topRow = Enumerable
            .Range(1, layout.GetLength(0) - 2)
            .Select(i => (Start: (1, i), Direction: Direction.Down));

        var bottomRow = Enumerable
            .Range(1, layout.GetLength(0) - 2)
            .Select(i => (Start: (layout.GetLength(1) - 2, i), Direction: Direction.Up));

        var leftColumn = Enumerable
            .Range(1, layout.GetLength(1) - 2)
            .Select(i => (Start: (i, 1), Direction: Direction.Right));

        var rightColumn = Enumerable
            .Range(1, layout.GetLength(1) - 2)
            .Select(i => (Start: (i, layout.GetLength(0) - 2), Direction: Direction.Left));

        var energized = topRow
            .Concat(bottomRow)
            .Concat(leftColumn)
            .Concat(rightColumn)
            .Select(x => Energize(layout, x.Start, x.Direction));

        var max = energized.Max(x => x.AsEnumerable().Count(x => x.IsEnergized));

        return max;
    }

    public static int Energized(string input)
    {
        var layout = GetLayout(input);

        var energized = Energize(layout, (1, 1), Direction.Right);

        var count = energized.AsEnumerable().Count(x => x.IsEnergized);

        return count;
    }

    static ContraptionObject[,] Energize(ContraptionObject[,] layout, (int Row, int Col) start, Direction direction)
    {
        var energized = layout.Select(x => x with { }); //clone
        var beams = new Stack<Beam>();

        beams.Push(new Beam(energized.GetPosition(start.Row, start.Col), direction));

        while (beams.Count > 0)
        {
            var beam = beams.Pop();

            var co = beam.Position.Value;
            var d = beam.Direction;
            var p = beam.Position;

            switch ((co, d))
            {
                case (Bound, _):
                    continue;

                case (EmptySpace, _) or (Vertical, Direction.Up or Direction.Down) or (Horizontal, Direction.Left or Direction.Right):
                    beams.Push(beam with { Position = p.Move(d) });
                    break;

                case (FortyFive, Direction.Up) or (MinusFortyFive, Direction.Down):
                    beams.Push(beam with { Position = p.MoveRight(), Direction = Direction.Right });
                    break;
                case (FortyFive, Direction.Down) or (MinusFortyFive, Direction.Up):
                    beams.Push(beam with { Position = p.MoveLeft(), Direction = Direction.Left });
                    break;
                case (FortyFive, Direction.Left) or (MinusFortyFive, Direction.Right):
                    beams.Push(beam with { Position = p.MoveDown(), Direction = Direction.Down });
                    break;
                case (FortyFive, Direction.Right) or (MinusFortyFive, Direction.Left):
                    beams.Push(beam with { Position = p.MoveUp(), Direction = Direction.Up });
                    break;

                case (Vertical, Direction.Left or Direction.Right):
                    if (co.IsEnergized) continue;
                    beams.Push(beam with { Position = p.MoveUp(), Direction = Direction.Up });
                    beams.Push(beam with { Position = p.MoveDown(), Direction = Direction.Down });
                    break;

                case (Horizontal, Direction.Up or Direction.Down):
                    if (co.IsEnergized) continue;
                    beams.Push(beam with { Position = p.MoveLeft(), Direction = Direction.Left });
                    beams.Push(beam with { Position = p.MoveRight(), Direction = Direction.Right });
                    break;
            }

            co.IsEnergized = true;
        }

        return energized;
    }

    static ContraptionObject[,] GetLayout(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var contraptionObjects = lines.Select(x => x.Select<char, ContraptionObject>(x => x switch
        {
            '@' => new Bound(),
            '|' => new Vertical(),
            '-' => new Horizontal(),
            '/' => new FortyFive(),
            '\\' => new MinusFortyFive(),
            _ => new EmptySpace()
        }));
        
        var layout  = contraptionObjects
            .ToMultidimensionalArray()
            .SurroundWith(new Bound());

        return layout;
    }
}