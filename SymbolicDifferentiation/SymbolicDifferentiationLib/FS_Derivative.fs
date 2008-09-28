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

open FS_Utils;
open FS_AbstractSyntaxTree;

let rec private Deriv expression =  
    match expression with
        | Variable _ -> Number 1.0
        | Number _   -> Number 0.0
        | Add(x,y)   -> (Deriv x) + (Deriv y)
        | Mul(x,y)   -> (x * Deriv(y)) + (Deriv(x) * y)
        | Pow(x,y)   -> Number (y) * Pow(x,(y - 1.0))
        | _ -> failwith "Not supported"
        
let Derivate x = 
    x |> ToFs |> Deriv |> ToCs
        







