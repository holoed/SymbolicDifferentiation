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

let rec private Create (exp, data:Dictionary<string, double array>, map2) =
    let Process exp = Create(exp, data, map2)
    match exp with
    | Number n -> Array.of_list [n]
    | Variable x -> data.Item(x)
    
    | Add(x, Number n) -> Array.map(fun left -> left + n) (Process x)
    | Add(Number n, y) -> Array.map(fun right -> n + right) (Process y)
    | Add(x, y) -> map2(fun left right -> left + right) (Process x) (Process y)

    | Mul(x, Number n) -> Array.map(fun left -> left * n) (Process x)
    | Mul(Number n, y) -> Array.map(fun right -> n * right) (Process y)
    | Mul(x, y) -> map2(fun left right -> left * right) (Process x) (Process y)

    | Pow(x, n) -> Array.map(fun left -> System.Math.Pow(left, n)) (Process x)
    
type Ret(map2, exp) =
    member x.Execute(data) = Create(exp, data, map2)

let Build = fun exp -> Ret(Array.map2, (ToFs exp))

let BuildParallel procNum dataSize = fun exp -> Ret((FS_Parallel.pmap2 procNum dataSize), (ToFs exp))