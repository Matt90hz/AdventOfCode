namespace AdventOfCode2024.Day04;
public static class CeresSearch
{
    public static int CountXMAS(string input)
    {
        char[,] letters = ParseLetters(input);
        int xmasCount = 0;

        for (int i = 0; i < letters.GetLength(0); i++)
        {
            for (int j = 0; j < letters.GetLength(1); j++)
            {
                xmasCount += Check(letters, i, j, x => x.GetEast());
                xmasCount += Check(letters, i, j, x => x.GetWest());
                xmasCount += Check(letters, i, j, x => x.GetNorth());
                xmasCount += Check(letters, i, j, x => x.GetSouth());
                xmasCount += Check(letters, i, j, x => x.GetNorthEast());
                xmasCount += Check(letters, i, j, x => x.GetNorthWest());
                xmasCount += Check(letters, i, j, x => x.GetSouthEast());
                xmasCount += Check(letters, i, j, x => x.GetSouthWest());
            }
        }

        return xmasCount;
    }

    private static int Check(char[,] letters, int row, int col, Func<IPosition<char>, IEnumerable<IPosition<char>>> direction)
    {
        if (letters[row, col] != 'X') return 0;

        var position = letters.GetPosition(row, col);

        var nextChar = direction(position)
            .Take(3)
            .Select(x => x.Value);

        var isXMas = nextChar.SequenceEqual(['M', 'A', 'S']);

        return isXMas ? 1 : 0;
    }

    private static char[,] ParseLetters(string input)
    {
        var letters = input
            .Split(Environment.NewLine)
            .Select(x => x.ToCharArray())
            .ToMultidimensionalArray();

        return letters;
    }
}
