using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day2;

internal enum Cube { Red, Green, Blue }

internal record Game(int Id, Reveal[] Reveals);

internal record Reveal(int Num, Cube Cube);

internal static class CubesGame
{
    public static int PossibleGames(string input)
    {
        var games = GetGames(input);
        var maxReds = 12;
        var maxGreens = 13;
        var maxBlues = 14;

        var possibleGames = games.Where(game =>
            game.Reveals.All(reveal => 
                reveal.Cube switch
                {
                    Cube.Red => reveal.Num <= maxReds,
                    Cube.Green => reveal.Num <= maxGreens,
                    _ => reveal.Num <= maxBlues,
                }));

        return possibleGames.Sum(game => game.Id);
    }

    public static int MaxCubesPossibe(string input)
    {
        var games = GetGames(input);

        var maxCubes = games.Select(game => 
            game.MaxCube(Cube.Red) * game.MaxCube(Cube.Green) * game.MaxCube(Cube.Blue));

        return maxCubes.Sum();
    }

    static Game[] GetGames(string input)
    {
        var lines = input.Split('\n');
        var games = lines.Select(line => new Game(GetGameId(line), GetReveals(line)));

        return games.ToArray();
    }

    static Reveal[] GetReveals(string line)
    {
        var revealsString = line.Split(':')[1];
        var revealStrings = revealsString.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
        var reveals = revealStrings.Select(GetReveal);

        return reveals.ToArray();
    }

    static Reveal GetReveal(string revealString)
    {
        var revealNumCubeString = revealString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var num = int.Parse(revealNumCubeString[0]);
        var cube = Enum.Parse<Cube>(revealNumCubeString[1], ignoreCase: true);

        return new Reveal(num, cube);
    }

    static int GetGameId(string line)
    {
        var firstSpaceIndex = line.IndexOf(' ');
        var colomnIndex = line.IndexOf(':');
        var id = line[firstSpaceIndex..colomnIndex];

        return int.Parse(id);
    }
}

internal static class GamesExtensions 
{
    public static int MaxCube(this Game game, Cube cube) => game.Reveals
        .Where(x => x.Cube == cube)
        .MaxBy(x => x.Num)?.Num ?? 0;
}