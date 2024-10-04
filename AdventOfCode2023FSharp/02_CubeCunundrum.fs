
namespace AdventOfCode2023FSharp
    
module CubeCunundrum =

    type Color = 
        | Red
        | Green
        | Blue

    type Draw = { Num: int; Color: Color }
    type Set = { Set: Draw array }
    type Game = { Id: int; Subsets: Set array }

    open System.IO
    open System;

    let private parseGames (input: string) =

        let parseColor (input: string) =
            match input with
            | "Red" -> Red
            | "Green" -> Green
            | _ -> Blue

        let parseDraw (input: string) =
            let x = input.Split(' ', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
            let num = int x[0]
            let color = parseColor x[1]
            { Num = num; Color = color }

        let parseSet (input: string) =
            let x = input.Split(",", StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
            let set = Array.map parseDraw x
            { Set = set }

        let parseSubstes (input: string) = 
            input.Split(';')
            |> Array.map parseSet

        let parseId (input: string) =
            let x = input.Split(" ", StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
            int x[1]

        let parseGame (input: string) = 
            let x = input.Split(":", StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
            let id = parseId x[0]
            let subsets = parseSubstes x[1]
            { Id = id; Subsets = subsets }

        input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.map parseGame

    let inputTest = File.ReadAllText @"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day2\input_test.txt"

    let input = File.ReadAllText @"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day2\input.txt"

    let possibleGames (input: string) =
        
        let possibleDraw (draw: Draw) =
            match draw.Color with
            | Red -> draw.Num <= 12
            | Green -> draw.Num <= 13
            | Blue -> draw.Num <= 14

        let possibleSet (set:Set) =
            set.Set
            |> Array.forall possibleDraw

        let possibleGame (game: Game) =
            game.Subsets
            |> Array.forall possibleSet

        input
        |> parseGames
        |> Array.filter possibleGame
        |> Array.sumBy (fun game -> game.Id)