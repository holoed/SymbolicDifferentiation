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
    | Mul(a, Add(b,c)) -> ToString a + "*" + "(" + ToString b + "+" + ToString c + ")"
    | Mul(Add(a,b), c) -> "(" + ToString a + "+" + ToString b + ")" + "*" + ToString c
    | Pow(Add(a,b), n) -> "(" + ToString a + "+" + ToString b + ")" + "^" + n.ToString()
    | Add(a, b) -> ToString a + "+" + ToString b
    | Mul(a, b) -> ToString a + "*" + ToString b
    | Pow(a, n) -> ToString a + "^" + n.ToString() 

