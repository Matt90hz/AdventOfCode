using Pos = IncaTechnologies.Collection.Extensions.Position<char>;
using Path = System.Collections.Generic.Stack<(IncaTechnologies.Collection.Extensions.Position<char> Pos, IncaTechnologies.Collection.Extensions.Direction Dir)>;
namespace AdventOfCode2024.Day16;
public static class ReindeerMaze
{
    public static int LowestScore(string input)
    {
        var maze = ParseMaze(input);
        var start = maze.FindPosition('S') ?? throw new Exception("Start not found");

        var path = new Path();
        path.Push((start, Direction.Right));

        var pq = new PriorityQueue<Path, int>();
        pq.Enqueue(path, 0);

        while(pq.TryDequeue(out path, out var p))
        {
            if (path.Peek() is { Pos.Value: 'E' }) return p;

            var n = Next(path);

            foreach(var (x, sc) in n)
            {
                pq.Enqueue(x, p + sc);
            }
        }

        throw new Exception("Exit noy found");
    }

    private static IEnumerable<(Path, int)> Next(Path path)
    {
        var (pos, dir) = path.Pop();
        var positions = path.Select(x => x.Pos).ToArray();
        var adjacent = pos
            .GetAdjacent()
            .Where(x => positions.Contains(x) is false && x is { Value: '.' or 'E'});

        var next = adjacent.Select(a =>
        {
            var aDir = Navigation.GetDirection(pos, a);
            var score = aDir == dir ? 1 : 1001;
            var newPath = new Path(path);
            newPath.Push((pos, aDir));
            newPath.Push((a, aDir));

            return (newPath, score);
        });
        
        return next.ToArray();
    }

    private static char[,] ParseMaze(string input)
    {
        var maze = input
            .Split(Environment.NewLine)
            .Select(x => x.ToCharArray())
            .ToMultidimensionalArray();

        return maze;
    } 
}
