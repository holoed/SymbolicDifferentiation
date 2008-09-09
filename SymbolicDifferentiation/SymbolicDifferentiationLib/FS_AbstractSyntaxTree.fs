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

type Expression = 
    | Variable of string
    | Number of double
    | Add of Expression * Expression
    | Mul of Expression * Expression
    | Pow of Expression * double
    static member (+) (x, y) = Add(x,y)
    static member (*) (x, y) = Mul(x,y)

