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

[<AutoOpen>]
module Helpers =
    let replaceRegex (pattern:string) (replacement:string) (input:string) = 
        Regex.Replace(input,pattern,replacement)

    (*
    replaceRegex @"(?m)^\{\%\s+highlight.*?$" "```" "{% highlight fsharp %}\r\nabc"
    replaceRegex @"(?m)^\d\d\d\d-\d\d-\d\d-(.*?$)" "$1" "2012-09-01-myfile.md"

    replaceRegex @"\(\\(.*?)\)" "(..\$1)" @"(\posts\abc\) )"
    replaceRegex @"\((.*?)\\\)" @"($1\index.md)" @"(\posts\abc\) )"
    replaceRegex @"\((.*?)html\)" @"($1md)"  @"(\posts\abc.html) )"

    *)

    let replace (find:string) replacement (input:string) = 
        input.Replace(find,replacement)

    let ifNone v opt = defaultArg opt v


module FixupFiles = 

    let rec fixupFileNames (d:DirectoryInfo) = 

        let renameIfNeeded (fi:FileInfo) =
            let oldName = fi.Name
            let newName = replaceRegex "(?m)^\d\d\d\d-\d\d-\d\d-(.*?$)" "$1" oldName 
            if newName <> oldName  then
                let path = fi.FullName.Replace(oldName,newName)
                try
                    fi.MoveTo(path)
                with
                | ex ->     
                    printfn "%s %s" fi.FullName ex.Message

        d.EnumerateFiles("*.md") 
        |> Seq.iter renameIfNeeded 

        d.EnumerateDirectories() 
        |> Seq.iter fixupFileNames 

module FixupText = 

    let replaceCodeBlockDelimiters text = 
        text
        |> replaceRegex "(?m)^\{\%\s+highlight.*?$" "```"
        |> replaceRegex "(?m)^\{\%\s+endhighlight.*?$" "```"

    let fixupLinkPaths text = 
        text
        |> replaceRegex @"\(/(.*?)\)" "(../$1)"     // replace root (/xxx) with (../xxx)
        |> replaceRegex @"\(\.\./(.*?)/\)" @"(../$1/index.md)"  // replace (../dir/) with (../dir/index.md)
        |> replaceRegex @"\(\.\./(.*?)/#(.*?)\)" @"(../$1/index.md#$2)"  // replace (../dir/#id) with (../dir/index.md#id)
        |> replaceRegex @"\(\.\./series/(.*?).html\)" @"(../series/$1.md)"  // replace (../series/xxx.html) with (../series/xxx.md)

    let fixupVideoPaths text = 
        text
        |> replace "../cap/index.md" "http://fsharpforfunandprofit.com/cap/"
        |> replace "../ddd/index.md" "http://fsharpforfunandprofit.com/ddd/"
        |> replace "../ettt/index.md" "http://fsharpforfunandprofit.com/ettt/"
        |> replace "../fppatterns/index.md" "http://fsharpforfunandprofit.com/fppatterns/"
        |> replace "../monadster/index.md" "http://fsharpforfunandprofit.com/monadster/"
        |> replace "../parser/index.md" "http://fsharpforfunandprofit.com/parser/"
        |> replace "../pbt/index.md" "http://fsharpforfunandprofit.com/pbt/"
        |> replace "../rop/index.md" "http://fsharpforfunandprofit.com/rop/"

    let fixupSmartQuotes text = 
        text
        |> replace "“" "\""
        |> replace "”" "\""
        |> replace "’" "'"
        |> replace "–" "--"

    let fixupText text = 
        text
        |> replaceCodeBlockDelimiters
        |> fixupLinkPaths
        |> fixupSmartQuotes 
        |> fixupVideoPaths 

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



module Series = 

    type PageInfo = {
        File : FileInfo
        Title: string
        Description: string
        SeriesId: string option
        SeriesIndexId : string option
        SeriesOrder: int option
        }

    let findField fieldName (line:string) = 
        let dquotedPattern = "(?i)" + fieldName + """\s*:\s*"([^"]*)"\s*"""
        let squotedPattern = "(?i)" + fieldName + """\s*:\s*'([^']*)'\s*"""
        let unquotedPattern = "(?i)" + fieldName + """\s*:\s*(\S+)\s*"""
        [dquotedPattern; squotedPattern; unquotedPattern]
        |> List.tryPick (fun pattern ->
            let m = Regex.Match(line,pattern)
            if m.Success then
                Some <| m.Groups.[1].Value
            else
                None
            )
        (*
        let pattern = """title:\s*"([^"]+)"\s*"""
        let m = Regex.Match("""title:"abc" """,pattern) in m.Value
        let m = Regex.Match("""title:"abc" """,pattern) in m.Groups.[1].Value
        let pattern = """title:\s*(\S+)\s*"""
        let m = Regex.Match("""title: abc """,pattern) in m.Groups.[1].Value
        let m = Regex.Match("""title:"abc" """,pattern) in m.Groups.[1].Value

        findField "title" """title:"" """
        findField "title" """title:"abc" """
        findField "title" """title: abc """
        findField "title" """desc: abc """
        *)


    let parsePageText fi (lines:string seq) = 
        let title = lines |> Seq.tryPick (findField "title") |> ifNone ""
        let description = lines |> Seq.tryPick (findField "description") |> ifNone ""
        let seriesId = lines |> Seq.tryPick (findField "seriesId") 
        let seriesIndexId = lines |> Seq.tryPick (findField "seriesIndexId") 
        let seriesOrder = lines |> Seq.tryPick (findField "seriesOrder") |> Option.map int
        {
        File = fi
        Title = title
        Description = description
        SeriesId = seriesId
        SeriesIndexId = seriesIndexId
        SeriesOrder = seriesOrder
        }

        (*
        let fi = FileInfo("..")
        let lines = 
            [
            """layout: post"""
            """title: "Comparing F# with C#: A simple sum" """
            """description: "In which we attempt to sum the squares from 1 to N without using a loop" """
            """nav: why-use-fsharp"""
            """seriesId: "Why use F#?" """
            """seriesOrder: 3"""
            """categories: [F# vs C#]"""
            ]
        parsePageText fi lines

        *)

    let parsePage (fi:FileInfo) =
        let path = fi.FullName
        File.ReadAllLines(path,Text.Encoding.Default)
        |> Seq.truncate 10
        |> parsePageText fi
        

    let seriesIdToPostPages()  = 
        let path = @"..\posts"
        let d = DirectoryInfo(path)
        d.EnumerateFiles("*.md") 
        |> Seq.map parsePage 
        |> Seq.toList
        |> List.groupBy (fun page -> page.SeriesId)
        |> List.map (fun (keyOpt,vals) -> keyOpt, vals |> List.sortBy (fun page -> page.SeriesOrder) )
        |> List.choose (fun (keyOpt,vals) -> keyOpt |> Option.map (fun key -> key,vals))
        |> Map.ofList

    // Series.seriesIdToPostPages()

    let seriesIdToSeriesIndexPages()  = 
        let path = @"..\series"
        let d = DirectoryInfo(path)
        d.EnumerateFiles("*.md") 
        |> Seq.map parsePage 
        |> Seq.toList
        |> List.choose (fun page -> page.SeriesIndexId |> Option.map (fun key -> key,page))

    // Series.seriesIdToSeriesIndexPages()

    type SeriesInfo = {
        SeriesPage : PageInfo
        PostPages : PageInfo list
        }

    let updateSeriesPageContent (seriesInfo:SeriesInfo) =
        let path = seriesInfo.SeriesPage.File.FullName
        let sb = File.ReadAllText(path) |> StringBuilder
        sb.AppendLine().AppendLine() |> ignore  // some vertical space

        seriesInfo.PostPages |> List.iter (fun page ->
            let title = page.Title
            let desc = if page.Description <> "" then page.Description + "." else ""
            let link = """../posts/""" + page.File.Name
            sb.AppendFormat("* [{0}]({1}). {2}",title,link,desc).AppendLine() |> ignore
        )
        File.WriteAllText(path,sb.ToString()) 

    let updateSeriesInfoPages()  = 
        let seriesIdToPostPages = seriesIdToPostPages()
        seriesIdToSeriesIndexPages() 
        |> List.map (fun (id,seriesPage) -> 
            let postPages = seriesIdToPostPages |> Map.tryFind id |> ifNone []
            {SeriesPage=seriesPage; PostPages=postPages}
            )
        |> List.iter updateSeriesPageContent
        
module Images = 

    let findImagesInFile (fi:FileInfo) =
        let path = fi.FullName
        let text = File.ReadAllText(path)
        let pattern = """![.*]\((.*?)\)"""
        Regex.Matches(text,pattern)
        |> Seq.cast<Match>
        |> Seq.map( fun m -> m.Groups.[1].Value)

        (*
        let pattern = """![.*]\((.*?)\)"""
        let text = 
        Regex.Matches(text,pattern)

        *)

    let rec findImages (d:DirectoryInfo) = 
        seq {
        yield! 
            d.EnumerateFiles("*.md") 
            |> Seq.collect findImagesInFile

        yield! 
            d.EnumerateDirectories() 
            |> Seq.collect findImages 
        }
        |> Seq.distinct
        |> Seq.toList
        |> List.sort

// process all
let path = @"..\"
let d = DirectoryInfo(path)

FixupFiles.fixupFileNames d
FixupText.fixupDirectory d

// Series.updateSeriesInfoPages()

Images.findImages d