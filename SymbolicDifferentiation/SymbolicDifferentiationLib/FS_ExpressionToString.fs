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
    let ToStringArgs args = 
        if (Seq.length args > 0) 
            then sprintf "(%s)" (String.concat "," (Seq.map (fun arg -> ToString arg) args))
            else sprintf ""
    match exp with
    | Number n when n < 0.0 -> sprintf "(%.0f)" n
    | Number n -> sprintf "%.0f" n
    | Variable x -> x
    | Binary(Mul, a, Binary(Add,b,c)) -> sprintf "%s*(%s)" (ToString a) (ToString (Binary(Add,b,c)))
    | Binary(Mul, Binary(Add, a,b), c) -> sprintf "(%s)*%s" (ToString (Binary(Add,a,b))) (ToString c)
    | Binary(Pow, Binary(Add, a,b), op) -> match op with
                                            | Binary(Add,x,y) -> sprintf "(%s)^(%s)" (ToString (Binary(Add,a,b))) (ToString (Binary(Add,x,y))) 
                                            | Binary(Sub,x,y) -> sprintf "(%s)^(%s)" (ToString (Binary(Add,a,b))) (ToString (Binary(Sub,x,y))) 
                                            | Binary(Mul,x,y) -> sprintf "(%s)^(%s)" (ToString (Binary(Add,a,b))) (ToString (Binary(Mul,x,y))) 
                                            | n -> sprintf "(%s)^%s" (ToString (Binary(Add,a,b))) (ToString n)
    | Binary(Add,a, b) -> sprintf "%s+%s" (ToString a) (ToString b)
    | Binary(Sub,a, op) -> match op with 
                            | Binary(Add,x,y) -> sprintf "%s-(%s)" (ToString a) (ToString (Binary(Add,x,y)))
                            | Binary(Mul,x,y) -> sprintf "%s-(%s)" (ToString a) (ToString (Binary(Mul,x,y)))
                            | Binary(Div,x,y) -> sprintf "%s-(%s)" (ToString a) (ToString (Binary(Div,x,y)))
                            | b -> sprintf "%s-%s" (ToString a) (ToString b)
    | Binary(Mul,a, b) -> sprintf "%s*%s" (ToString a) (ToString b)
    | Binary(Div,a, b) -> sprintf "(%s)/%s" (ToString a) (ToString b)
    | Binary(Pow,a, op) -> match op with
                            | Binary(Add,x,y) -> sprintf "%s^(%s)" (ToString a) (ToString (Binary(Add,x,y))) 
                            | Binary(Sub,x,y) -> sprintf "%s^(%s)" (ToString a) (ToString (Binary(Sub,x,y))) 
                            | Binary(Mul,x,y) -> sprintf "%s^(%s)" (ToString a) (ToString (Binary(Mul,x,y))) 
                            | n -> sprintf "%s^%s" (ToString a) (ToString n) 
    | FunApp(name, args) -> sprintf "%s%s" name (ToStringArgs args)
    | FunDecl(name, args, body) -> sprintf "%s%s=%s" name (ToStringArgs args) (ToString body)