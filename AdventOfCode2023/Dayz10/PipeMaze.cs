using System.Text;

namespace AdventOfCode2023.Dayz10;

internal static class PipeMaze
{
    public static int AnimalDens(string[] input)
    {
        var maze = Create(input);

        //enlarge the maze
        var enlargedMaze = maze.Select(x => x.ToPipeSquare(maze)).Normalize();

        //solve maze
        var solvedMaze = Solve(enlargedMaze);

        //clear useless data
        var clearedMeze = solvedMaze.Select(x => x is BeenHere or Bound or Start ? x : new Soil(x.Row, x.Col));

        //eplore the outbounds
        var curr = clearedMeze[3, 3];

        var neighbour = curr
            .AllNeighbours(clearedMeze)
            .Where(x => x is Soil)
            .ToArray();

        var toExplore = new List<Pipe>(neighbour);

        while(toExplore.Count > 0)
        {
            foreach(var thing in toExplore)
            {
                clearedMeze[thing.Row, thing.Col] = new Out(thing);
            }

            var nextToExplore = toExplore
                .SelectMany(x => x.AllNeighbours(clearedMeze).Where(x => x is Soil))
                .Distinct()
                .ToArray();

            toExplore.Clear();
            toExplore.AddRange(nextToExplore);

            //Task.Delay(25).Wait();
            //Console.Clear();
            //Console.Write(clearedMeze.ToFriendlyString());
        }

        //count inside
        int inside = 0;

        foreach (var thing in clearedMeze)
        {
            if (thing is Soil soil && thing.AllNeighbours(clearedMeze).All(x => x is Soil)) inside++;
        }

        return inside/9;
    }

    public static int Depth(string[] input)
    {
        var maze = Create(input);

        var start = maze.FindStart();
        var neighbours = start.Neighbours(maze).Where(start.IsConnectedTo);
        var path1 = neighbours.First();
        var path2 = neighbours.Last();
        var depth = 1;

        while (path1 != path2)
        {
            var nextPath1 = path1.Neighbours(maze).First(path1.IsConnectedTo);
            var nextpath2 = path2.Neighbours(maze).First(path2.IsConnectedTo);

            maze[path1.Row, path1.Col] = new BeenHere(path1);
            maze[path2.Row, path2.Col] = new BeenHere(path2);

            path1 = nextPath1;
            path2 = nextpath2;

            depth++;
        }

        return depth;
    }

    public static Pipe[,] Solve(Pipe[,] maze)
    {
        var start = maze.FindStart();

        var neighbours = start.Neighbours(maze).Where(start.IsConnectedTo);
        var path1 = neighbours.First();
        var path2 = neighbours.Last();

        while (path1 != path2)
        {
            var nextPath1 = path1.Neighbours(maze).First(path1.IsConnectedTo);
            var nextpath2 = path2.Neighbours(maze).First(path2.IsConnectedTo);

            maze[path1.Row, path1.Col] = new BeenHere(path1);
            maze[path2.Row, path2.Col] = new BeenHere(path2);

            path1 = nextPath1;
            path2 = nextpath2;

            //Task.Delay(25).Wait();
            //Console.Clear();
            //Console.Write(maze.ToFriendlyString());
        }

        maze[path1.Row, path1.Col] = new BeenHere(path1);

        var x = maze.ToFriendlyString();

        return maze;
    }

    public static Pipe[,] Create(string[] lines)
    {
        var linesWithBounds = SurroundWith(lines, 'B');

        var maze = new Pipe[linesWithBounds.Length, linesWithBounds[0].Length];

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                maze[i, j] = linesWithBounds[i][j] switch
                {
                    '|' => new Vertical(i, j),
                    '-' => new Horizontal(i, j),
                    'L' => new TopToRight(i, j),
                    'J' => new TopToLeft(i, j),
                    '7' => new LeftToBottom(i, j),
                    'F' => new RightToBottom(i, j),
                    'S' => new Start(i, j),
                    'B' => new Bound(i, j),
                    _ => new Soil(i, j),
                };
            }
        }

        return maze;
    }

    public static T[,] Select<T>(this Pipe[,] maze, Func<Pipe, T> selector)
    {
        var newMaze = new T[maze.GetLength(0), maze.GetLength(1)];

        for (int i = 0; i < newMaze.GetLength(0); i++)
        {
            for (int j = 0; j < newMaze.GetLength(1); j++)
            {
                newMaze[i, j] = selector(maze[i, j]); 
            }
        }

        return newMaze;
    }

    public static Pipe[,] Normalize(this Pipe[,][,] maze)
    {
        var inRowLeght = maze[0, 0].GetLength(0);
        var inColLeght = maze[0, 0].GetLength(1);
        var rowLenght = maze.GetLength(0) * inRowLeght;
        var colLenght = maze.GetLength(1) * inColLeght;
        
        var newMaze = new Pipe[rowLenght, colLenght];

        for (int i = 0; i < maze.GetLength(0); i++)
        { 
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                var x = maze[i, j].ToFriendlyString();

                foreach(var pipe in maze[i, j])
                {
                    var row = i * inRowLeght + pipe.Row;
                    var col = j * inColLeght + pipe.Col;

                    newMaze[row, col] = pipe with { Row = row, Col = col };
                }
            }
        }

        return newMaze;
    }

    public static Pipe? Find(this Pipe[,] maze, Func<Pipe, bool> predicate)
    {
        foreach (var pipe in maze)
        {
            if (predicate(pipe)) return pipe;
        }

        return null;
    }

    static Start FindStart(this Pipe[,] maze) => maze.Find(x => x is Start) as Start
        ?? throw new Exception("Start not found! Maze must have a start.");

    static string[] SurroundWith(string[] lines, char c)
    {
        var lineOfChar = new string(Enumerable.Repeat(c, lines[0].Length).ToArray());

        var addTopBottom = lines
            .Prepend(lineOfChar)
            .Append(lineOfChar);

        var addLeftRight = addTopBottom.Select(x => $"{c}{x}{c}");

        var surrounded = addLeftRight.ToArray();

        return surrounded;
    }

}