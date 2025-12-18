open System.IO

//let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day03\input_test.txt"
let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day03\input.txt"

let banks = File.ReadAllLines(path)

let rec joltage (bank: string) d =
    if d = 1 then
        bank |> Seq.maxBy int |> (fun x -> $"{x}")
    else
        let l = Seq.length bank - d
        let i, x = bank[..l] |> Seq.indexed |> Seq.maxBy (fun (_, x) -> int x)
        let y = joltage bank[i + 1 ..] (d - 1)

        $"{x}" + y

let totalJoltage = banks |> Seq.map (fun bank -> joltage bank 2) |> Seq.sumBy int // 357 17403

let totalLargeJoltage =
    banks |> Seq.map (fun bank -> joltage bank 12) |> Seq.sumBy int64 //3121910778619 173416889848394
