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

let add (x: double seq, y: double seq) = Seq.map2 (fun x y -> x + y) x y 

let mul (x: double seq, y: double seq) = Seq.map2 (fun x y -> x * y) x y 

let pow (x: double seq, y: double seq) = Seq.map2 (fun x y -> System.Math.Pow(x,y)) x y

let max (x: double seq, y: double seq) = Seq.map2 (fun x y -> if (x > y) then x else y) x y 
