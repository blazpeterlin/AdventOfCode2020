module Dijkstra

open System
open System.IO
open Microsoft.FSharp.Collections
open System.Collections.Generic
open AocCommon

//Dijkstra's algorithm: Nigel Galloway, August 5th., 2018
[<CustomEquality;CustomComparison>]
type Dijkstra<'N,'G when 'G:comparison>={toN:'N;cost:Option<'G>;fromN:'N}
                                        override g.Equals n =match n with| :? Dijkstra<'N,'G> as n->n.cost=g.cost|_->false
                                        override g.GetHashCode() = hash g.cost
                                        interface System.IComparable with
                                          member n.CompareTo g =
                                            match g with
                                            | :? Dijkstra<'N,'G> as n when n.cost=None -> (-1)
                                            | :? Dijkstra<'N,'G>      when n.cost=None -> 1
                                            | :? Dijkstra<'N,'G> as g                  -> compare n.cost g.cost
                                            | _-> invalidArg "n" "expecting type Dijkstra<'N,'G>"
let Dijkstra N G y =
  let rec fN l f =
    if List.isEmpty l then f
    else let n=List.min l
         if n.cost=None then f else
         fN(l|>List.choose(fun n'->if n'.toN=n.toN then None else match n.cost,n'.cost,Map.tryFind (n.toN,n'.toN) G with
                                                                  |Some g,None,Some wg                ->Some {toN=n'.toN;cost=Some(g+wg);fromN=n.toN}
                                                                  |Some g,Some g',Some wg when g+wg<g'->Some {toN=n'.toN;cost=Some(g+wg);fromN=n.toN}
                                                                  |_                                  ->Some n'))((n.fromN,n.toN)::f)
  let r = fN (N|>List.map(fun n->{toN=n;cost=(Map.tryFind(y,n)G);fromN=y})) []
  (fun n->let rec fN z l=match List.tryFind(fun (_,g)->g=z) r with
                         |Some(n',g') when y=n'->Some(n'::g'::l)
                         |Some(n',g')          ->fN n' (g'::l)
                         |_                    ->None
          fN n [])
 

let GenerateEdges (getNeighbours:'T->'T seq) (N:'T seq) =
    let S = N |> Set
    let E = N |> Seq.map (fun n -> getNeighbours n |> Seq.filter (S.Contains) |> Seq.map (fun ngbr -> ((n, ngbr),1)) ) |> Seq.concat |> Map
    E

 //// example: 
 // type Node= |A|B|C|D|E|F
 // let G=Map[((A,B),7);((A,C),9);((A,F),14);((B,C),10);((B,D),15);((C,D),11);((C,F),2);((D,E),6);((E,F),9)]
 // let paths=Dijkstra [B;C;D;E;F] G A
 // printfn "%A" (paths E)
 // printfn "%A" (paths F)

let inline Dijkstra2D_PointList (isAdj:'P->'P->bool) (pts:'P list) (y:'P) =
    let rec fN l f =
        if List.isEmpty l 
        then f
        else 
            let n=List.min l
            if n.cost=None 
            then f 
            else
                fN (l
                    |>List.choose(fun n'->    
                                    if n'.toN=n.toN 
                                    then None 
                                    else
                                        match n.cost,n'.cost,isAdj n.toN n'.toN with
                                        |Some g,None,true             ->Some {toN=n'.toN;cost=Some(g+1);fromN=n.toN}
                                        |Some g,Some g',true when g+1<g'->Some {toN=n'.toN;cost=Some(g+1);fromN=n.toN}
                                        |_                                  ->Some n'))
                    ((n.fromN,n.toN)::f)
    let r = fN (pts|>List.map(fun n->{toN=n;cost=(if isAdj y n then Some 1 else None);fromN=y})) []
    (fun n->let rec fN z l=match List.tryFind(fun (_,g)->g=z) r with
                            |Some(n',g') when y=n'->Some(n'::g'::l)
                            |Some(n',g')          ->fN n' (g'::l)
                            |_                    ->None
            fN n [])

let inline Dijkstra2D_PointList_Moves4Plus pts y =
    let isAdj (x1, y1) (x2, y2) = abs (x1 - x2) + abs (y1 - y2) = 1
    Dijkstra2D_PointList isAdj pts y

let inline Dijkstra2D_PointList_Moves4X pts y =
    let isAdj (x1, y1) (x2, y2) = abs (y1 - y2) = 1 && abs (x1 - x2) = 1
    Dijkstra2D_PointList isAdj pts y

let inline Dijkstra2D_PointList_Moves8 pts y =
    let isAdj (x1, y1) (x2, y2) = abs (y1 - y2) <= 1 && abs (x1 - x2) <= 1
    Dijkstra2D_PointList isAdj pts y


// todo: could parameterize stopAt. if we need shortest path from Y1 to Y2, we don't need the entire map dijsktra'd

// int*int = 'P
//let Dijkstra2D_PointSet (getAdj:int*int->(int*int) list) (pts:(int*int) list) (y:int*int)  =
////let inline Dijkstra2D_PointSet (getAdj:int*int->Set<int*int>) (pts:list<int*int>) (y:int*int)  =
//    let smartAdj = new Dictionary<int*int, (int*int) Set>()
//    let getCachedAdj (pt:int*int) =
//    //let smartAdj = new Dictionary<int*int, Set<int*int> >()
//    //let getCachedAdj (pt:int*int) =
//        match tryGetValue pt smartAdj with
//        | Some result -> result
//        | None ->
//            let result = getAdj pt |> Set
//            smartAdj.[pt] <- result
//            result

//    let rec fN (l:Map<'t, Dijkstra<'t,int>>) excl f =
//        if Map.count l = Set.count excl then f else 
//        let n=Map.min l
//        if n.cost=None then f else

//        let adjSet = getCachedAdj n.toN |> Seq.filter (fun adji -> not(excl.Contains adji))

//        let lC = l.Length

//        //let unsolvedNodes = 
//        //    l
//        //    |>List.choose(fun n'->    
//        //                    if n'.toN=n.toN then None else
//        //                    if not (adjSet.Contains n'.toN) then Some n' else
//        //                    match n.cost,n'.cost with
//        //                    |Some g,None             ->Some {toN=n'.toN;cost=Some(g+1);fromN=n.toN}
//        //                    |Some g,Some g' when g+1<g'->Some {toN=n'.toN;cost=Some(g+1);fromN=n.toN}
//        //                    |_                                  ->Some n'
//        //                    )

//        let x = 
//            adjSet 
//            |> Seq.choose 
//                (fun n' ->
//                    match n.cost, n'.cost with
//                    | Some g, None                   -> Some {toN=n'.toN;cost=Some(g+1);fromN=n.toN}
//                    | Some g,Some g' when g+1<g'     -> Some {toN=n'.toN;cost=Some(g+1);fromN=n.toN}
//                    | _                              -> Some n'
//                )
            

//        let excl2 = excl.Add n.toN

//        let pathSoFar = (n.fromN,n.toN)::f

//        fN unsolvedNodes excl2 pathSoFar

//    let adjSet = getCachedAdj y
//    let state = pts |> Seq.map (fun pt -> {toN=pt; cost=(if adjSet.Contains pt then Some 1 else None); fromN=y}) |> Seq.map (fun x -> (x.toN, x)) |> Map
//    let r = fN state Set.empty []

//    let dictR = r |> List.map (fun (x,y) -> (y,x)) |> Map

//    (fun n -> 
//        let rec fN z l = 
//            match (tryGetValue z dictR) with
//            |Some n' when y=n'->Some(n'::z::l)
//            |Some n'          ->fN n' (z::l)
//            |_                    ->None

//        fN n []
//    )

//    //(fun n->let rec fN z l=match List.tryFind(fun (_,g)->g=z) r with
//    //                        |Some(n',g') when y=n'->Some(n'::g'::l)
//    //                        |Some(n',g')          ->fN n' (g'::l)
//    //                        |_                    ->None
//    //        fN n [])

let inline getAdjPlus (x,y) = (x-1,y)::(x+1,y)::(x,y-1)::(x,y+1)::[] |> seq
let inline getAdjX (x,y) = (x-1,y-1)::(x+1,y+1)::(x+1,y-1)::(x-1,y+1)::[] |> seq
let inline getAdj8 (x,y) = 
    let batch1 = getAdjPlus (x,y)
    let batch2 = getAdjX (x,y)
    Seq.append batch1 batch2


//let DFS 