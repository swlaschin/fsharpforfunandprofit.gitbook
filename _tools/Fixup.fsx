System.IO.Directory.SetCurrentDirectory __SOURCE_DIRECTORY__

// ==============================================
// Script file to fixup files for publishing to GitBook
//
// == Inputs and outputs ==
//
// The tool will process all code under the parent directory (../) by default

open System
open System.IO
open System.Text
open System.Text.RegularExpressions

let replaceRegex (pattern:string) (replacement:string) (input:string) = 
    Regex.Replace(input,pattern,replacement)

(*
replaceRegex @"(?m)^\{\%\s+highlight.*?$" "```" "{% highlight fsharp %}\r\nabc"
replaceRegex @"(?m)^\d\d\d\d-\d\d-\d\d-(.*?$)" "$1" "2012-09-01-myfile.md"

replaceRegex @"\(\\(.*?)\)" "(..\$1)" @"(\posts\abc\) )"
replaceRegex @"\((.*?)\\\)" @"($1\index.md)" @"(\posts\abc\) )"
*)

let replace (find:string) replacement (input:string) = 
    input.Replace(find,replacement)


let replaceCodeBlockDelimiters text = 
    text
    |> replaceRegex "(?m)^\{\%\s+highlight.*?$" "```"
    |> replaceRegex "(?m)^\{\%\s+endhighlight.*?$" "```"

let fixupLinkPaths text = 
    text
    |> replaceRegex @"\(\\(.*?)\)" "(..\$1)" 
    |> replaceRegex @"\((.*?)\\\)" @"($1\index.md)" 

let fixupSmartQuotes text = 
    text
    |> replace "“" "\""
    |> replace "”" "\""
    |> replace "’" "'"

let fixupText text = 
    text
    |> replaceCodeBlockDelimiters
    |> fixupLinkPaths
    |> fixupSmartQuotes 

// write to file
let fixupFile (fi:FileInfo) = 
    let path = fi.FullName
    File.ReadAllText(path,Text.Encoding.Default)
    |> fixupText
    |> fun text -> File.WriteAllText(path,text,Text.Encoding.ASCII)

let rec fixupDirectory (d:DirectoryInfo) = 

    d.EnumerateFiles("*.md") 
    |> Seq.iter fixupFile

    d.EnumerateDirectories() 
    |> Seq.iter fixupDirectory


let rec fixupFileNames (d:DirectoryInfo) = 

    let renameIfNeeded (fi:FileInfo) =
        let oldName = fi.Name
        let newName = replaceRegex "(?m)^\d\d\d\d-\d\d-\d\d-(.*?$)" "$1" oldName 
        if newName <> oldName  then
            let path = fi.FullName.Replace(oldName,newName)
            fi.MoveTo(path)

    d.EnumerateFiles("*.md") 
    |> Seq.iter renameIfNeeded 

    d.EnumerateDirectories() 
    |> Seq.iter fixupFileNames 

// process all
let path = @"..\"
let d = DirectoryInfo(path)

fixupFileNames d
fixupDirectory d

