module Hw6.MaybeBuilder

type MaybeBuilder() =
    member builder.Bind(a, f): Result<'e,'d> =
        match a with
        | Ok a -> f a
        | Error message -> Error message 
    member builder.Return x: Result<'a,'b> = x
let maybe = MaybeBuilder()