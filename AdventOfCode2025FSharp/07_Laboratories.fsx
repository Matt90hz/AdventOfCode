open System.IO

//let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day07\input_test.txt"
let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day07\input.txt"

let timesBeamsSplit =
    let diagramFile = File.ReadAllLines(path)

    let diagram =
        Array2D.init diagramFile.Length diagramFile[0].Length (fun x y -> diagramFile[x][y])

    Seq.fold2
        (fun split curr prev ->
            diagram[prev, *]
            |> Seq.mapi (fun i x ->
                match x with
                | 'S' ->
                    diagram[curr, i] <- '|'
                    0
                | '|' ->
                    if diagram[curr, i] = '^' then
                        diagram[curr, i - 1] <- '|'
                        diagram[curr, i + 1] <- '|'
                        1
                    else
                        diagram[curr, i] <- '|'
                        0
                | _ -> 0)
            |> (fun s -> split + Seq.sum s))
        0
        (seq { 1 .. Array2D.length1 diagram - 1 })
        (seq { 0 .. Array2D.length1 diagram - 1 }) //21 1590

let timeLines =
    let diagramFile = File.ReadAllLines(path)

    let diagram =
        Array2D.init diagramFile.Length diagramFile[0].Length (fun x y ->
            match diagramFile[x][y] with
            | 'S' -> 1L
            | '^' -> -1L
            | _ -> 0L)

    Seq.iter2
        (fun curr prev ->
            diagram[prev, *]
            |> Seq.iteri (fun i x ->
                match x, diagram[curr, i] with
                | p, c when p > 0 && c < 0 ->
                    diagram[curr, i - 1] <- diagram[curr, i - 1] + p
                    diagram[curr, i + 1] <- diagram[curr, i + 1] + p
                | p, c when p > 0 -> diagram[curr, i] <- p + c
                | _ -> ()))
        (seq { 1 .. Array2D.length1 diagram - 1 })
        (seq { 0 .. Array2D.length1 diagram - 1 })

    Seq.sum diagram[Array2D.length1 diagram - 1, *] // 40 20571740188555