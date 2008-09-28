// * **********************************************************************************************
// * Copyright (c) Edmondo Pentangelo. 
// *
// * This source code is subject to terms and conditions of the Microsoft Public License. 
// * A copy of the license can be found in the License.html file at the root of this distribution. 
// * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
// * Microsoft Public License.
// *
// * You must not remove this notice, or any other, from this software.
// * **********************************************************************************************

#light

open FS_AbstractSyntaxTree;
open FS_Utils;
open System.Collections.Generic;

//TODO: Remove duplication between parallel and sequential version
let rec private Create (exp, data:Dictionary<string, double seq>, functions:Dictionary<string, 'f>) =
    let Execute x = x |> Array.of_seq |> Seq.of_array
    let Parallel asyncs = (Seq.of_array((Async.Run(Async.Parallel(asyncs)))))
    let Process exp = Create(exp, data, functions)
    match exp with
    | Number n -> seq[n]
    | Variable x -> data.Item(x)
    | Add(x, y) ->       functions.Item("Add") (Parallel(seq[async { return Execute(Process x) }; async { return Execute(Process y) }]))
    | Mul(x, y) ->       functions.Item("Mul") (Parallel(seq[async { return Execute(Process x) }; async { return Execute(Process y) }]))
    | Pow(x, n) ->       functions.Item("Pow") (seq[(Process x);seq[n]])
    | Fun(name, args) -> functions.Item(name)  (Parallel(Seq.map (fun arg -> async { return Execute(Process arg) }) args))
    
type Ret(exp, functions) =
    member x.Execute(data) = Create(exp, data, functions)

let Build = fun (exp, functions) ->  Ret((ToFs exp), functions)