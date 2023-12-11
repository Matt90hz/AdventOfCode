using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2023.Dayz10;

internal enum Direction { Left, Right, Up, Down, None }

internal abstract record Pipe(int Row, int Col);

internal record Bound(int Row, int Col) : Pipe(Row, Col);

internal record Vertical(int Row, int Col) : Pipe(Row, Col);

internal record Horizontal(int Row, int Col) : Pipe(Row, Col);

internal record TopToLeft(int Row, int Col) : Pipe(Row, Col);

internal record TopToRight(int Row, int Col) : Pipe(Row, Col);

internal record LeftToBottom(int Row, int Col) : Pipe(Row, Col);

internal record RightToBottom(int Row, int Col) : Pipe(Row, Col);

internal record Start(int Row, int Col) : Pipe(Row, Col);

internal record Soil(int Row, int Col) : Pipe(Row, Col);

internal record BeenHere(Pipe Pipe) : Pipe(Pipe.Row, Pipe.Col);

internal record Den(Pipe Pipe) : Pipe(Pipe.Row, Pipe.Col);

internal record Out(Pipe Pipe) : Pipe(Pipe.Row, Pipe.Col);

internal record In(Pipe Pipe) : Pipe(Pipe.Row, Pipe.Col);

internal static class PipeExtensions
{
    public static Pipe[,] ToPipeSquare(this Pipe pipe)
    {
        var pipeSquare = pipe switch
        {
            Bound => new Pipe[3, 3]
            {
                { new Bound(0,0), new Bound(0,1), new Bound(0,2), },
                { new Bound(1,0), new Bound(1,1), new Bound(1,2), },
                { new Bound(2,0), new Bound(2,1), new Bound(2,2), },
            },
            Start => new Pipe[3, 3]
            {
                { new Soil(0,0), new BeenHere(new Vertical(0,1)), new Soil(0,2), },
                { new BeenHere(new Horizontal(1,0)), new Start(1,1), new BeenHere(new Horizontal(1,2)), },
                { new Soil(2,0), new BeenHere(new Vertical(2,1)), new Soil(2,2), },
            },
            Vertical => new Pipe[3, 3]
            {
                { new Soil(0,0), new Vertical(0,1), new Soil(0,2), },
                { new Soil(1,0), new Vertical(1,1), new Soil(1,2), },
                { new Soil(2,0), new Vertical(2,1), new Soil(2,2), },
            },
            Horizontal => new Pipe[3, 3]
            {
                { new Soil(0,0), new Soil(0,1), new Soil(0,2), },
                { new Horizontal(1,0), new Horizontal(1,1), new Horizontal(1,2), },
                { new Soil(2,0), new Soil(2,1), new Soil(2,2), },
            },
            TopToRight => new Pipe[3, 3]
            {
                { new Soil(0,0), new Vertical(0,1), new Soil(0,2), },
                { new Soil(1,0), new TopToRight(1,1), new Horizontal(1,2), },
                { new Soil(2,0), new Soil(2,1), new Soil(2,2), },
            },
            TopToLeft => new Pipe[3, 3]
            {
                { new Soil(0,0), new Vertical(0,1), new Soil(0,2), },
                { new Horizontal(1,0), new TopToLeft(1,1), new Soil(1,2), },
                { new Soil(2,0), new Soil(2,1), new Soil(2,2), },
            },
            RightToBottom => new Pipe[3, 3]
            {
                { new Soil(0,0), new Soil(0,1), new Soil(0,2), },
                { new Soil(1,0), new RightToBottom(1,1), new Horizontal(1,2), },
                { new Soil(2,0), new Vertical(2,1), new Soil(2,2), },
            },
            LeftToBottom => new Pipe[3, 3]
            {
                { new Soil(0,0), new Soil(0,1), new Soil(0,2), },
                { new Horizontal(1,0), new LeftToBottom(1,1), new Soil(1,2), },
                { new Soil(2,0), new Vertical(2,1), new Soil(2,2), },
            },
            BeenHere => ((BeenHere)pipe).Pipe.ToPipeSquare().Select(x => x is not Soil ? new BeenHere(x) : x),
            _ => new Pipe[3, 3]
            {
                { new Soil(0,0), new Soil(0,1), new Soil(0,2), },
                { new Soil(1,0), new Soil(1,1), new Soil(1,2), },
                { new Soil(2,0), new Soil(2,1), new Soil(2,2), },
            },
        } ;

        return pipeSquare;
    }

    public static bool IsDen(this Pipe pipe, Pipe[,] maze)
    {
        if (pipe is BeenHere || pipe is Start) return false;

        var foundPath = false;

        //up
        for (int i = pipe.Row; i > 0; i--)
        {
            if (maze[i, pipe.Col] is BeenHere)
            {
                foundPath = true;
                break;
            }
        }

        if (foundPath is false) return false;

        foundPath = false;

        //down
        for (int i = pipe.Row; i < maze.GetLength(0) - 1; i++)
        {
            if (maze[i, pipe.Col] is BeenHere)
            {
                foundPath = true;
                break;
            }
        }

        if (foundPath is false) return false;

        foundPath = false;

        //left
        for (int i = pipe.Col; i > 0; i--)
        {
            if (maze[pipe.Row, i] is BeenHere)
            {
                foundPath = true;
                break;
            }
        }

        if (foundPath is false) return false;

        foundPath = false;

        //right
        for (int i = pipe.Col; i < maze.GetLength(1) - 1; i++)
        {
            if (maze[pipe.Row, i] is BeenHere)
            {
                foundPath = true;
                break;
            }
        }

        return foundPath;
    }

    public static bool IsConnectedTo(this Pipe from, Pipe to)
    {
        if (from is Soil || to is Soil) return false;

        var direction = from.GetDirection(to);

        var isConnected = direction switch
        {
            Direction.Left => (from, to) switch
            {
                (Start or Horizontal or TopToLeft or LeftToBottom, Horizontal or RightToBottom or TopToRight) => true,
                _ => false,
            },
            Direction.Right => (from, to) switch
            {
                (Start or Horizontal or TopToRight or RightToBottom, Horizontal or LeftToBottom or TopToLeft) => true,
                _ => false,
            },
            Direction.Up => (from, to) switch
            {
                (Start or Vertical or TopToRight or TopToLeft, Vertical or LeftToBottom or RightToBottom) => true,
                _ => false,
            },
            Direction.Down => (from, to) switch
            {
                (Start or Vertical or RightToBottom or LeftToBottom, Vertical or TopToRight or TopToLeft) => true,
                _ => false,
            },
            _ => false,
        };

        return isConnected;
    }

    public static Direction GetDirection(this Pipe from, Pipe to)
    {
        return (from.Row - to.Row, from.Col - to.Col) switch
        {
            (0, +1) => Direction.Left,
            (0, -1) => Direction.Right,
            (+1, 0) => Direction.Up,
            (-1, 0) => Direction.Down,
            _ => Direction.None,
        };
    }

    public static Pipe[] Neighbours(this Pipe pipe, Pipe[,] maze) => new Pipe[]
    {
        maze[pipe.Row + 1, pipe.Col],
        maze[pipe.Row, pipe.Col + 1],
        maze[pipe.Row - 1, pipe.Col],
        maze[pipe.Row, pipe.Col - 1]
    };

    public static Pipe[] AllNeighbours(this Pipe pipe, Pipe[,] maze) => new Pipe[]
    {
        maze[pipe.Row + 1, pipe.Col],
        maze[pipe.Row, pipe.Col + 1],
        maze[pipe.Row - 1, pipe.Col],
        maze[pipe.Row, pipe.Col - 1],
        maze[pipe.Row - 1, pipe.Col - 1],
        maze[pipe.Row + 1, pipe.Col - 1],
        maze[pipe.Row + 1, pipe.Col + 1],
        maze[pipe.Row - 1, pipe.Col + 1],
    };

    public static string ToFriendlyString(this Pipe[,] maze)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                sb.Append(maze[i, j].ToFriendlyString());
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static string ToFriendlyString(this Pipe pipe) => pipe switch
    {
        In => "i",
        Out => "O",
        Bound => "@",
        Soil => ".",
        BeenHere => ((BeenHere)pipe).Pipe switch
        {
            Vertical => "│",
            Horizontal => "─",
            TopToLeft => "┘",
            TopToRight => "└",
            RightToBottom => "┌",
            LeftToBottom => "┐",
            _ => "X",
        },
        Start => "S",
        Den => "_",
        _ => ","
        //─│┐┌└┘
    };
}
