using System.Collections.Generic;

namespace IncaTechnologies.Collection.Extensions
{
    public static class Compass
    {
        public static IEnumerable<Position<T>> GetNorth<T>(this Position<T> position)
        {
            for (long i = position.Row - 1; i >= 0; i--)
            {
                position.Row = i;
                yield return position;
            }
        }

        public static IEnumerable<Position<T>> GetSouth<T>(this Position<T> position)
        {
            for (long i = position.Row + 1; i < position.Array.GetLongLength(0); i++)
            {
                position.Row = i;
                yield return position;
            }
        }

        public static IEnumerable<Position<T>> GetEast<T>(this Position<T> position)
        {
            for (long i = position.Column + 1; i < position.Array.GetLongLength(1); i++)
            {
                position.Column = i;
                yield return position;
            }
        }

        public static IEnumerable<Position<T>> GetWest<T>(this Position<T> position)
        {
            for (long i = position.Column - 1; i >= 0; i--)
            {
                position.Column = i;
                yield return position;
            }
        }

        public static IEnumerable<Position<T>> GetNorthEast<T>(this Position<T> position)
        {
            for (long i = position.Row - 1, j = position.Column + 1; i >= 0 && j < position.Array.GetLongLength(1); i--, j++)
            {
                position.Row = i;
                position.Column = j;
                yield return position;
            }
        }

        public static IEnumerable<Position<T>> GetNorthWest<T>(this Position<T> position)
        {
            for (long i = position.Row - 1, j = position.Column - 1; i >= 0 && j >= 0; i--, j--)
            {
                position.Row = i;
                position.Column = j;
                yield return position;
            }
        }

        public static IEnumerable<Position<T>> GetSouthEast<T>(this Position<T> position)
        {
            for (long i = position.Row + 1, j = position.Column + 1; i < position.Array.GetLongLength(0) && j < position.Array.GetLongLength(1); i++, j++)
            {
                position.Row = i;
                position.Column = j;
                yield return position;
            }
        }

        public static IEnumerable<Position<T>> GetSouthWest<T>(this Position<T> position)
        {
            for (long i = position.Row + 1, j = position.Column - 1; i < position.Array.GetLongLength(0) && j >= 0; i++, j--)
            {
                position.Row = i;
                position.Column = j;
                yield return position;
            }
        }
    }
}
