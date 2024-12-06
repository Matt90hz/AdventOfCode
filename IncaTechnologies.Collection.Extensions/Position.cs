using System;

namespace IncaTechnologies.Collection.Extensions
{
    public struct Position<T> : IEquatable<Position<T>>
    {
        public T[,] Array { get; }

        public long Row { get; internal set; }

        public long Column { get; internal set; }

        public readonly T Value
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

        public readonly void Deconstruct(out T[,] array, out long row, out long column)
        {
            array = Array;
            row = Row;
            column = Column;
        }

        public readonly void Deconstruct(out long row, out long column)
        {
            row = Row;
            column = Column;
        }

        public readonly bool Equals(Position<T> other) => Row == other.Row && Column == other.Column;

        public override readonly bool Equals(object obj) => obj is Position<T> other && Equals(other);

        public readonly override string ToString() => $"[{Row}, {Column}] {Value}";

        public override readonly int GetHashCode() => base.GetHashCode(); // to do

        public static bool operator ==(Position<T> x, Position<T> y) => x.Equals(y);

        public static bool operator !=(Position<T> x, Position<T> y) => !x.Equals(y);
    }
}
