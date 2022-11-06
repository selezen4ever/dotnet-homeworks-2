module Hw6.Client.App

open System
open System.Globalization
open System.Threading.Tasks
open Hw6.Client.CalculatorRequestClient
open Hw6.Client.TaskMaybeBuilder
open Hw6.Client.MaybeBuilder
open Microsoft.FSharp.Core

[<Literal>] 
let OpPlus = "Plus"

[<Literal>] 
let OpMinus = "Minus"

[<Literal>] 
let OpMultiply = "Multiply"

[<Literal>] 
let OpDivide = "Divide"

let client = new CalculatorRequestClient("http://localhost:5000")

let tryParseOperation (op: string) =
    match op with
    | "+" -> Ok OpPlus
    | "-" -> Ok OpMinus
    | "*" -> Ok OpMultiply
    | "/" -> Ok OpDivide
    | _ -> Error "Unknown operation"

let tryParseArg (strArg: string) = 
    match Decimal.TryParse (strArg.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture) with
    | true, decimal -> Ok decimal
    | _ -> Error $"Incorrect num {strArg}"

let readInputData ()=
    maybe {
        printf "Enter 1 num: "
        let! v1 = Console.ReadLine() |> tryParseArg
        printf "Specify operation: "
        let! op = Console.ReadLine() |> tryParseOperation
        printf "Enter 2 num: "
        let! v2 = Console.ReadLine() |> tryParseArg
        return {
            value1 = v1
            operation = op
            value2 = v2
        }
    }
    
let displayCalculationResult (result: Result<string, string>) =
    match result with
    | Ok result -> printfn $"Result: %s{result}"
    | Error error -> printfn $"Error: %s{error}"
    
let makeCalculation (calculation: MathExpression) =
    task{
        match! client.Calculate(calculation) with
        | Ok result -> return Ok result
        | Error error -> return Error error.message
    }
    
let rec waitForExitOrContinue () =
    printfn "\nEnter - continue\nEsc - exit\n"

    match Console.ReadKey().Key with
    | ConsoleKey.Escape -> false
    | ConsoleKey.Enter -> true
    | _ -> waitForExitOrContinue ()

[<EntryPoint>]
let main argv =
    let workWithUser () =
        let computation = taskMaybe {
            let! calculation = readInputData () |> Task.FromResult
            return! makeCalculation calculation
        }
        task {
            let! calculatorResult = computation
            displayCalculationResult calculatorResult
        }

    let rec loop () =
        workWithUser().Result |> ignore
        if waitForExitOrContinue() then
            loop()

    loop()
    0 // return an integer exit code
