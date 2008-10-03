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

type Expression<'a> = 
    | Variable of string
    | Number of 'a
    | Add of Expression<'a> * Expression<'a>
    | Sub of Expression<'a> * Expression<'a>
    | Mul of Expression<'a> * Expression<'a>
    | Div of Expression<'a> * Expression<'a>
    | Pow of Expression<'a> * Expression<'a>
    | FunApp of string * Expression<'a> seq
    | FunDecl of string * Expression<'a>
    static member (+) (x:Expression<'a>, y:Expression<'a>) = Add(x,y)
    static member (-) (x:Expression<'a>, y:Expression<'a>) = Sub(x,y)
    static member (*) (x:Expression<'a>, y:Expression<'a>) = Mul(x,y)
    static member (/) (x:Expression<'a>, y:Expression<'a>) = Div(x,y)


