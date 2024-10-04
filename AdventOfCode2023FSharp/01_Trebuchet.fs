namespace AdventOfCode2023FSharp

module Trebuchet =

    open System

    let firstDigit (line: string) = line |> Seq.find Char.IsDigit

    let lastDigit (line: string) = line |> Seq.findBack Char.IsDigit

    let trebuchetCoordinate (line: string) = int $"{firstDigit line}{lastDigit line}"

    let getLines (input: string) = input.Split(Environment.NewLine)

    let trebuchetCoordinates (input: string) = 
        input 
        |> getLines 
        |> Seq.map trebuchetCoordinate