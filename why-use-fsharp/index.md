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

F# is not cluttered up with [coding "noise"](../posts/fvsc-sum-of-squares.md) such as curly brackets, semicolons and so on. 

You almost never have to specify the type of an object, thanks to a powerful [type inference system](../posts/conciseness-type-inference.md).

And, compared with C#, it generally takes [fewer lines of code](../posts/fvsc-download.md) to solve the same problem.


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
 [complex type definitions](../posts/conciseness-type-definitions.md), doing [list processing](../posts/conciseness-extracting-boilerplate.md),
 [comparison and equality](../posts/convenience-types.md), [state machines](../posts/designing-with-types-representing-states.md), and much more. 

And because functions are first class objects, it is very easy to create powerful and reusable code by creating functions
that have [other functions as parameters](../posts/conciseness-extracting-boilerplate.md),
or that [combine existing functions](../posts/conciseness-functions-as-building-blocks.md) to create new functionality. 

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
that it is actually [impossible to write incorrect code](../posts/designing-for-correctness.md)
or mix up [units of measure](../posts/units-of-measure.md), greatly reducing the need for unit tests.   


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
Asynchronous programming is [very easy](../posts/concurrency-async-and-parallel.md), as is parallelism.

F# also has a built-in [actor model](../posts/concurrency-actor-model.md), and excellent support for event handling
and [functional reactive programming](../posts/concurrency-reactive.md). 

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

In particular, F# is designed as a hybrid functional/OO language, so it can do [virtually everything that C# can do](../posts/completeness-anything-csharp-can-do.md).  


Of course, F# is [part of the .NET ecosystem](../posts/completeness-seamless-dotnet-interop.md), which gives you seamless access to all the third party .NET libraries and tools.
It runs on most platforms, including Linux and smart phones (via Mono and the new .NET Core).


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

* [Introduction to the 'Why use F#' series](../posts/why-use-fsharp-intro.md). An overview of the benefits of F#
* [F# syntax in 60 seconds](../posts/fsharp-in-60-seconds.md). A very quick overview on how to read F# code
* [Comparing F# with C#: A simple sum](../posts/fvsc-sum-of-squares.md). In which we attempt to sum the squares from 1 to N without using a loop
* [Comparing F# with C#: Sorting](../posts/fvsc-quicksort.md). In which we see that F# is more declarative than C#, and we are introduced to pattern matching.
* [Comparing F# with C#: Downloading a web page](../posts/fvsc-download.md). In which we see that F# excels at callbacks, and we are introduced to the 'use' keyword
* [Four Key Concepts](../posts/key-concepts.md). The concepts that differentiate F# from a standard imperative language
* [Conciseness](../posts/conciseness-intro.md). Why is conciseness important?
* [Type inference](../posts/conciseness-type-inference.md). How to avoid getting distracted by complex type syntax
* [Low overhead type definitions](../posts/conciseness-type-definitions.md). No penalty for making new types
* [Using functions to extract boilerplate code](../posts/conciseness-extracting-boilerplate.md). The functional approach to the DRY principle
* [Using functions as building blocks](../posts/conciseness-functions-as-building-blocks.md). Function composition and mini-languages make code more readable
* [Pattern matching for conciseness](../posts/conciseness-pattern-matching.md). Pattern matching can match and bind in a single step
* [Convenience](../posts/convenience-intro.md). Features that reduce programming drudgery and boilerplate code
* [Out-of-the-box behavior for types](../posts/convenience-types.md). Immutability and built-in equality with no coding
* [Functions as interfaces](../posts/convenience-functions-as-interfaces.md). OO design patterns can be trivial when functions are used
* [Partial Application](../posts/convenience-partial-application.md). How to fix some of a function's parameters
* [Active patterns](../posts/convenience-active-patterns.md). Dynamic patterns for powerful matching
* [Correctness](../posts/correctness-intro.md). How to write 'compile time unit tests'
* [Immutability](../posts/correctness-immutability.md). Making your code predictable
* [Exhaustive pattern matching](../posts/correctness-exhaustive-pattern-matching.md). A powerful technique to ensure correctness
* [Using the type system to ensure correct code](../posts/correctness-type-checking.md). In F# the type system is your friend, not your enemy
* [Worked example: Designing for correctness](../posts/designing-for-correctness.md). How to make illegal states unrepresentable
* [Concurrency](../posts/concurrency-intro.md). The next major revolution in how we write software?
* [Asynchronous programming](../posts/concurrency-async-and-parallel.md). Encapsulating a background task with the Async class
* [Messages and Agents](../posts/concurrency-actor-model.md). Making it easier to think about concurrency
* [Functional Reactive Programming](../posts/concurrency-reactive.md). Turning events into streams
* [Completeness](../posts/completeness-intro.md). F# is part of the whole .NET ecosystem
* [Seamless interoperation with .NET libraries](../posts/completeness-seamless-dotnet-interop.md). Some convenient features for working with .NET libraries
* [Anything C# can do...](../posts/completeness-anything-csharp-can-do.md). A whirlwind tour of object-oriented code in F#
* [Why use F#: Conclusion](../posts/why-use-fsharp-conclusion.md). 
