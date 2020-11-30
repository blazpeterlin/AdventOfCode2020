module AocCommon

open System
open System.IO
open Microsoft.FSharp.Collections
open System.Collections.Generic
open System.Diagnostics

open Microsoft.FSharp.Reflection
    

let thd (x,y,z) = z

let acSplit (chars : string) (line : string) = 
    line.Split(chars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

let acSplitStr (strs : string[]) (line : string) = 
    line.Split(strs, StringSplitOptions.RemoveEmptyEntries)

let appendPipe str1 str2 = Seq.append str2 str1

//let acMinByAll
let tryGetValue key (dict:IDictionary<_,_>) =
    match dict.TryGetValue(key) with
    | true, value -> Some value
    | false,_ -> None

let tryGetValue2 (dict:IDictionary<_,_>) key =
    match dict.TryGetValue(key) with
    | true, value -> Some value
    | false,_ -> None

let groupBy2 getKey getVal src =
    Seq.groupBy getKey src
    |> Seq.map (fun (key, vals) -> (key, Seq.map getVal vals))

type DLL<'T>(prev : DLL<'T> option, next: DLL<'T> option, content : 'T) as this = class 
    
    //member this.Content = content with get,set
    
    let mutable _Prev : DLL<'T> option = prev
    let mutable _Next : DLL<'T> option = next
    
    member x.Prev
        with get() = (match _Prev with | None -> this | Some x -> x)
        and set(newVal) = _Prev <- Some newVal
    member x.Next
        with get() = (match _Next with | None -> this | Some x -> x)
        and set(newVal) = _Next <- Some newVal

    member that.LinkNext (c:'T) =
        let newOne = new DLL<'T>(Some that, Some that.Next, c)
        let next = that.Next
        next.Prev <- newOne
        that.Next <- newOne

        newOne

        //newOne.Next <- next
        //newOne.Prev <- this

    member this.UnlinkNext =
        let realNext = this.Next.Next
        this.Next <- realNext
        realNext.Prev <- this
    //member val Next : DLL<'T> with next with get,set


    member val Content = content with get

    new(content) = DLL(None, None, content)
        //this2.Prev = this
        //this2.Next = this
end

let foldDllNext = fun (dll:DLL<int>) _ -> dll.Next
let foldDllPrev = fun (dll:DLL<int>) _ -> dll.Prev


let printSparse points =
    //let knownPoints = points |> List.map (fun pt -> match pt with | {x,y} -> Some {x=x;y=y} | _ -> None) |> List.choose
    let minX = points |> List.map (fun (x,y) -> x) |> List.min
    let maxX = points |> List.map (fun (x,y) -> x) |> List.max
    
    let minY = points |> List.map (fun (x,y) -> y) |> List.min
    let maxY = points |> List.map (fun (x,y) -> y) |> List.max

    let set = points |> Set

    if maxX - minX > 150 || maxY - minY > 150
    then false
    else
        for j in minY .. maxY do
            for i in minX .. maxX do
                Console.Write (
                    if set.Contains ((i,j))
                    then "█" 
                    else " "
                )
            Console.WriteLine()

        true



let printSparseChars points =
    //let knownPoints = points |> List.map (fun pt -> match pt with | {x,y} -> Some {x=x;y=y} | _ -> None) |> List.choose
    let minX = points |> List.map (fun ((x,y),c) -> x) |> List.min
    let maxX = points |> List.map (fun ((x,y),c) -> x) |> List.max
    
    let minY = points |> List.map (fun ((x,y),c) -> y) |> List.min
    let maxY = points |> List.map (fun ((x,y),c) -> y) |> List.max

    let set = points |> Map

    if maxX - minX > 150 || maxY - minY > 150
    then false
    else
        for j in minY .. maxY do
            for i in minX .. maxX do
                Console.Write (
                    if set.ContainsKey ((i,j))
                    then set.[(i,j)]
                    else " "
                )
            Console.WriteLine()

        true


let allMinBy fn sq =
    if Seq.isEmpty sq
    then Seq.empty
    else
        let mappedSq = sq |> Seq.map (fun elt -> (elt, fn elt))
        let minVal = mappedSq |> Seq.map snd |> Seq.min
        let minElts = mappedSq |> Seq.filter (fun (_, v) -> v = minVal)
        minElts |> Seq.map fst
    
let allMaxBy fn sq =
    if Seq.isEmpty sq
    then Seq.empty
    else
        let mappedSq = sq |> Seq.map (fun elt -> (elt, fn elt))
        let maxVal = mappedSq |> Seq.map snd |> Seq.max
        let maxElts = mappedSq |> Seq.filter (fun (_, v) -> v = maxVal)
        maxElts |> Seq.map fst


let printToFile filename openAfter useSw = 
    use sw = File.CreateText(filename)
    useSw sw
    if openAfter
    then 
        let psi = ProcessStartInfo()
        psi.Arguments<-filename
        psi.UseShellExecute<-true
        psi.FileName<-filename
        psi.Verb<-"OPEN"
        System.Diagnostics.Process.Start psi |> ignore
    else 0 |> ignore



let isAdj4Plus (x1,y1) (x2,y2) =
    abs(x1-x2)+abs(y1-y2)=1
let isAdj4X (x1,y1) (x2,y2) =
    abs(x1-x2) =1 && abs(y1-y2)=1
let isAdj8 (x1,y1) (x2,y2) =
    [abs(x1-x2);abs(y1-y2)] |> Seq.max |> fun x -> x = 1


let toString (x:'a) = 
    let (case, _ ) = FSharpValue.GetUnionFields(x, typeof<'a>)
    case.Name
    
let fromString<'a> (s:string) =
    match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name.ToLower() = s.ToLower()) with
    |[|case|] -> Some(FSharpValue.MakeUnion(case,[||]) :?> 'a)
    |_ -> None
    