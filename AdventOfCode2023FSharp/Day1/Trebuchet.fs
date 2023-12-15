
module Trebuchet

open System


let isDigit (c: char) = Char.IsDigit(c) 

let firstDigit (line: string) = line |> Seq.find isDigit

let lastDigit (line: string) = line |> Seq.findBack isDigit

let trebuchetCoordinate (line: string) = int $"{firstDigit line}{lastDigit line}"

let getLines (input: string) = input.Split(Environment.NewLine)

let trebuchetCoordinates(input: string) = 
    input 
    |> getLines 
    |> Seq.map trebuchetCoordinate
