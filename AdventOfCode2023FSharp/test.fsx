let primeFactors (n: int64) = 

    let rec factorize (factors: int64 list) (n: int64) (p: int64) =
        if n = 1L then factors
        elif n < p * p then n :: factors
        elif n % p = 0L then factorize (p :: factors) (n / p) p
        else factorize factors n (p + 1L)

    let print (p, k) =
        if k = 1L then sprintf "(%d)" p 
        else sprintf "(%d**%d)" p k

    factorize [] n 2L
    |> List.groupBy (fun f -> f)
    |> List.map (fun (k, g) -> (k, int64 g.Length))
    |> List.sortBy fst
    |> List.map print
    |> String.concat ""

printfn "%s" (primeFactors 6814720L)