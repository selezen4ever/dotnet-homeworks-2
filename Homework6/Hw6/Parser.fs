module Hw6.Parser

open Calculator

let inline parseOperation operation =
    match operation with
    | Calculator.plus -> Ok(CalculatorOperation.Plus)
    | Calculator.minus -> Ok(CalculatorOperation.Minus)
    | Calculator.multiply -> Ok(CalculatorOperation.Multiply)
    | Calculator.divide -> Ok(CalculatorOperation.Divide)
    | _ ->  Error $"Could not parse value '{operation}'"
