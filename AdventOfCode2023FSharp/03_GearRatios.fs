namespace AdventOfCode2023FSharp

module GearRatios =
    open System
    open System.IO

    let inputTest = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day3\input_test.txt")

    let input = File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day3\input.txt")

    let sumPartNumber(input: string) =

        let charAndIndexes = 
            let schematic = input.Split(Environment.NewLine)
            [for i in 0..schematic.Length - 1 do
                for j in 0..schematic[i].Length - 1 do
                    (schematic[i].[j], i, j)]

        let digitIndexes =
            charAndIndexes
            |> List.filter (fun (c, _, _) -> Char.IsDigit(c))
            |> List.map (fun (_, x, y) -> (x, y))

        let symbolsIndexes =      
            charAndIndexes
            |> List.filter (fun (c, _, _) -> not (Char.IsDigit(c) || c = '.') )
            |> List.map (fun (_, x, y) -> (x, y))

        let overlapping =

            let indicesAround ((x, y): int*int) = 
                [for i in -1..1 do
                    for j in -1..1 do
                        (x + i, y + j)] 
                |> List.except [(x, y)]

            let removeConsecutives (items: (int*int) list) =
                let arr = List.toArray items
                [for i in 0..arr.Length - 1 do
                    if i = 0 then arr[i] else
                    let (px, py) = arr[i-1]
                    let (nx, ny) = arr[i]
                    if px <> nx || py + 1 <> ny then arr[i]]
                                 
            symbolsIndexes
            |> List.collect indicesAround          
            |> List.filter (fun x -> digitIndexes |> List.contains x)
            |> List.distinct
            |> removeConsecutives

        let partNumberConnected = 

            let getNumberFromIndex ((x, y): int*int) =

                let head =
                    charAndIndexes
                    |> List.filter (fun (_, i, j) -> i = x && j >= y)
                    |> List.takeWhile (fun (c, _, _) -> Char.IsDigit(c))

                let tail = 
                    charAndIndexes
                    |> List.filter (fun (_, i, j) -> i = x && j < y)
                    |> List.rev
                    |> List.takeWhile (fun (c, _, _) -> Char.IsDigit(c))
                    |> List.rev

                tail @ head
                |> List.map (fun (c, _, _) -> c)
                |> List.toArray
                |> System.String
                |> int
         
            overlapping
            |> List.map getNumberFromIndex     

        partNumberConnected
        |> List.sum
