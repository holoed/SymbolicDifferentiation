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

let rec private Create (exp, data:Dictionary<string, double seq>, functions:Dictionary<string, 'f>) =
    let Process exp = Create(exp, data, functions)
    match exp with
    | Number n -> Seq.init_infinite (fun i -> n)
    | Variable x -> data.Item(x)
    | Add(x, y) -> functions.Item("Add")(Process x) (Process y)
    | Mul(x, y) -> functions.Item("Mul") (Process x) (Process y)
    | Pow(x, n) -> functions.Item("Pow") (Process x) (Seq.init_infinite (fun i -> n))
    
type Ret(exp, functions) =
    member x.Execute(data) = Create(exp, data, functions)

let Build = fun (exp, functions) ->  Ret((ToFs exp), functions)