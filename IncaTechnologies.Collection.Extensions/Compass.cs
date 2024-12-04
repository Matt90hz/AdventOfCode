using System.Collections.Generic;

namespace IncaTechnologies.Collection.Extensions
{
    public static class Compass
    {
        public static IEnumerable<IPosition<T>> GetNorth<T>(this IPosition<T> position)
        {
            for (long i = position.Row - 1; i >= 0; i--)
            {
                yield return position.Array.GetPosition(i, position.Column);
            }
        }

        public static IEnumerable<IPosition<T>> GetSouth<T>(this IPosition<T> position)
        {
            for (long i = position.Row + 1; i < position.Array.GetLongLength(0); i++)
            {
                yield return position.Array.GetPosition(i, position.Column);
            }
        }

        public static IEnumerable<IPosition<T>> GetEast<T>(this IPosition<T> position)
        {
            for (long i = position.Column + 1; i < position.Array.GetLongLength(1); i++)
            {
                yield return position.Array.GetPosition(position.Row, i);
            }
        }

        public static IEnumerable<IPosition<T>> GetWest<T>(this IPosition<T> position)
        {
            for (long i = position.Column - 1; i >= 0; i--)
            {
                yield return position.Array.GetPosition(position.Row, i);
            }
        }

        public static IEnumerable<IPosition<T>> GetNorthEast<T>(this IPosition<T> position)
        {
            for (long i = position.Row - 1, j = position.Column + 1; i >= 0 && j < position.Array.GetLongLength(1); i--, j++)
            {
                yield return position.Array.GetPosition(i, j);
            }
        }

        public static IEnumerable<IPosition<T>> GetNorthWest<T>(this IPosition<T> position)
        {
            for (long i = position.Row - 1, j = position.Column - 1; i >= 0 && j >= 0; i--, j--)
            {
                yield return position.Array.GetPosition(i, j);
            }
        }

        public static IEnumerable<IPosition<T>> GetSouthEast<T>(this IPosition<T> position)
        {
            for (long i = position.Row + 1, j = position.Column + 1; i < position.Array.GetLongLength(0) && j < position.Array.GetLongLength(1); i++, j++)
            {
                yield return position.Array.GetPosition(i, j);
            }
        }

        public static IEnumerable<IPosition<T>> GetSouthWest<T>(this IPosition<T> position)
        {
            for (long i = position.Row + 1, j = position.Column - 1; i < position.Array.GetLongLength(0) && j >= 0; i++, j--)
            {
                yield return position.Array.GetPosition(i, j);
            }
        }
    }
}
