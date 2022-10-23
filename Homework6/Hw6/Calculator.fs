module Hw6.Calculator

type CalculatorOperation =
     | Plus = 0
     | Minus = 1
     | Multiply = 2
     | Divide = 3

[<Literal>] 
let OpPlus = "Plus"

[<Literal>] 
let OpMinus = "Minus"

[<Literal>] 
let OpMultiply = "Multiply"

[<Literal>] 
let OpDivide = "Divide"

let parseOperation operation =
    match operation with
    | OpPlus -> Ok(CalculatorOperation.Plus)
    | OpMinus -> Ok(CalculatorOperation.Minus)
    | OpMultiply -> Ok(CalculatorOperation.Multiply)
    | OpDivide -> Ok(CalculatorOperation.Divide)
    | _ ->  Error $"Could not parse value '{operation}'"

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline calculate (value1: ^a) operation (value2: ^a)  =
    match operation with
    | CalculatorOperation.Plus -> Ok(value1 + value2)
    | CalculatorOperation.Minus -> Ok(value1 - value2)
    | CalculatorOperation.Multiply -> Ok(value1 * value2)
    | _ ->
            match decimal value2 with
            | 0.0m -> Error "DivideByZero"
            | _ -> Ok(value1 / value2)