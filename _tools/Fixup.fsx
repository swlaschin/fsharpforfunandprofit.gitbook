System.IO.Directory.SetCurrentDirectory __SOURCE_DIRECTORY__

// ==============================================
// Script file to fixup files for publishing to GitBook
//
// == Inputs and outputs ==
//
// The tool will process all code under the parent directory (../) by default

open System
open System.IO

let replace (find:string) replace (s:string) = 
    s.Replace(find,replace)

let replaceLine (startsWith:string) replace (s:string) = 
    if s.StartsWith startsWith then
        replace 
    else
        s
let replaceCodeBlockDelimiters text = 
    text
    |> replaceLine "{% highlight" "```"
    |> replaceLine "{% endhighlight" "```"

let fixupLine text = 
    text
    |> replaceCodeBlockDelimiters

// write to file
let fixupFile (fi:FileInfo) = 
    let path = fi.FullName
    File.ReadAllLines path 
    |> Seq.map fixupLine
    |> fun lines -> File.WriteAllLines(path,lines)

let rec fixupDirectory (d:DirectoryInfo) = 

    d.EnumerateFiles("*.md") 
    |> Seq.iter fixupFile

    d.EnumerateDirectories() 
    |> Seq.iter fixupDirectory


// process all
let path = @"..\"
let d = DirectoryInfo(path)
fixupDirectory d

