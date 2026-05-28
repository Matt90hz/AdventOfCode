open System
open System.IO

let isSolvable (lists: 'a list list) (transformers: ('a list -> 'a list) list) (limits: 'a -> bool) =
    // Placeholder for the actual logic to determine if the puzzle is solvable
    true

let rotate (items: (int * int) list) : (int * int) list = 
    items |> List.map (fun (x, y) -> y, 2 - x)

let flipHorizontal (items: (int * int) list) : (int * int) list = items

let flipVertical (items: (int * int) list) : (int * int) list = items

let parse (input: string) : ((int * int) list list * ((int * int) list -> (int * int) list) list * (int * int -> bool)) list =
    let listsTypes =
        input
        |> _.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        |> Seq.where (fun line -> not (line.Contains 'x'))
        |> Seq.map (fun line -> line.ToCharArray())
        |> Seq.windowed 5
        |> Seq.map (fun line -> 
            line
            |> Seq.skip 1 // Skip the first row (header)
            |> Seq.mapi (fun i row -> row |> Seq.mapi (fun j c -> i, j, c))
            |> Seq.collect (fun x -> x)
            |> Seq.filter (fun (_, _, c) -> c = '#')
            |> Seq.map (fun (i, j, _) -> i, j)
            |> List.ofSeq)
        |> List.ofSeq

    let lastSection =
        input
        |> _.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        |> Seq.where (fun line -> line.Contains 'x')
        |> Seq.map (fun line -> line.Split ':')

    let multipliers =
        lastSection
        |> Seq.map (fun sub ->
            sub
            |> Array.last
            |> _.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            |> Array.map int)
        |> List.ofSeq

    let sizes =
        lastSection
        |> Seq.map (fun sub -> sub |> Array.head |> _.Split('x') |> Array.map int)
        |> List.ofSeq

    let transformers = [ rotate; flipHorizontal; flipVertical ]

    (sizes, multipliers)
    ||> Seq.map2 (fun size multiplier ->
        let lists =
            multiplier
            |> Seq.indexed
            |> Seq.collect (fun (i, m) -> List.replicate m listsTypes[i])
            |> List.ofSeq

        let limit = fun (x, y) -> x < size[0] && y < size[1]
        lists, transformers, limit)
    |> List.ofSeq

/// Solve the "Christmas Tree Farm" puzzle (see context.md).
/// Accepts the full puzzle input as a single string and returns a boolean result.
let solve (input: string) : bool seq =
    input |> parse |> Seq.map ((<|||) isSolvable)

@"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day12\input.txt"
|> File.ReadAllText
|> parse
|> printf "%A" // true
