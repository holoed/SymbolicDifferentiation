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

let private CalcChunkStartStopIndex size procNum index = 
    let chunkSize = size / procNum
    let reminder = size % procNum
    index * chunkSize, 
    if index < procNum - 1 then 
        (index + 1) * chunkSize - 1
    else
        size
        
let private Chunks size procNum =
    let Calc = CalcChunkStartStopIndex size procNum
    let rec Chunks'(i, list) = 
        if i < procNum then
            Chunks'(i + 1, list @ [Calc(i)])
        else
            list
    Chunks'(0, [])
    
let private ProcessChunk(f, x: 'a array, y: 'b array) chunk = 
    let ProcessChunk' (startIndex, endIndex) =
        let rec ProcessChunk'' (i, list) =
            if i < endIndex then
                ProcessChunk'' (i + 1, list @ [f x.[i] y.[i] ])
            else
               if i < x.Length then 
                    list @ [f x.[i] y.[i]]
               else
                    list
        ProcessChunk''(startIndex, [])
    ProcessChunk' chunk
           
    
let pmap2 n size f (x: 'a seq) (y: 'b seq) =     
    let pmap2Array n f (x: 'a array) (y: 'b array) = 
        let chunks = Chunks size n
        let results = List.map (ProcessChunk(f, x, y)) chunks
        Seq.of_list (List.concat results)
    pmap2Array n f (Array.of_seq(Seq.take size x)) (Array.of_seq (Seq.take size y))
    