module Hw6.Client.MaybeBuilder

open System.Threading.Tasks

type MaybeBuilder() =
    member builder.Bind(a, f): Result<'e,'d> =
        match a with
        | Ok a -> f a
        | Error message -> Error message 
    member builder.Return x : Result<'a, 'b> = Ok x
    member builder.ReturnFrom x : Result<'a, 'b> = x
let maybe = MaybeBuilder()