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

open NUnit.Framework;
open SymbolicDifferentiation.Extensions;
open SymbolicDifferentiation.Tests;
open FS_Utils;

let parse input = input |> FS_Tokenizer.Tokenize |> FS_Parser.Execute

[<TestFixture>] 
type myAppTests =  
    new() = {} 
    
    [<Test>] 
    member t.Add() = ExpressionAssert.AreEqual( parse "2 + 3", FromQuoteToCs <@ fun () -> 2 + 3 @> )
    
    [<Test>] 
    member t.AddAdd() = ExpressionAssert.AreEqual( parse "2 + 3 + 4", FromQuoteToCs <@ fun () -> 2 + 3 + 4 @> )

    [<Test>] 
    member t.Mul() = ExpressionAssert.AreEqual( parse "2 * 3", FromQuoteToCs <@ fun () -> 2 * 3 @> )   

    [<Test>] 
    member t.MulMul() = ExpressionAssert.AreEqual( parse "2 * 3 * 4", FromQuoteToCs <@ fun () -> 2 * 3 * 4 @> ) 
    
    [<Test>] 
    member t.MulAdd() = ExpressionAssert.AreEqual( parse "5 * (2 + 3)", FromQuoteToCs <@ fun () -> 5 * (2 + 3) @> )    
    
    [<Test>] 
    member t.AddMul() = ExpressionAssert.AreEqual( parse "5 + 2 * 3", FromQuoteToCs <@ fun () -> 5 + 2 * 3 @> ) 
    
//    [<Test>] 
//    member t.AddMul2() = ExpressionAssert.AreEqual( parse "A + B", FromQuoteToCs <@ fun (A,B) -> A + B @> )    