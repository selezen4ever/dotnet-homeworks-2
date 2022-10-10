module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

    
let isArgLengthSupported (args : string[]) =
    match Array.length args with
    | 3 -> true
    | _ -> false
    
let parseNum(arg:string) =
    match Double.TryParse arg with
    | true, float -> float
    | _ -> ArgumentException() |> raise
    
let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> ArgumentException() |> raise
    
    
let parseCalcArguments(args : string[]) =
    match isArgLengthSupported args with
    | false -> ArgumentException() |> raise
    | true ->
         {
            arg1 = parseNum args.[0];
            operation = parseOperation args.[1];
            arg2 = parseNum args.[2];
         }

        
         
        
