module Hw6.Client.TaskMaybeBuilder

open System.Threading.Tasks

type TaskMaybeBuilder() =
    member builder.Bind(a, f): Task<Result<'e,'d>> =
        task {
            match! a with
            | Ok a -> return! f a
            | Error message -> return Error message 
        }
    member builder.Return x: Task<Result<'e,'d>> = task { return Ok x }
    member builder.ReturnFrom x : Task<Result<'e, 'd>> = x
    member builder.Zero() : Task<Result<unit, 'd>> = task { return Ok () }
let taskMaybe = TaskMaybeBuilder()