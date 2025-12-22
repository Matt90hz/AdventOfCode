open System
open System.IO

//let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day06\input_test.txt"
let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day06\input.txt"

let grandTotal =
    File.ReadAllLines(path)
    |> Seq.map (fun line -> line.Split(' ', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries))
    |> Seq.collect Seq.indexed
    |> Seq.groupBy (fun (i, _) -> i)
    |> Seq.map (fun (_, x) -> Seq.map (fun (_, x) -> x) x)
    |> Seq.map (fun x ->
        if Seq.last x = "+" then
            x |> Seq.rev |> Seq.tail |> Seq.sumBy int64
        else
            x |> Seq.rev |> Seq.tail |> Seq.map int64 |> Seq.reduce (*))
    |> Seq.sum // 4277556 6172481852142

let grandTotalCephalopodsMath =
    let lines = File.ReadAllLines(path)
    let opRow = Array.length lines - 1

    let number col =
        lines[.. opRow - 1]
        |> Seq.map (fun line -> line[col])
        |> Seq.fold (fun n c -> $"{n}{c}") ""
        |> int64

    Seq.foldBack
        (fun i (tot, num) ->
            if lines |> Seq.forall (fun line -> line[i] = ' ') then
                (tot, num)
            else
                let n = number i :: num

                match lines[opRow][i] with
                | '+' -> tot + Seq.sum n, []
                | '*' -> tot + Seq.reduce (*) n, []
                | _ -> tot, n)
        (seq { 0 .. String.length lines[0] - 1 })
        (0L, []) // 3263827 10188206723429
