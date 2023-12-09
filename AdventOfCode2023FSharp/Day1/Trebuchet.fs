
module Trebuchet

open System.Linq
open System


let isDigit (c: char) = Char.IsDigit(c) 

let firstDigit (line: string) = line.First(isDigit)

let lastDigit (line: string) = line.Last(isDigit)

let trebuchetCoordinate (line: string) = int $"{firstDigit line}{lastDigit line}"

let lines (input: string) = input.Split(Environment.NewLine)

type String with member x.Lines() = lines x

let trebuchetCoordinates(input: string) = input.Lines().Select(trebuchetCoordinate)
    
let result = (trebuchetCoordinates "").Sum() 
