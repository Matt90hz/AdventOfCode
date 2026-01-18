open System
open System.IO
open System.Collections.Generic

let inputPath =
    match fsi.CommandLineArgs with
    | [| _; "-t" |] -> @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day11\input_test1.txt"
    | _ -> @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day11\input.txt"

let inputPath2 =
    match fsi.CommandLineArgs with
    | [| _; "-t" |] -> @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day11\input_test2.txt"
    | _ -> @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day11\input.txt"

let connections path =
    path
    |> File.ReadLines
    |> Seq.map (fun line ->
        match line.Split(':') with
        | [| a; b |] -> a, b.Split(' ', StringSplitOptions.RemoveEmptyEntries) |> List.ofArray
        | _ -> raise <| Exception "Not parsed correctly")
    |> Map.ofSeq

let possibleWaysOut =
    let connections = connections inputPath

    let rec findWaysOut paths =
        match paths with
        | [] -> 0
        | [ "out" ] -> 1
        | x :: r -> findWaysOut connections[x] + findWaysOut r

    findWaysOut [ "you" ]

printfn "%A" possibleWaysOut // 5 477

let pathsThroughDacAndFft =
    let connections = connections inputPath2
    let cache = Dictionary<string * string, int64>()

    let rec findWays f t =
        match cache.TryGetValue((f, t)) with
        | true, ways -> ways
        | false, _ ->
            match connections |> Map.tryFind f with
            | None -> 0L
            | Some next when next |> List.contains t -> 1L
            | Some next ->
                let ways = next |> List.sumBy (fun n -> findWays n t)
                cache.Add((f, t), ways)
                ways

    let svr_dac = findWays "svr" "dac"
    let dac_fft = findWays "dac" "fft"
    let fft_out = findWays "fft" "out"
    let svr_fft = findWays "svr" "fft"
    let fft_dac = findWays "fft" "dac"
    let dac_out = findWays "dac" "out"

    (svr_dac * dac_fft * fft_out) + (svr_fft * fft_dac * dac_out)

printfn "%A" pathsThroughDacAndFft // 2 383307150903216
