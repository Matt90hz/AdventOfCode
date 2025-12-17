open System
open System.IO

//let input = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day01\input_test.txt"
let input = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day01\input.txt"

let steps =
    File.ReadLines(input)
    |> Seq.map (fun line -> if line[0] = 'R' then int line[1..] else -int(line[1..]))

let password =
    steps
    |> Seq.scan (fun position step -> (position + step) % 100) 50
    |> Seq.filter (fun position -> position = 0)
    |> Seq.length //982

let passwordNewProtocol =

    let rotate position step = (((position + step) % 100) + 100) % 100

    let timesPassedZero position step =
        match Math.DivRem(step, 100) with
        | d, r when step >= 0 && position + r >= 100 -> d + 1
        | d, _ when step >= 0 -> d
        | d, r when position <> 0 && position + r <= 0 -> -d + 1
        | d, _ -> -d

    steps
    |> Seq.scan (fun (pos, _) step -> rotate pos step, timesPassedZero pos step) (50, 0)
    |> Seq.sumBy (fun (_, pass) -> pass) //6106
