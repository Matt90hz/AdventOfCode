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

enum Direction { Left, Right, Up, Down, None }

abstract record ContraptionObject 
{
    public bool IsEnergized { get; set; } = false;

    public List<Beam> Beams { get; } = new List<Beam>();
};

record EmptySpace : ContraptionObject;
record Bound : ContraptionObject;
record Splitter : ContraptionObject;
record Mirror : ContraptionObject;
record FourtyFive : Mirror;
record MinusFortyFive : Mirror;
record Vertical : Splitter;
record Horizontal : Splitter;

record Beam(Position<ContraptionObject> Position, Direction Direction);

internal static class TheFloorWillBeLava
{

    public static int MaxEnergized(string input)
    {
        var layout = GetLayout(input);

        var topRow = Enumerable.Range(1, layout.GetLength(0) - 2)
            .Select(i => new Beam(GetLayout(input).GetPosition(1, i), Direction.Down));

        var bottomRow = Enumerable.Range(1, layout.GetLength(0) - 2)
            .Select(i => new Beam(GetLayout(input).GetPosition(layout.GetLength(1) - 2, i), Direction.Up));

        var leftColumn = Enumerable.Range(1, layout.GetLength(1) - 2)
            .Select(i => new Beam(GetLayout(input).GetPosition(i, 1), Direction.Right));

        var rightColumn = Enumerable.Range(1, layout.GetLength(1) - 2)
            .Select(i => new Beam(GetLayout(input).GetPosition(i, layout.GetLength(0) - 2), Direction.Left));

        var beams = new List<Beam>();

        beams.AddRange(topRow);
        beams.AddRange(bottomRow);
        beams.AddRange(leftColumn);
        beams.AddRange(rightColumn);

        var energizedList = beams.Select(Energized);

        var max = energizedList.Max();

        return max;
    }

    public static int Energized(string input)
    {
        var layout = GetLayout(input);

        var startBeam = new Beam(layout.GetPosition(1, 1), Direction.Right);

        var energized = Energized(startBeam);

        return energized;
    }

    static int Energized(Beam startBeam)
    {
        var layout = startBeam.Position.Array;

        var beams = new List<Beam>() { startBeam };

        layout[startBeam.Position.Row, startBeam.Position.Column].Beams.AddRange(beams);
        layout[startBeam.Position.Row, startBeam.Position.Column].IsEnergized = true;

        while (beams.Any())
        {
            beams = beams.SelectMany(Move).ToList();

            //Task.Delay(5).Wait();
            //Console.CursorVisible = false;
            //Console.Clear();
            //Console.Write(layout.ToFriendlyString(ContraptionObjectToString));
        }

        var energized = layout
            .GetRows()
            .Select(row => row.Count(x => x.IsEnergized))
            .Sum();

        return energized;
    }

    static IEnumerable<Beam> Move(Beam beam)
    {
        var nextPosition = beam.Position.Value switch
        {
            FourtyFive => beam.Direction switch
            {
                Direction.Left => new[] { beam.MoveBeamDown() },
                Direction.Right => new[] { beam.MoveBeamUp() },
                Direction.Up => new[] { beam.MoveBeamRight() },
                Direction.Down => new[] { beam.MoveBeamLeft() },
                _ => throw new NotImplementedException(),
            },
            MinusFortyFive => beam.Direction switch
            {
                Direction.Left => new[] { beam.MoveBeamUp() },
                Direction.Right => new[] { beam.MoveBeamDown() },
                Direction.Up => new[] { beam.MoveBeamLeft() },
                Direction.Down => new[] { beam.MoveBeamRight() },
                _ => throw new NotImplementedException(),
            },
            Vertical => beam.Direction switch
            {
                Direction.Left or Direction.Right => new[] { beam.MoveBeamDown(), beam.MoveBeamUp() },
                Direction.Up or Direction.Down => beam.Direction switch
                {
                    Direction.Up => new[] { beam.MoveBeamUp() },
                    Direction.Down => new[] { beam.MoveBeamDown() },
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            },
            Horizontal => beam.Direction switch
            {
                Direction.Up or Direction.Down => new[] { beam.MoveBeamLeft(), beam.MoveBeamRight() },
                Direction.Left or Direction.Right => beam.Direction switch
                {
                    Direction.Left => new[] { beam.MoveBeamLeft() },
                    Direction.Right => new[] { beam.MoveBeamRight() },
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            },
            _ => beam.Direction switch
            {
                Direction.Left => new[] { beam.MoveBeamLeft() },
                Direction.Right => new[] { beam.MoveBeamRight() },
                Direction.Up => new[] { beam.MoveBeamUp() },
                Direction.Down => new[] { beam.MoveBeamDown() },
                _ => throw new NotImplementedException(),
            },
        };

        nextPosition = nextPosition.Where(x => x is not null).ToArray();

        return nextPosition!;
    }

    static ContraptionObject GetContraptionObject(char x) => x switch
    {
        '@' => new Bound(),
        '|' => new Vertical(),
        '-' => new Horizontal(),
        '/' => new FourtyFive(),
        '\\' => new MinusFortyFive(),
        _ => new EmptySpace()
    };

    static ContraptionObject[,] GetLayout(string x)
    {
        var layout = x
            .Split(Environment.NewLine)
            .Select(x => x.Select(x => GetContraptionObject(x)))
            .ToMultidimensionalArray()
            .SurroundWith(new Bound());

        return layout;
    }

    static bool IsMovementAllowed(Beam curr, Position<ContraptionObject> next)
    {
        if (next.Value is Bound) return false;

        if (next.Value.IsEnergized
            && curr.Position.Value is EmptySpace
            && next.Value.Beams.Any(x => x.Direction == curr.Direction)) return false;

        return true;
    }

    static Beam? MoveBeamUp(this Beam beam)
    {
        var next = beam.Position.MoveUp();

        if (IsMovementAllowed(beam, next) is false) return null;
      
        next.Value.IsEnergized = true;

        var nextBeam = new Beam(next, Direction.Up);

        next.Value.Beams.Add(nextBeam);

        return nextBeam;
    }

    static Beam? MoveBeamDown(this Beam beam)
    {
        var next = beam.Position.MoveDown();

        if (IsMovementAllowed(beam, next) is false) return null;

        next.Value.IsEnergized = true;

        var nextBeam = new Beam(next, Direction.Down);

        next.Value.Beams.Add(nextBeam);

        return nextBeam;
    }

    static Beam? MoveBeamLeft(this Beam beam)
    {
        var next = beam.Position.MoveLeft();

        if (IsMovementAllowed(beam, next) is false) return null;

        next.Value.IsEnergized = true;

        var nextBeam = new Beam(next, Direction.Left);

        next.Value.Beams.Add(nextBeam);

        return nextBeam;
    }

    static Beam? MoveBeamRight(this Beam beam)
    {
        var next = beam.Position.MoveRight();

        if (IsMovementAllowed(beam, next) is false) return null;

        next.Value.IsEnergized = true;

        var nextBeam = new Beam(next, Direction.Right);

        next.Value.Beams.Add(nextBeam);

        return nextBeam;
    }

    static string ContraptionObjectToString(this ContraptionObject co) => (co, co.IsEnergized) switch
    {
        (Bound, _) => "@",
        (FourtyFive, _) => "/",
        (MinusFortyFive, _) => "\\",
        (Vertical, _) => "|",
        (Horizontal, _) => "-",
        (EmptySpace, true) => "X",
        (EmptySpace, false) => ".",    
        _ => string.Empty,
    };
}

//co.Direction switch
//        {
//            Direction.Left => "<",
//            Direction.Right => ">",
//            Direction.Up => "A",
//            Direction.Down => "V",
//            _ => "X",
//        }