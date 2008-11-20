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

type Operator = Add | Sub | Mul | Div | Pow | GreaterThan | LessThan 

type Expression<'a> = 
    | Variable of string
    | Number of 'a
    | Binary of Operator * Expression<'a> * Expression<'a>
    | FunApp of string * Expression<'a> seq
    | FunDecl of string * Expression<'a> seq * Expression<'a>
    | Cond of Expression<'a> * Expression<'a> * Expression<'a>
    static member (+) (x:Expression<'a>, y:Expression<'a>) = Binary(Add, x,y)
    static member (-) (x:Expression<'a>, y:Expression<'a>) = Binary(Sub, x,y)
    static member (*) (x:Expression<'a>, y:Expression<'a>) = Binary(Mul, x,y)
    static member (/) (x:Expression<'a>, y:Expression<'a>) = Binary(Div, x,y)


