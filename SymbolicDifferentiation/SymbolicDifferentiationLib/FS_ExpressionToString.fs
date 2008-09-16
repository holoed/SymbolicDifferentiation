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

let rec ToString exp = 
    match exp with
    | Number n -> n.ToString()
    | Variable x -> x
    | Mul(a, Add(b,c)) -> sprintf "%s*(%s)" (ToString a) (ToString (Add(b,c)))
    | Mul(Add(a,b), c) -> sprintf "(%s)*%s" (ToString (Add(a,b))) (ToString c)
    | Pow(Add(a,b), n) -> sprintf "(%s)^%.0f" (ToString (Add(a,b))) n
    | Add(a, b) -> sprintf "%s+%s" (ToString a) (ToString b)
    | Mul(a, b) -> sprintf "%s*%s" (ToString a) (ToString b)
    | Pow(a, n) -> sprintf "%s^%.0f" (ToString a) n 

