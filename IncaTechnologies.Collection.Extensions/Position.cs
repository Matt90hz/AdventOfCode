using System;

namespace IncaTechnologies.Collection.Extensions
{
    public interface IPosition<T> : IEquatable<IPosition<T>>
    {
        T[,] Array { get; }

        long Row { get; }

        long Column { get; }

        T Value
        {
            get => Array[Row, Column];
            set => Array[Row, Column] = value;
        }

    }

    internal class Position<T> : IPosition<T>, IEquatable<IPosition<T>>
    {
        public T[,] Array { get; }

        public long Row { get; }

        public long Column { get; }

        public T Value
        {
            get => Array[Row, Column];
            set => Array[Row, Column] = value;
        }

        internal Position(T[,] array, long row, long column)
        {
            Array = array;
            Row = row;
            Column = column;
        }

        public bool Equals(IPosition<T> other) => Row == other.Row && Column == other.Column;

        public bool Equals(Position<T> other) => Row == other.Row && Column == other.Column;

        public override bool Equals(object obj) => obj is IPosition<T> other && Equals(other);

        public override string ToString()
        {
            return $"[{Row}, {Column}] {Value}";
        }

        public override int GetHashCode() //to do
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Position<T> x, Position<T> y) => x.Equals(y);

        public static bool operator !=(Position<T> x, Position<T> y) => !x.Equals(y);
    }
}
