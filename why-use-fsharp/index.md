---
layout: page
title: "Why use F#?"
description: "Why you should consider using F# for your next project"
nav: why-use-fsharp
hasIcons: 1
image: "/assets/img/four-concepts2.png"
---

Although F# is great for specialist areas such as scientific or data analysis, it is also an excellent choice for enterprise development. 
Here are five good reasons why you should consider using F# for  your next project. 

## ![](../assets/img/glyphicons/glyphicons_030_pencil.png) Conciseness

F# is not cluttered up with [coding "noise"](../posts/fvsc-sum-of-squares/)</a> such as curly brackets, semicolons and so on. 

You almost never have to specify the type of an object, thanks to a powerful [type inference system](../posts/conciseness-type-inference/).

And, compared with C#, it generally takes [fewer lines of code](../posts/fvsc-download/) to solve the same problem.


```
// one-liners
[1..100] |> List.sum |> printfn "sum=%d"

// no curly braces, semicolons or parentheses
let square x = x * x
let sq = square 42 

// simple types in one line
type Person = {First:string; Last:string}

// complex types in a few lines
type Employee = 
  | Worker of Person
  | Manager of Employee list

// type inference
let jdoe = {First="John";Last="Doe"}
let worker = Worker jdoe
```

## ![](../assets/img/glyphicons/glyphicons_343_thumbs_up.png) Convenience


Many common programming tasks are much simpler in F#.  This includes things like creating and using
 [complex type definitions](../posts/conciseness-type-definitions/), doing [list processing](../posts/conciseness-extracting-boilerplate/),
 [comparison and equality](../posts/convenience-types/), [state machines](../posts/designing-with-types-representing-states/), and much more. 

And because functions are first class objects, it is very easy to create powerful and reusable code by creating functions
that have [other functions as parameters](../posts/conciseness-extracting-boilerplate/),
or that [combine existing functions](../posts/conciseness-functions-as-building-blocks/) to create new functionality. 

```
// automatic equality and comparison
type Person = {First:string; Last:string}
let person1 = {First="john"; Last="Doe"}
let person2 = {First="john"; Last="Doe"}
printfn "Equal? %A"  (person1 = person2)

// easy IDisposable logic with "use" keyword
use reader = new StreamReader(..)

// easy composition of functions
let add2times3 = (+) 2 >> (*) 3
let result = add2times3 5
```

## ![](../assets/img/glyphicons/glyphicons_150_check.png) Correctness


F# has a [powerful type system](../posts/correctness-type-checking.md) which prevents many common errors such
as [null reference exceptions](../posts/the-option-type.md#option-is-not-null).

Values are [immutable by default](../posts/correctness-immutability.md), which prevents a large class of errors.

In addition, you can often encode business logic using the [type system](../posts/correctness-exhaustive-pattern-matching.md) itself in such a way 
that it is actually [impossible to write incorrect code](/posts/designing-for-correctness/)
or mix up [units of measure](/posts/units-of-measure/), greatly reducing the need for unit tests.   


```
// strict type checking
printfn "print string %s" 123 //compile error

// all values immutable by default
person1.First <- "new name"  //assignment error 

// never have to check for nulls
let makeNewString str = 
   //str can always be appended to safely
   let newString = str + " new!"
   newString

// embed business logic into types
emptyShoppingCart.remove   // compile error!

// units of measure
let distance = 10<m> + 10<ft> // error!
```

## ![](../assets/img/glyphicons/glyphicons_054_clock.png) Concurrency 


F# has a number of built-in libraries to help when more than one thing at a time is happening.
Asynchronous programming is <a href="/posts/concurrency-async-and-parallel/">very easy</a>, as is parallelism.
F# also has a built-in <a href="/posts/concurrency-actor-model/">actor model</a>, and excellent support for event handling
and <a href="/posts/concurrency-reactive/">functional reactive programming</a>. 


And of course, because data structures are immutable by default, sharing state and avoiding locks is much easier.

```
// easy async logic with "async" keyword
let! result = async {something}

// easy parallelism
Async.Parallel [ for i in 0..40 -> 
      async { return fib(i) } ]

// message queues
MailboxProcessor.Start(fun inbox-> async{
	let! msg = inbox.Receive()
	printfn "message is: %s" msg
	})
```

## ![](../assets/img/glyphicons/glyphicons_280_settings.png) Completeness


Although it is a functional language at heart, F# does support other styles which are not 100% pure,
which makes it much easier to interact with the non-pure world of web sites, databases, other applications, and so on.

In particular, F# is designed as a hybrid functional/OO language, so it can do [virtually everything that C# can do](/posts/completeness-anything-csharp-can-do/).  


Of course, F# is [part of the .NET ecosystem](/posts/completeness-seamless-dotnet-interop/), which gives you seamless access to all the third party .NET libraries and tools.
It runs on most platforms, including Linux and smart phones (via Mono and the new .NETCore).


Finally, it is well integrated with Visual Studio (Windows) and Xamarin (Mac), which means you get a great IDE with IntelliSense support, a debugger,
and many plug-ins for unit tests, source control, and other development tasks. Or on Linux, you can use the MonoDevelop IDE instead.

```
// impure code when needed
let mutable counter = 0

// create C# compatible classes and interfaces
type IEnumerator<'a> = 
    abstract member Current : 'a
    abstract MoveNext : unit -> bool 

// extension methods
type System.Int32 with
    member this.IsEven = this % 2 = 0

let i=20
if i.IsEven then printfn "'%i' is even" i
	
// UI code
open System.Windows.Forms 
let form = new Form(Width= 400, Height = 300, 
   Visible = true, Text = "Hello World") 
form.TopMost <- true
form.Click.Add (fun args-> printfn "clicked!")
form.Show()
```

## The "Why Use F#?" series

The following series of posts demonstrates each of these F# benefits, using standalone snippets of F# code (and often with C# code for comparison).  

* 1. [Introduction to the 'Why use F#' series](../posts/why-use-fsharp-intro.md). An overview of the benefits of F#
* 2. F# syntax in 60 seconds. A very quick overview on how to read F# code
* 3. Comparing F# with C#: A simple sum. In which we attempt to sum the squares from 1 to N without using a loop
* 4. Comparing F# with C#: Sorting. In which we see that F# is more declarative than C#, and we are introduced to pattern matching.
* 5. Comparing F# with C#: Downloading a web page. In which we see that F# excels at callbacks, and we are introduced to the 'use' keyword
* 6. Four Key Concepts. The concepts that differentiate F# from a standard imperative language
* 7. Conciseness. Why is conciseness important?
* 8. Type inference. How to avoid getting distracted by complex type syntax
* 9. Low overhead type definitions. No penalty for making new types
* 10. Using functions to extract boilerplate code. The functional approach to the DRY principle
* 11. Using functions as building blocks. Function composition and mini-languages make code more readable
* 12. Pattern matching for conciseness. Pattern matching can match and bind in a single step
* 13. Convenience. Features that reduce programming drudgery and boilerplate code
* 14. Out-of-the-box behavior for types. Immutability and built-in equality with no coding
* 15. Functions as interfaces. OO design patterns can be trivial when functions are used
* 16. Partial Application. How to fix some of a function's parameters
* 17. Active patterns. Dynamic patterns for powerful matching
* 18. Correctness. How to write 'compile time unit tests'
* 19. Immutability. Making your code predictable
* 20. Exhaustive pattern matching. A powerful technique to ensure correctness
* 21. Using the type system to ensure correct code. In F# the type system is your friend, not your enemy
* 22. Worked example: Designing for correctness. How to make illegal states unrepresentable
* 23. Concurrency. The next major revolution in how we write software?
* 24. Asynchronous programming. Encapsulating a background task with the Async class
* 25. Messages and Agents. Making it easier to think about concurrency
* 26. Functional Reactive Programming. Turning events into streams
* 27. Completeness. F# is part of the whole .NET ecosystem
* 28. Seamless interoperation with .NET libraries. Some convenient features for working with .NET libraries
* 29. Anything C# can do... A whirlwind tour of object-oriented code in F#
* 30. Why use F#: Conclusion
