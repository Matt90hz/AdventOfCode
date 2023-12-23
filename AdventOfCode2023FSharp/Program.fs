
open Trebuchet
open System.IO

let input = File.ReadAllText "D:\VisualStudio\AdventOfCode\AdventOfCode2023\Dayz15\input_test1.txt"

printf "%A" (LensLibrary.hashSum input)
