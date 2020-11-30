module PriorityQueue

open System

type BinomialTree<'a> = PQNode of (int * 'a * list<BinomialTree<'a>>)


type t<'a> = list<BinomialTree<'a>>

let pqEmpty = []
let pqIsEmpty = function [] -> true | _ -> false
let pqRank (PQNode(r, _, _)) = r
let pqRoot (PQNode(_, x, _)) = x
let private link (PQNode(r, x1, xs1) as n1) (PQNode(_, x2, xs2) as n2) =
    if x1 <= x2 then
        PQNode(r+1, x1, n2 :: xs1)
    else
        PQNode(r+1, x2, n1 :: xs2)

let rec private pqInsertTree t = function
    | [] -> [t]
    | hd::tl as t' ->
        if pqRank t < pqRank hd then t::t' else pqInsertTree (link t hd) tl

let private pqSingletonTree x = PQNode(0, x, [])

let pqSingleton x = [pqSingletonTree x] 

let pqInsert x t =
    pqInsertTree (pqSingletonTree x) t

let rec pqMerge h1 h2 =
    match h1, h2 with
    | [], x -> x
    | x, [] -> x
    | (hd1::tl1), (hd2::tl2) ->
        if pqRank hd1 < pqRank hd2 then hd1 :: pqMerge tl1 h2
        elif pqRank hd1 > pqRank hd2 then hd2 :: pqMerge h1 tl2
        else pqInsertTree (link hd1 hd2) (pqMerge ( tl1) ( tl2))

let rec private pqRemoveMinTree = function
    | [] -> Exception("Empty") |> raise
    | [t] -> t, []
    | hd::tl ->
        let hd', tl'= pqRemoveMinTree tl
        if pqRoot hd <= pqRoot hd' then hd, tl else hd', hd::tl'

let pqFindMin h =
    let (t, _) = pqRemoveMinTree h
    pqRoot t

let pqRemoveMin h =
    let PQNode(_, x, xs1), xs2 = pqRemoveMinTree h
    pqMerge (List.rev xs1) xs2