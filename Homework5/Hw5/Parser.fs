module Hw5.Parser

open System
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]): Result<'a,'b> =
    match Array.length args with
    | 3 -> Ok args
    | _ -> Error WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | Calculator.plus -> Ok(arg1, CalculatorOperation.Plus, arg2)
    | Calculator.minus -> Ok(arg1, CalculatorOperation.Minus, arg2)
    | Calculator.multiply -> Ok(arg1, CalculatorOperation.Multiply, arg2)
    | Calculator.divide -> Ok(arg1, CalculatorOperation.Divide, arg2)
    | _ ->  Error WrongArgFormatOperation

    
let parseArgs (args: string[]): Result<('a * string * 'b), Message> =
     try
         Ok(args[0] |> decimal, args[1], args[2] |> decimal)
         with
            | _ -> Error WrongArgFormat
        

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match (operation, arg2) with
    | (CalculatorOperation.Divide, 0.0m) -> Error DivideByZero
    | _ -> Ok(arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe {
        let! supportedArgsLength = isArgLengthSupported args
        let! parsedArgs = parseArgs supportedArgsLength
        let! parsedArgsWithOperation = isOperationSupported parsedArgs
        let! nonZeroDenominatorArgs = isDividingByZero parsedArgsWithOperation
        return nonZeroDenominatorArgs
    }
    
    