open System

open Hw5.MaybeBuilder
open Hw5.Calculator
open Hw5.Parser

[<EntryPoint>]
let main args =
    let res = maybe {
        let! (arg1, op, arg2) = args |> parseCalcArguments
        return calculate arg1 op arg2
    }
    match res with
    | Ok res -> printfn $"{res}" 
    | Error message -> printfn $"{message}"
    0
        