using System.Collections.Generic;

namespace IncaTechnologies.Collection.Extensions
{
    public static class Surroundings
    {
        public static IEnumerable<Position<T>> GetNeighbors<T>(this Position<T> position)
        {
            if (position.TryMoveUp(out var up))
            {
                if (up.TryMoveLeft(out var upLeft)) yield return upLeft;
                yield return up;
                if (up.TryMoveRight(out var upRight)) yield return upRight;
            }

            if (position.TryMoveRight(out var right)) yield return right;

            if (position.TryMoveDown(out var down))
            {
                if (down.TryMoveRight(out var downRight)) yield return downRight;
                yield return down;
                if (down.TryMoveLeft(out var downLeft)) yield return downLeft;
            }

            if (position.TryMoveLeft(out var left)) yield return left;
        }

        public static IEnumerable<Position<T>> GetAdjacent<T>(this Position<T> position)
        {
            if (position.TryMoveUp(out var up)) yield return up;
            if (position.TryMoveRight(out var right)) yield return right;
            if (position.TryMoveDown(out var down)) yield return down;
            if (position.TryMoveLeft(out var left)) yield return left;
        }

        public static IEnumerable<Position<T>> GetReachable<T>(this Position<T> position, int distance)
        {
            var (array, r, c) = position;

            for (int i = 1; i <= distance; i++)
            {
                for (int j = 0; j <= distance - i; j++)
                {
                    var reachable = array.GetPosition(r - i, c + j);
                    if (!reachable.IsOutOfBound()) yield return reachable;

                    reachable = array.GetPosition(r + i, c - j);
                    if (!reachable.IsOutOfBound()) yield return reachable;

                    reachable = array.GetPosition(r + j, c + i);
                    if (!reachable.IsOutOfBound()) yield return reachable;

                    reachable = array.GetPosition(r - j, c - i);
                    if (!reachable.IsOutOfBound()) yield return reachable;
                }
            }
        }
    }
}
