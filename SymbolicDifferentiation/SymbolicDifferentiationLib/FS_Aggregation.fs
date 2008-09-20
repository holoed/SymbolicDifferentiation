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
open FS_Parallel;

let rec private Create (exp, data:Dictionary<string, 'a>, map2) =
    let Process exp = Create(exp, data, map2)
    match exp with
    | Number n -> Seq.init_infinite(fun i -> n)
    | Variable x -> Seq.append (data.Item(x)) (Seq.init_infinite(fun i -> 0.0))
    | Add(x, y) -> map2(fun left right -> left + right) (Process x) (Process y)
    | Mul(x, y) -> map2(fun left right -> left * right) (Process x) (Process y)
    | Pow(x, n) -> map2(fun left right -> System.Math.Pow(left, right)) (Process x) (Seq.init_infinite(fun i -> n))
    
type Ret(map2, exp) =
    member x.Execute(data) = Create(exp, data, map2)

let Build = fun exp -> Ret(Seq.map2, (ToFs exp))

let BuildParallel procNum dataSize = fun exp -> Ret((FS_Parallel.pmap2 procNum dataSize), (ToFs exp))