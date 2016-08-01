---
layout: page
title: "Installing and using F#"
description: "Instructions for downloading, installing and using F# with Visual Studio, SharpDevelop and MonoDevelop"
nav: 
hasComments: 1
image: "/assets/img/fsharp_eval2.png"

---

The F# compiler is a free and open source tool which is available for Windows, Mac and Linux (via Mono).
Find out more about F# and how to install it at the [F# Foundation](http://fsharp.org/).

You can use it with an IDE (Visual Studio, MonoDevelop), or with your favorite editor (VS Code and Atom have especially good F# support using [Ionide](http://ionide.io/)),
or simply as a standalone command line compiler.  

If you don't want to install anything, you can try the [.NET Fiddle](https://dotnetfiddle.net/) site, which is an interactive environment
where you can explore F# in your web browser. You should be able to run most of the code on this site there.

## Working with the code examples ##

Once you have F# installed and running, you can follow along with the code samples.  

The best way to run the code examples on this site is to type the code into an `.FSX` script file, which you can then send to the F# interactive window for evaluation.
Alternatively you can type the examples directly into the F# interactive console window. I would recommend the script file approach for anything other than one or two lines.

For the longer examples, the code is downloadable from this website -- the links will be in the post. 

Finally, I would encourage you to play with and modify the examples. If you then get compiler errors,
do check out the ["troubleshooting F#"](../troubleshooting-fsharp/index.md) page, which explains the most common problems, and how to fix them.


<a id="projects-solutions" ></a>   
## Projects and Solutions ##

F# uses exactly the same "projects" and "solutions" model that C# does, so if you are familiar with that, you should be able to create an F# executable quite easily.  

To make a file that will be compiled as part of the project, rather than a script file, use the `.fs` extension. `.fsx` files will not be compiled.

An F# project does have some major differences from C# though:

* The F# files are organized *linearly*, not in a hierarchy of folders and subfolders.
  In fact, there is no "add new folder" option in an F# project! This is not generally a problem, because, unlike C#,
  an F# file contains more than one class.  What might be a whole folder of classes in C# might easily be a single file in F#.
* The *order of the files in the project is very important*: a "later" F# file can use the public types defined in an "earlier" F# file,
  but not the other way around. Consequently, you cannot have any circular dependencies between files.
* You can change the order of the files by right-clicking and doing "Move Up" or "Move Down".
  Similarly, when creating a new file, you can choose to "Add Above" or "Add Below" an existing file.

<a id="shell-scripts" ></a>   
## Shell scripts in F# ##

You can also use F# as a scripting language, rather than having to compile code into an EXE.  This is done by using the FSI program, which is not only a console but can also be used to run scripts in the same way that you might use Python or Powershell. 

This is very convenient when you want to quickly create some code without compiling it into a full blown application. The F# build automation system ["FAKE"](https://github.com/fsharp/FAKE) is an example of how useful this can be.

To see how you can do this yourself, here is a little example script that downloads a web page to a local file. First create an FSX script file -- call it "`ShellScriptExample.fsx`" -- and paste in the following code. 

```
// ================================
// Description: 
//    downloads the given url and stores it as a file with a timestamp
//
// Example command line: 
//    fsi ShellScriptExample.fsx http://google.com google
// ================================

// "open" brings a .NET namespace into visibility
open System.Net
open System

// download the contents of a web page
let downloadUriToFile url targetfile =        
    let req = WebRequest.Create(Uri(url)) 
    use resp = req.GetResponse() 
    use stream = resp.GetResponseStream() 
    use reader = new IO.StreamReader(stream) 
    let timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm")
    let path = sprintf "%s.%s.html" targetfile timestamp 
    use writer = new IO.StreamWriter(path) 
    writer.Write(reader.ReadToEnd())
    printfn "finished downloading %s to %s" url path

// Running from FSI, the script name is first, and other args after
match fsi.CommandLineArgs with
    | [| scriptName; url; targetfile |] ->
        printfn "running script: %s" scriptName
        downloadUriToFile url targetfile
    | _ ->
        printfn "USAGE: [url] [targetfile]"
```

Don't worry about how the code works right now. It's pretty crude anyway, and a better example would add error handling, and so on.

To run this script, open a command window in the same directory and type:

```
fsi ShellScriptExample.fsx http://google.com google_homepage
```

As you play with the code on this site, you might want to experiment with creating some scripts at the same time.

