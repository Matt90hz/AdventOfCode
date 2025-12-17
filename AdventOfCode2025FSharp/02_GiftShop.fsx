open System.IO

//let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day02\input_test.txt"
let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day02\input.txt"

let ids =
    File.ReadAllText(path)
    |> fun text -> text.Split(',')
    |> Seq.collect (fun range ->
        let ids = range.Split('-')
        seq { int64 ids[0] .. int64 ids[1] })

let isInvalidId id =
    let str = string id
    let mid = String.length str / 2

    str[.. (mid - 1)] = str[mid..]

let sumInvalidIds = ids |> Seq.filter isInvalidId |> Seq.sum //1227775554 18700015741

let isMoreInvalidId id =
    let str = string id
    let len = String.length str

    seq { 1 .. len / 2 }
    |> Seq.exists (fun size ->
        str
        |> Seq.chunkBySize size
        |> Seq.pairwise
        |> Seq.forall (fun (x, y) -> Seq.length x = Seq.length y && Seq.forall2 (=) x y))

let sumInvalidMoreIds = ids |> Seq.filter isMoreInvalidId |> Seq.sum // 4174379265 20077272987
