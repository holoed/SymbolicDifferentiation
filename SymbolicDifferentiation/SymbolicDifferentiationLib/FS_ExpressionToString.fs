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
    | Number n when n < 0.0 -> sprintf "(%.0f)" n
    | Number n -> sprintf "%.0f" n
    | Variable x -> x
    | Mul(a, Add(b,c)) -> sprintf "%s*(%s)" (ToString a) (ToString (Add(b,c)))
    | Mul(Add(a,b), c) -> sprintf "(%s)*%s" (ToString (Add(a,b))) (ToString c)
    | Pow(Add(a,b), Add(x,y)) -> sprintf "(%s)^(%s)" (ToString (Add(a,b))) (ToString (Add(x,y))) 
    | Pow(Add(a,b), Sub(x,y)) -> sprintf "(%s)^(%s)" (ToString (Add(a,b))) (ToString (Sub(x,y))) 
    | Pow(Add(a,b), Mul(x,y)) -> sprintf "(%s)^(%s)" (ToString (Add(a,b))) (ToString (Mul(x,y))) 
    | Pow(Add(a,b), n) -> sprintf "(%s)^%s" (ToString (Add(a,b))) (ToString n)
    | Add(a, b) -> sprintf "%s+%s" (ToString a) (ToString b)
    | Sub(a, Add(x,y)) -> sprintf "%s-(%s)" (ToString a) (ToString (Add(x,y)))
    | Sub(a, Mul(x,y)) -> sprintf "%s-(%s)" (ToString a) (ToString (Mul(x,y)))
    | Sub(a, Div(x,y)) -> sprintf "%s-(%s)" (ToString a) (ToString (Div(x,y)))
    | Sub(a, b) -> sprintf "%s-%s" (ToString a) (ToString b)
    | Mul(a, b) -> sprintf "%s*%s" (ToString a) (ToString b)
    | Div(a, b) -> sprintf "(%s)/%s" (ToString a) (ToString b)
    | Pow(a, Add(x,y)) -> sprintf "%s^(%s)" (ToString a) (ToString (Add(x,y))) 
    | Pow(a, Sub(x,y)) -> sprintf "%s^(%s)" (ToString a) (ToString (Sub(x,y))) 
    | Pow(a, Mul(x,y)) -> sprintf "%s^(%s)" (ToString a) (ToString (Mul(x,y))) 
    | Pow(a, n) -> sprintf "%s^%s" (ToString a) (ToString n) 
    | FunApp(name, args) -> sprintf "%s(%s)" name (String.concat "," (Seq.map (fun arg -> ToString arg) args))
    | FunDecl(name, body) -> sprintf "%s=%s" name (ToString body)