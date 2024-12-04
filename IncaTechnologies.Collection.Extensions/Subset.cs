namespace IncaTechnologies.Collection.Extensions
{
    public static class Subset
    {
        public static T[,] GetSquare<T>(this T[,] @this, long row, long column, long width)
            => @this.GetPortion<T>(row, column, width, width);

        public static T[,] GetPortion<T>(this T[,] @this, long row, long column, long width, long height)
        {
            var portion = new T[height, width];

            for (long i = row; i < height; i++)
            {
                for (long j = column; j < width; j++)
                {
                    portion[i - row, j - height] = @this[i, j];
                }
            }

            return portion;
        }
    }
}
