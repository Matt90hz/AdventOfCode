open System
open System.IO

let inputPath =
    match fsi.CommandLineArgs with
    | [| _; "-t" |] -> @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day12\input_test.txt"
    | _ -> @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day12\input.txt"

let presents, regions =
    let blob =
        File.ReadAllLines inputPath
        |> Seq.fold
            (fun blob line ->
                match blob, line with
                | [], line -> [ line + "\n" ]
                | bag :: blob, line when String.IsNullOrWhiteSpace line -> String.Empty :: bag :: blob
                | bag :: blob, line -> bag + line + "\n" :: blob)
            []

    let presents =
        blob
        |> List.tail
        |> List.map (fun bag ->
            let split = bag.Split(':')
            let code = int split[0]

            let gift =
                split[1].Split('\n', StringSplitOptions.RemoveEmptyEntries)
                |> Array.map _.ToCharArray()
                |> fun jagged -> Array2D.init jagged.Length jagged[0].Length (fun x y -> jagged[x][y])

            code, gift)

    let regions =
        blob
        |> List.head
        |> _.Split('\n', StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun line ->
            let split = line.Split ':'
            let size = split[0].Split 'x'

            let numbers =
                split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map int

            int size[0], int size[1], numbers)

    presents, regions

let regionsFitPresents =
    let fitRegion (width, height, presentsToFit) =
        let totPresent = presentsToFit |> Array.sum
        let regionSize = width * height

        regionSize >= totPresent * 9

    regions |> Seq.where fitRegion |> Seq.length

printfn "%A" regionsFitPresents // 505
