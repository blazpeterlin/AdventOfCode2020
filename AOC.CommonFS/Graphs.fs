module Graphs

open System
open System.IO
open Microsoft.FSharp.Collections
open System.Collections.Generic
open AocCommon
open PriorityQueue


let GenerateEdges (getNeighbours:'T->'T seq) (N:'T seq) =
    let S = N |> Set
    let E = N |> Seq.map (fun n -> getNeighbours n |> Seq.filter (S.Contains) |> Seq.map (fun ngbr -> ((n, ngbr),1)) ) |> Seq.concat |> Map
    E

let inline getAdjPlus (x,y) = (x-1,y)::(x+1,y)::(x,y-1)::(x,y+1)::[] |> seq
let inline getAdjX (x,y) = (x-1,y-1)::(x+1,y+1)::(x+1,y-1)::(x-1,y+1)::[] |> seq
let inline getAdj8 (x,y) = 
    let batch1 = getAdjPlus (x,y)
    let batch2 = getAdjX (x,y)
    Seq.append batch1 batch2












type Edge<'V> = { Src: 'V; Dst: 'V; Cost: float; }



[<CustomEquality;CustomComparison>]
type SortableV<'V, 'C when 'C:comparison>={v:'V;cost:Option<'C>}
                                            override g.Equals n =match n with| :? SortableV<'V,'C> as n->n.cost=g.cost|_->false
                                            override g.GetHashCode() = hash g.cost
                                            interface System.IComparable with
                                              member n.CompareTo g =
                                                match g with
                                                | :? SortableV<'V,'C> as n when n.cost=None -> (-1)
                                                | :? SortableV<'V,'C>      when n.cost=None -> 1
                                                | :? SortableV<'V,'C> as g                  -> compare n.cost g.cost
                                                | _-> invalidArg "n" "expecting type SortableV<'N,'G>"

let Dijkstra (V:'V list) (getE:'V -> Edge<'V> seq) (start:'V) (finish:'V option) =
    let mutable horizonQ = pqSingleton {v=start;cost=Some 0.0}
    let mutable opened = V |> Set
    //let mutable closed = [] |> Set
    let mutable ends = V |> Set
    let mutable dist = [(start, 0.0)] |> Map |> Dictionary
    let mutable pathsRev = [(start, [start])] |> Map |> Dictionary

    while not (pqIsEmpty horizonQ) do
        let curr = pqFindMin horizonQ
        horizonQ <- pqRemoveMin horizonQ


        if not (opened.Contains curr.v) then 0|>ignore else
        //if closed.Contains curr.v then 0|>ignore else
        //closed <- closed.Add curr.v
        opened <- opened.Remove curr.v

        if finish.IsSome && finish.Value = curr.v then horizonQ <- pqEmpty else

        let currEs = getE curr.v |> Seq.filter (fun x -> opened.Contains x.Dst)
        for e in currEs do
            let nextDist = dist.[curr.v] + e.Cost
            let next = e.Dst
            
            if  not (dist.ContainsKey next) 
                ||
                nextDist < dist.[next]
            then 
                dist.[next] <- nextDist
                pathsRev.[next] <- next::pathsRev.[curr.v]
                horizonQ <- pqInsert {v=next;cost=Some nextDist} horizonQ
                ends <- ends.Remove curr.v

    (dist, pathsRev, ends)

let DijkstraNoWeight (V:'V list) (getE:'V -> 'V seq) (start:'V) (finish:'V option) =
    let getEdgesWeights = fun src -> getE src |> Seq.map (fun dst -> {Src = src; Dst = dst; Cost=1.0})
    Dijkstra V getEdgesWeights start finish

