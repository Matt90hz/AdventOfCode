namespace AdventOfCode2023FSharp

module Trebuchet =

    open System
    open System.IO

    let trebuchetCoordinates path = 
        File.ReadAllLines(path)
        |> Seq.map (fun line -> int $"{Seq.find Char.IsDigit line}{Seq.findBack Char.IsDigit line}")
        |> Seq.sum

    let trebuchetCoordinates3 path = 
        let mutable sum  = 0
        for line in File.ReadAllLines(path) do
            sum <- sum + int $"{Seq.find Char.IsDigit line}{Seq.findBack Char.IsDigit line}"
        sum
        

    let firstDigit (line: string) = line |> Seq.find Char.IsDigit

    let lastDigit (line: string) = line |> Seq.findBack Char.IsDigit

    let trebuchetCoordinate (line: string) = int $"{firstDigit line}{lastDigit line}"

    let getLines (input:  string) = input.Split(Environment.NewLine)

    let trebuchetCoordinates2 (input: string) = 
        input 
        |> getLines 
        |> Seq.map trebuchetCoordinate