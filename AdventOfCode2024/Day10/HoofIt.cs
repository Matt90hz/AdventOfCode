namespace AdventOfCode2024.Day10;
public static class HoofIt
{
    public static int TrailheadsScore(string input)
    {
        var map = ParseMap(input);
        var start = map.GetPositions().Where(x => x.Value == 0);
        var scores = start.Select(GetScore);
        var sum = scores.Sum();
        return sum;
    }

    public static int TrailheadsRating(string input)
    {
        var map = ParseMap(input);
        var start = map.GetPositions().Where(x => x.Value == 0);
        var scores = start.Select(GetRating);
        var sum = scores.Sum();
        return sum;
    }

    private static int GetRating(Position<int> start)
    {
        var (map, _, _) = start;

        var stack = new Stack<Stack<Position<int>>>();
        var trail = new Stack<Position<int>>();

        trail.Push(start);
        stack.Push(trail);

        int score = 0;

        while (stack.TryPop(out trail))
        {
            var next = trail.Pop();

            if (next.Value == 9)
            {
                score++;
                continue;
            }

            var adjacent = next.GetAdjacent().Where(x =>
            {
                bool not_seen = trail.Contains(x) is false;
                bool valid = x.Value == next.Value + 1;

                return not_seen && valid;
            });

            if (adjacent.Any() is false) continue;

            foreach (var x in adjacent.Skip(1))
            {
                var newTrail = new Stack<Position<int>>(trail);
                newTrail.Push(x);
                stack.Push(newTrail);
            }

            trail.Push(adjacent.First());
            stack.Push(trail);
        }

        return score;
    }

    private static int GetScore(Position<int> start)
    {
        var (map, _, _) = start;

        var trails = map.Select(x => false);

        var stack = new Stack<Position<int>>();

        stack.Push(start);

        int score = 0;

        while (stack.TryPop(out var next)) 
        {
            var (r, c) = next;

            trails[r, c] = true;

            if (next.Value == 9) 
            {
                score++;
                continue;
            }

            var adjacent = next.GetAdjacent();

            foreach (var x in adjacent)
            {
                bool seen = trails[x.Row, x.Column];
                bool invalid = x.Value != next.Value + 1;

                if (seen || invalid) continue;

                stack.Push(x);
            }
        }

        return score;
    }

    private static int[,] ParseMap(string input)
    {
        var map = input
            .Split(Environment.NewLine)
            .Select(x => x.Select(x => x -'0'))
            .ToMultidimensionalArray();

        return map;
    }
}
