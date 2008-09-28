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

let rec private SimplifyImpl exp = 
    match exp with
    | Number a -> Number a
    | Variable a -> Variable a
    | Add(a, b) ->
        match SimplifyImpl a, SimplifyImpl b with
        | x, Number 0.0 -> x
        | Number 0.0, y -> y
        | Number n1, Number n2 -> Number(n1 + n2)
        | a, b -> Add(a,b)
    | Mul(a, b) -> 
        match SimplifyImpl a, SimplifyImpl b with
        | _, Number 0.0 -> Number 0.0
        | Number 0.0, _ -> Number 0.0
        | a, Number 1.0 -> a
        | Number 1.0, b -> b
        | Number n1, Number n2 -> Number(n1 * n2)
        | Number n1, Mul(Number n2, c) -> Mul(Number(n1 * n2), c)
        | a, b -> Mul(a,b)
    | Pow(a, b) ->
        match SimplifyImpl a, b with
        | Number n1, 1.0 -> Number n1
        | a, 1.0 -> a
        | a, b -> Pow(a,b)
    | exp -> exp

    
let Simplify exp = exp |> ToFs |> SimplifyImpl |> ToCs