<p><strong>Project Description</strong><br />Simple C#/F# library for Symbolic Differentiation and whatever else comes to mind ;)<br /><br />This project is about building a library of useful code and at the same time have fun with the latest C# 3.0 / F# CTP features :)<br /><br />And now also to explore the interoperability between C# and F# :))<br /><br />At the moment we have a simple implementation of the following:<br /><br />C# and F#:<br />- Tokenizer<br />- Combinator Parsers inspired by Brian's brillian Inside F# blog<br />- AST<br />- Symbolic Differentiation Visitor<br />- Simplifier Visitor<br />- Functions support<br />- Conditional expression support (new)<br /><br />F# only:<br />- Aggregation of sequences based on Expressions ;)<br />- Parallel computation of expressions based on depth of AST (Example: Expression like: <span class="codeInline"> (A+B) * (D+E) </span> uses 2 cores and <span class="codeInline"> ( (A +B) * (D+E) ) + ( (A +B) * (D+E) ) </span> uses 4 cores :))))<br />- ToString Visitor <br /><br />Requirements:<br />- Any code contributed to this project be fully TDDed.<br />- No code generation code is allowed.<br /><br />What's next: <br />- Expand the functionality of each component to cover as many cases as possible and improve the quality of the code and testing along the way.<br /><br />Recommended Readings<br /><a href="http://www.amazon.co.uk/gp/product/0262510871?ie=UTF8&amp;tag=httpfsharpcbl-21&amp;linkCode=as2&amp;camp=1634&amp;creative=6738&amp;creativeASIN=0262510871">Structure and Interpretation of Computer Programs</a><br /><a href="http://www.amazon.co.uk/gp/product/1590598504?ie=UTF8&amp;tag=httpfsharpcbl-21&amp;linkCode=as2&amp;camp=1634&amp;creative=6738&amp;creativeASIN=1590598504">Expert F#</a><br /><br />References:<br /><a href="http://fsharpcode.blogspot.com">http://fsharpcode.blogspot.com</a><br /><a href="http://en.wikipedia.org/wiki/Differentiation_rules">http://en.wikipedia.org/wiki/Differentiation_rules</a><br /><a href="http://lorgonblog.spaces.live.com/blog/cns!701679AD17B6D310!133.entry?_c=BlogPart">http://lorgonblog.spaces.live.com/blog/cns!701679AD17B6D310!133.entry?_c=BlogPart</a><br /><a href="http://blogs.msdn.com/lukeh/archive/2007/08/19/monadic-parser-combinators-using-c-3-0.aspx">http://blogs.msdn.com/lukeh/archive/2007/08/19/monadic-parser-combinators-using-c-3-0.aspx</a><br /><a href="http://www.cs.nott.ac.uk/~gmh/pearl.pdf">http://www.cs.nott.ac.uk/~gmh/pearl.pdf</a><br /><a href="http://leibnizdream.wordpress.com/2007/12/22/first-class-functions2-multiprogramming/">http://leibnizdream.wordpress.com/2007/12/22/first-class-functions2-multiprogramming/</a><br /><br />Parallel versus Sequential Computation Performance:<br /><br />Expression: <span class="codeInline"> (((A + B) * (A + B)) * ((A + B) * (A + B))) + (((A + B) * (A + B)) * ((A + B) * (A + B))) </span><br />Data Size: <span class="codeInline"> A: 1..1000000 B: 1..1000000 </span><br /><br /><span class="codeInline"> Intel Xeon CPU X5450 QUAD-CORE @ 3.00Ghz X2 (8 cores) 3.25 GB of Ram </span><br /><br />Sequential elapsed: <span class="codeInline"> 00:06:28.4842822 </span><br />Parallel elapsed: <span class="codeInline"> 00:01:48.6659580 </span><br /><br />Operations were made more intensive using the following code:<br /><span class="codeInline"> (+) : for (int i = 0; i &lt; 10000; i++) { z = x + y; } </span><br /><span class="codeInline"> (*) : for (int i = 0; i &lt; 10000; i++) { z = x * y; } </span><br /><br />Performance comparison between C# and F# Tokenizer: <br />(Size of the test data: 100.000 Letters separated by spaces)<br /><br />C#: 00:09:39.7925956 (hh:mm:ss)<br /><br />F#: 00:09:42.6639161 (hh:mm:ss)<br /><br />Enjoy ;)<br /><br />Edmondo Pentangelo</p>
