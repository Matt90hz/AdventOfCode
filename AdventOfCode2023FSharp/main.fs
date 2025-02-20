namespace AdventOfCode2023FSharp

module main =

    open Trebuchet
    open System.IO

    printfn "%d" (trebuchetCoordinates @"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day1\Input.txt")
    printfn "%d" (trebuchetCoordinates3 @"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day1\Input.txt")
    printfn "%d" (File.ReadAllText(@"D:\VisualStudio\AdventOfCode\AdventOfCode2023\Day1\Input.txt") |> trebuchetCoordinates2 |> Seq.sum)
                                      