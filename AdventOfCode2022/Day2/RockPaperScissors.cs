using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day2;

public enum Hand { Rock = 1, Paper = 2, Scissors = 3 }

public enum Score { Win = 6, Draw = 3, Loss = 0 }

static class RockPaperScissors
{
    public static int TotalScore(string input) => input
        .Split(Environment.NewLine)
        .Select(line => (Other: GetHand(line[0]), Me: GetHand(line[2])))
        .Select(game => (int)GetScore(game.Me, game.Other) + (int)game.Me)
        .Sum();

    public static int TotalScoreRightWay(string input) => input
        .Split(Environment.NewLine)
        .Select(line => (Other: GetHand(line[0]), Score: GetScore(line[2])))
        .Select(game => game switch
        {
            (Hand.Rock, Score.Loss) => (Me: Hand.Scissors, Other: Hand.Rock),
            (Hand.Rock, Score.Win) => (Me: Hand.Paper, Other: Hand.Rock),
            (Hand.Paper, Score.Loss) => (Me: Hand.Rock, Other: Hand.Paper),
            (Hand.Paper, Score.Win) => (Me: Hand.Scissors, Other: Hand.Paper),
            (Hand.Scissors, Score.Loss) => (Me: Hand.Paper, Other: Hand.Scissors),
            (Hand.Scissors, Score.Win) => (Me: Hand.Rock, Other: Hand.Scissors),
            (var other, Score.Draw) => (Me: other, Other: other),
            _ => throw new Exception("Unreachable!")
        })
        .Select(game => (int)GetScore(game.Me, game.Other) + (int)game.Me)
        .Sum();

    static Hand GetHand(char c) => c switch
    {
        'A' or 'X' => Hand.Rock,
        'B' or 'Y' => Hand.Paper,
        'C' or 'Z' => Hand.Scissors,
        _ => throw new ArgumentException($"Invalid! {c}.")
    };

    static Score GetScore(Hand me, Hand other) => (me, other) switch
    {
        (Hand.Rock, Hand.Paper) => Score.Loss,
        (Hand.Paper, Hand.Scissors) => Score.Loss,
        (Hand.Scissors, Hand.Rock) => Score.Loss,
        (Hand.Rock, Hand.Scissors) => Score.Win,
        (Hand.Paper, Hand.Rock) => Score.Win,
        (Hand.Scissors, Hand.Paper) => Score.Win,
        _ => Score.Draw
    };

    static Score GetScore(char c) => c switch
    {
        'X' => Score.Loss,
        'Y' => Score.Draw,
        'Z' => Score.Win,
        _ => throw new ArgumentException($"Invalid! {c}.")
    };
}
