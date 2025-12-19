open System.IO

//let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day04\input_test.txt"
let path = @"D:\VisualStudio\AdventOfCode\AdventOfCode2025\Day04\input.txt"

let department =
    let text = File.ReadAllLines(path)
    Array2D.init text.Length text[0].Length (fun x y -> text[x][y])

module Array2D =

    let toSeq (arr: 'T[,]) = seq { for x in arr -> x }

    let count item (arr: 'T[,]) =
        arr |> toSeq |> Seq.filter (fun x -> x = item) |> Seq.length

    let adjacents x y (arr: 'T[,]) =
        let sx, ex =
            match x with
            | 0 -> 0, x + 1
            | x when x = Array2D.length1 arr -> x - 1, x
            | _ -> x - 1, x + 1

        let sy, ey =
            match y with
            | 0 -> 0, y + 1
            | y when y = Array2D.length2 arr -> y - 1, y
            | _ -> y - 1, y + 1

        arr[sx..ex, sy..ey]

let markMovableRolls x y (dep: char[,]) =
    if dep[x, y] = '@' && dep |> Array2D.adjacents x y |> Array2D.count '@' <= 4 then
        'x'
    else
        dep[x, y]

let accessible =
    department
    |> Array2D.mapi (fun x y _ -> markMovableRolls x y department)
    |> Array2D.count 'x' //13 1437

let removableRolls =
    let rec remove dep =
        let toRemove = Array2D.mapi (fun x y _ -> markMovableRolls x y dep) dep

        if toRemove |> (fun a -> seq { for b in a -> b }) |> Seq.contains 'x' then
            let removed = Array2D.map (fun x -> if x = 'x' then '.' else x) toRemove
            remove removed
        else
            Array2D.map (fun x -> if x = 'x' then '.' else x) toRemove

    Array2D.count '@' department - Array2D.count '@' (remove department) //43 8765