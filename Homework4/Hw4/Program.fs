open Hw4
open Hw4.Parser
open Calculator
[<EntryPoint>]
let main args = 
        args
        |> parseCalcArguments 
        |> fun x -> (calculate x.arg1 x.operation x.arg2) 
        |> printf "%f"
        0
        
