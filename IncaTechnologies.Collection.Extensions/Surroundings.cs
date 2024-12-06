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
                if (down.TryMoveRight(out var downLeft)) yield return downLeft;
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
    }
}
