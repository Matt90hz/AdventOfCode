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

        var priorityQueue = new PriorityQueue<Path, int>();
        priorityQueue.Enqueue(path, 0);

        var pathCache = new Dictionary<Pos, Path>
        {
            { start, path }
        };

        var priorityCache = new Dictionary<Pos, int>()
        {
            { start, 0 }
        };

        while (priorityQueue.TryDequeue(out path, out var pathScore))
        {
            //Animate(path);
            if (path.Peek() is { Pos.Value: 'E' }) return pathScore;

            var nextPaths = NextPaths(path);

            foreach (var (nextPath, scoreIncrease) in nextPaths)
            {
                var newScore = pathScore + scoreIncrease;
                var cacheKey = nextPath.Peek().Pos;

                if (priorityCache.TryGetValue(nextPath.Peek().Pos, out var cachedScore))
                {
                    if (cachedScore > newScore)
                    {
                        var cachedPath = pathCache[cacheKey];
                        priorityQueue.Remove(cachedPath, out _, out _);
                        pathCache[cacheKey] = nextPath;
                        priorityCache[cacheKey] = newScore;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    pathCache.Add(cacheKey, nextPath);
                    priorityCache.Add(cacheKey, newScore);
                }

                priorityQueue.Enqueue(nextPath, newScore);
            }
        }

        throw new Exception("Exit noy found");
    }

    private static IEnumerable<(Path Path, int ScoreIncrease)> NextPaths(Path path)
    {
        var (pos, dir) = path.Pop();
        var prevPos = path.Count > 1 ? path.Peek().Pos : pos;

        var adjacentPositions = pos
            .GetAdjacent()
            .Where(adjPos => prevPos != adjPos && adjPos is { Value: '.' or 'E' });

        var nextPaths = adjacentPositions.Select(a =>
        {
            var aDir = Navigation.GetDirection(pos, a);
            var score = aDir == dir ? 1 : 1001;

            var newPath = new Path(path);
            newPath.Push((pos, aDir));
            newPath.Push((a, aDir));

            return (newPath, score);
        });

        return nextPaths;
    }

    private static char[,] ParseMaze(string input)
    {
        var maze = input
            .Split(Environment.NewLine)
            .Select(x => x.ToCharArray())
            .ToMultidimensionalArray();

        return maze;
    }

    private static void Animate(Path path)
    {
        var maze = path.First().Pos.Array.Select(x => x);

        foreach (var ((r, c), dir) in path)
        {
            maze[r, c] = dir switch
            {
                Direction.Up => '^',
                Direction.Down => 'v',
                Direction.Left => '<',
                Direction.Right => '>',
                _ => throw new NotImplementedException(),
            };
        }

        var str = maze.ToFriendlyString();

        Console.CursorTop = 0;
        Console.CursorLeft = 0;
        Console.CursorVisible = false;
        Console.Write(str);
    }
}
