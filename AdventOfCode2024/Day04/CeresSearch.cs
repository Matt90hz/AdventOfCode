using IncaTechnologies.Collection.Extensions;

namespace AdventOfCode2024.Day04;
public static class CeresSearch
{
    public static int CountXMAS(string input)
    {
        char[,] letters = ParseLetters(input);
        int count = 0;

        for (int i = 0; i < letters.GetLength(0); i++)
        {
            for (int j = 0; j < letters.GetLength(1); j++)
            {
                count += CountIfXMAS(letters, i, j, x => x.GetEast());
                count += CountIfXMAS(letters, i, j, x => x.GetWest());
                count += CountIfXMAS(letters, i, j, x => x.GetNorth());
                count += CountIfXMAS(letters, i, j, x => x.GetSouth());
                count += CountIfXMAS(letters, i, j, x => x.GetNorthEast());
                count += CountIfXMAS(letters, i, j, x => x.GetNorthWest());
                count += CountIfXMAS(letters, i, j, x => x.GetSouthEast());
                count += CountIfXMAS(letters, i, j, x => x.GetSouthWest());
            }
        }

        return count;
    }

    public static int CountX_MAS(string input)
    {
        char[,] letters = ParseLetters(input);

        int count = 0;

        for (int i = 1; i < letters.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < letters.GetLength(1) - 1; j++)
            {
                if (letters[i, j] != 'A') continue;
                
                var ne = letters[i - 1, j + 1];
                var nw = letters[i - 1, j - 1];
                var se = letters[i + 1, j + 1];
                var sw = letters[i + 1, j - 1]; 

                var is_ne_sw_MAS = (nw == 'M' && se == 'S') || (nw == 'S' && se == 'M');
                var is_nw_se_MAS = (ne == 'M' && sw == 'S') || (ne == 'S' && sw == 'M');

                var is_x_MAS = is_ne_sw_MAS && is_nw_se_MAS;

                if (is_x_MAS) count++;
            }
        }

        return count;
    }

    private static int CountIfXMAS(char[,] letters, int row, int col, Func<IPosition<char>, IEnumerable<IPosition<char>>> direction)
    {
        if (letters[row, col] != 'X') return 0;

        var position = letters.GetPosition(row, col);

        var maybeXMAS = direction(position)
            .Take(3)
            .Select(x => x.Value);

        var isXMas = maybeXMAS.SequenceEqual(['M', 'A', 'S']);

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
