using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOC.Common;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;
using static AOC.Common.Func;
using System.IO;
using System.Text.RegularExpressions;

namespace d20
{
    class Solved6AM
    {
        //// Reminders:

        //var txt = ih.AsText();
        //var tkns = ih.AsTokens<int>();
        //var chch = ih.AsCharListOfLists();

        // AsInt
        // Graph<(int, int)>.From2dWithMoves(pts, Moves.PLUS).Dijkstra((0,0),(3,1)).Path();
        // ih.ModifyLines(ln => { if (ln == "") return "-"; else return ln; });

        //.Select(ln => ln.FSplit("-", " ", ":"))
        //.Select(tkns => tkns.Tuplify(int.Parse, Enumerable.First, x => x)) // int, char, string
        //.Where(t => t.t1 < t.t2)

        // var allPos = chs.ToValByPos();

        // var vm = new VirtualMachine(lns);
        // var visited = new HashSet<int>();
        // bool rut()
        // {
        //     return !visited.Add(vm.Idx);
        // }
        // vm.RunUntilTrue(rut);
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var grps = ih.AsLines().Split("");

            var tiles = grps.Select(grp => grp.Skip(1).Select(_ => _.ToList()).ToList()).ToList();
            var ids = grps.Select(_ => _.ToList()[0].Split(' ', ':')[1].AsInt()).ToList();
            var tilesById = new Dictionary<int, List<List<char>>>();
            for(int i = 0; i < ids.Count; i++)
            {
                tilesById[ids[i]] = tiles[i];
            }

            Dictionary<int, List<(string edge, int edgeType)>> edgesByT = new();
            Dictionary<string, List<(int tile, int edgeType)>> tByEdge = new();

            for (int i = 0; i <tiles.Count; i++)
            {
                var tile = tiles[i];
                var id = ids[i];

                string x0 = "", y0="", xN="", yN="";
                //foreach(var dim1 in new[] { 0, tile.Count-1 })
                {
                    foreach(var dim2 in FRng(0, tile.Count))
                    {
                        x0 += tile[0][dim2];
                        xN += tile[tile.Count - 1][dim2];
                        y0 += tile[dim2][0];
                        yN += tile[dim2][tile.Count - 1];
                        //y0 = "" + tile[dim2][0] + y0;
                        //yN = "" + tile[dim2][tile.Count - 1] + yN;
                    }
                }

                var allCandidates = new[] { (x0,0), (y0,3), (xN,2), (yN,1) }.ToList();
                var candidatesReversed = allCandidates.Select(tpl => (new string(tpl.Item1.Reverse().ToArray()), tpl.Item2 + 4)).ToList();
                candidatesReversed.Reverse(); // reverse..?
                var last2first = candidatesReversed.Last();
                candidatesReversed.Insert(0, last2first);
                candidatesReversed.RemoveAt(4);

                allCandidates = allCandidates.Union(candidatesReversed).ToList();       
                foreach(var c in allCandidates)
                {
                    if (!edgesByT.ContainsKey(id)) { edgesByT[id] = new List<(string edge, int edgeType)>(); }
                    edgesByT[id].Add(c);

                    if (!tByEdge.ContainsKey(c.Item1)) { tByEdge[c.Item1] = new List<(int tile, int edgeType)>(); }
                    tByEdge[c.Item1].Add((id, c.Item2));
                }
            }

            {
                var tileTheoreticalNeighbours = new Dictionary<int, List<(int tile, int edgeTypeKey, int edgeTypeVal)>>();
                foreach(var kvp in edgesByT)
                {
                    var t1 = kvp.Key;
                    tileTheoreticalNeighbours[t1] = new List<(int tile, int edgeTypeKey, int edgeTypeVal)>();
                    foreach (var (edge, edgeType) in kvp.Value)
                    {
                        var otherTs = tByEdge[edge].Where(_ => _.tile != t1).ToList();
                        tileTheoreticalNeighbours[t1].AddRange(otherTs.Select(tpl => (tpl.tile, edgeType, tpl.edgeType)).ToList());
                    }
                }

                var sortedTNN = tileTheoreticalNeighbours.OrderBy(kvp => kvp.Value.Count).ToList();
                var corners = sortedTNN.Where(_ => _.Value.GroupBy(tpl => tpl.edgeTypeKey < 4).Select(grp => grp.Count()).Max() <= 2).ToList();
                long res1 = corners.Select(kvp => (long)kvp.Key).Aggregate((n1, n2) => n1 * n2);


                var cornersAndEdges = sortedTNN.Where(_ => _.Value.GroupBy(tpl => tpl.edgeTypeKey < 4).Select(grp => grp.Count()).Max() <= 3).ToList();

                // misunderstood
                //if (ih.P) // skip for T to chec ksth
                //{
                //    foreach (var kvp in cornersAndEdges)
                //    {
                //        var id = kvp.Key;
                //        edgesByT.Remove(id);
                //        foreach (var k in tByEdge.Keys)
                //        {
                //            tByEdge[k] = tByEdge[k].Where(tpl => tpl.tile != id).ToList();
                //        }
                //    }
                //}
            }


            int dimSize = (int)Math.Sqrt(tiles.Count);

            {
                var tileTheoreticalNeighbours = new Dictionary<int, List<(int tile, int edgeTypeKey, int edgeTypeVal)>>();
                foreach (var kvp in edgesByT)
                {
                    var t1 = kvp.Key;
                    tileTheoreticalNeighbours[t1] = new List<(int tile, int edgeTypeKey, int edgeTypeVal)>();
                    foreach (var (edge, edgeType) in kvp.Value)
                    {
                        var otherTs = tByEdge[edge].Where(_ => _.tile != t1).ToList();
                        tileTheoreticalNeighbours[t1].AddRange(otherTs.Select(tpl => (tpl.tile, edgeType, tpl.edgeType)).ToList());
                    }
                }
                

                var sortedTNN = tileTheoreticalNeighbours.OrderBy(kvp => kvp.Value.Count).ToList();
                
                var corners = sortedTNN.Where(_ => _.Value.GroupBy(tpl => tpl.edgeTypeKey < 4).Select(grp => grp.Count()).Max() <= 2).ToList();
                var edges = sortedTNN.Where(_ => _.Value.GroupBy(tpl => tpl.edgeTypeKey < 4).Select(grp => grp.Count()).Max() <= 3).Except(corners).ToList();

                Dictionary<int, ((int x, int y) pos, bool transposed, int rotation)> dictPosByTile = new();

                /* Rotations: (top-to-top)
                 *  0
                 * 3 1
                 *  2
                */
                // collapse first corner to 1 pair

                string outp2 = "output-t2.txt";
                string outp3 = "output-t3.txt";
                File.WriteAllText(outp2, "");
                File.WriteAllText(outp3, "");

                int currRow = -1;
                List<string> currRowT3 = new List<string>();

                void PrintOut(int row, List<string> str)
                {
                    File.AppendAllLines(outp2, new[] { "", });
                    File.AppendAllLines(outp2, str);

                    if (currRow != row)
                    {
                        File.AppendAllLines(outp3, currRowT3);
                        currRow = row;
                        currRowT3 = str.Select(_ => "").ToList();
                    }
                    
                    for(int i = 0; i < str.Count; i++)
                    {
                        currRowT3[i] += str[i];
                    }
                    
                }

                List<string> GetStrings(int c1, bool transpose, int rot)
                {

                    var strs = tilesById[c1].Select(chs => new string(chs.ToArray())).ToList();
                    if (transpose) { strs = strs.Select(_ => new string(_.Reverse().ToArray())).ToList(); }
                    //PrintOut(strs);
                    for (int r = 0; r < (4 - rot) % 4; r++)
                    {
                        var strs2 = new List<string>();
                        for (int i = 0; i < strs.Count; i++)
                        {
                            string ln = "";
                            for (int j = 0; j < strs[i].Length; j++)
                            {
                                ln += strs[j][strs.Count - 1 - i];
                            }
                            strs2.Add(ln);
                        }
                        strs = strs2;
                    }
                    return strs;
                }
                List<string> GetStrings2(List<string> strs, bool transpose, int rot)
                {
                    if (transpose) { strs = strs.Select(_ => new string(_.Reverse().ToArray())).ToList(); }
                    //PrintOut(strs);
                    for (int r = 0; r < (4 - rot) % 4; r++)
                    {
                        var strs2 = new List<string>();
                        for (int i = 0; i < strs.Count; i++)
                        {
                            string ln = "";
                            for (int j = 0; j < strs[i].Length; j++)
                            {
                                ln += strs[j][strs.Count - 1 - i];
                            }
                            strs2.Add(ln);
                        }
                        strs = strs2;
                    }
                    return strs;
                }

                string GetLeft(List<string> strs)
                {
                    return new string(strs.Select(_ => _[0]).ToArray());
                }
                string GetRight(List<string> strs)
                {
                    return new string(strs.Select(_ => _.Last()).ToArray());
                }
                string GetTop(List<string> strs)
                {
                    return strs.First();
                }
                string GetBot(List<string> strs)
                {
                    return strs.Last();
                }

                string lastEdgeLeft = "";
                string lastEdgeTop = "";
                {
                    var c1 = corners[0].Key; corners.RemoveAt(0);
                    var c1rots = //edgesByT[c1].Where(_ => _.edgeType < 4).OrderBy(_ => _.edgeType).ToList();
                        tileTheoreticalNeighbours[c1].Where(_ => _.edgeTypeKey < 4).OrderBy(_ => _.edgeTypeKey).ToList();
                    int rot = 0;
                    if (c1rots[0].edgeTypeKey == 0 && c1rots[1].edgeTypeKey == 3) { rot = 2; }
                    if (c1rots[0].edgeTypeKey == 0 && c1rots[1].edgeTypeKey == 1) { rot = 1; }
                    if (c1rots[0].edgeTypeKey == 1 && c1rots[1].edgeTypeKey == 2) { rot = 0; }
                    if (c1rots[0].edgeTypeKey == 2 && c1rots[1].edgeTypeKey == 3) { rot = 3; }
                    var transpose = false;

                    var strs= GetStrings(c1, transpose, rot);
                    lastEdgeTop = GetTop(strs);
                }

                for (int j = 0; j < dimSize; j++)
                {
                    for (int i = 0; i < dimSize; i++)
                    {
                        bool found = false;
                        int nextTile;
                        if (i == 0)
                        {
                            nextTile = tByEdge[lastEdgeTop]
                                .Where(_ => !dictPosByTile.ContainsKey(_.tile))
                                .Single()
                                .tile;



                            var nextTileOptions = new List<List<string>>();
                            foreach (var tr in new[] { false, true })
                            {
                                foreach (var rot in new[] { 0, 1, 2, 3 })
                                {
                                    var strs = GetStrings(nextTile, tr, rot);
                                    nextTileOptions.Add(strs);
                                }
                            }

                            foreach (var nto in nextTileOptions)
                            {
                                var l = GetTop(nto);
                                if (l == lastEdgeTop)
                                {
                                    lastEdgeLeft = GetRight(nto);
                                    lastEdgeTop = GetBot(nto);
                                    PrintOut(j, nto);
                                    found = true;
                                    dictPosByTile[nextTile] = ((0, 0), false, 0); // placeholder, not needed ..?
                                    break;
                                }
                            }
                        }
                        else
                        {
                            nextTile = tByEdge[lastEdgeLeft]
                                .Where(_ => !dictPosByTile.ContainsKey(_.tile))
                                .Single()
                                .tile;




                            var nextTileOptions = new List<List<string>>();
                            foreach (var tr in new[] { false, true })
                            {
                                foreach (var rot in new[] { 0, 1, 2, 3 })
                                {
                                    var strs = GetStrings(nextTile, tr, rot);
                                    nextTileOptions.Add(strs);
                                }
                            }

                            foreach (var nto in nextTileOptions)
                            {
                                var l = GetLeft(nto);
                                if (l == lastEdgeLeft)
                                {
                                    lastEdgeLeft = GetRight(nto);
                                    PrintOut(j, nto);
                                    found = true;
                                    dictPosByTile[nextTile] = ((0, 0), false, 0); // placeholder, not needed ..?
                                    break;
                                }
                            }
                        }

                        if (found == false)
                        {

                        }
                        else if (found == true)
                        {

                        }
                    }
                }


                PrintOut(-1, new[] { "" }.ToList());

                var finalTxt = File.ReadAllLines(outp3).ToList();
                // trim every 10th row
                finalTxt = finalTxt.Select((row, idx) => (row, idx)).Where((row, idx) => idx % 10 != 0 && idx % 10 != 9).Select(tpl => tpl.Item1).ToList();
                // trim every 10th col
                finalTxt = finalTxt.Select(ln => ln.Select((c, idx) => (c, idx)).Where((col, idx) => idx % 10 != 0 && idx % 10 != 9).Select(_ => _.c).FPipe(chars => new string(chars.ToArray()))).ToList();



                var seaMonster =
@"                  # 
#    ##    ##    ###
 #  #  #  #  #  #   ";

                var sm1 = "                  # ";
                var sm2 = "#    ##    ##    ###";
                var sm3 = " #  #  #  #  #  #   ";

                var iteratedTxt = finalTxt;
                List<string> currTxt = new List<string>();
                while (iteratedTxt != currTxt)
                {
                    currTxt = iteratedTxt;
                    var currTxtVariations = new List<List<string>>();

                    foreach (var tr in new[] { false, true })
                    {
                        foreach (var rot in new[] { 0, 1, 2, 3 })
                        {
                            var strs = GetStrings2(currTxt, tr, rot);
                            currTxtVariations.Add(strs);
                        }
                    }

                    int totalMonsters = 0;
                    foreach (var ftv in currTxtVariations)
                    {
                        Dictionary<(int x, int y), List<int>> dict = new Dictionary<(int x, int y), List<int>>();
                        for (int i = 0; i < ftv.Count; i++)
                        {
                            for (int x = 0; x <= ftv[i].Length - sm1.Length; x++)
                            {
                                var ln = ftv[i].Substring(x, sm1.Length);
                                foreach (var mc in Regex.Matches(ln, ".*(" + new string(sm1.Select(_ => _ == '#' ? "(#|O)" : ".").SelectMany(_ => _).ToArray()) + ").*").Cast<Match>())
                                {
                                    var tpl = (x, i);
                                    if (!dict.ContainsKey(tpl)) { dict[tpl] = new(); }
                                    dict[tpl].Add(1);
                                }

                                foreach (var mc in Regex.Matches(ln, ".*(" + new string(sm2.Select(_ => _ == '#' ? "(#|O)" : ".").SelectMany(_ => _).ToArray()) + ").*").Cast<Match>())
                                {
                                    var tpl = (x, i - 1);

                                    if (!dict.ContainsKey(tpl)) { dict[tpl] = new(); }
                                    dict[tpl].Add(2);
                                }

                                foreach (var mc in Regex.Matches(ln, ".*(" + new string(sm3.Select(_ => _ == '#' ? "(#|O)" : ".").SelectMany(_ => _).ToArray()) + ").*").Cast<Match>())
                                {
                                    var tpl = (x, i - 2);

                                    if (!dict.ContainsKey(tpl)) { dict[tpl] = new(); }
                                    dict[tpl].Add(3);
                                }
                            }
                        }
                        var monsters = dict.Where(_ => _.Value.Count == 3).ToList();
                        bool foundAny = false;
                        var iteratedTxtNext = ftv.ToList();
                        foreach (var pos in monsters.Select(_ => _.Key))
                        {
                            // replace monster by O
                            for(int i = 0; i < sm1.Length;i++)
                            {
                                var chars = iteratedTxtNext[pos.y].Select(_ => _).ToArray();
                                if (sm1[i] == '#' && char.ToUpper(chars[pos.x+i])!='O') { chars[pos.x + i] = chars[pos.x+i] switch { 'O' => 'O', '#' => 'O', '.' => 'o', 'o' => 'o' }; foundAny = true;  }
                                iteratedTxtNext[pos.y] = new string(chars);
                            }
                            for (int i = 0; i < sm2.Length; i++)
                            {
                                var chars = iteratedTxtNext[pos.y+1].Select(_ => _).ToArray();
                                if (sm2[i] == '#' && char.ToUpper(chars[pos.x + i]) != 'O') { chars[pos.x + i] = chars[pos.x + i] switch { 'O' => 'O', '#' => 'O', '.' => 'o', 'o' => 'o' }; foundAny = true; }
                                iteratedTxtNext[pos.y+1] = new string(chars);
                            }
                            for (int i = 0; i < sm3.Length; i++)
                            {
                                var chars = iteratedTxtNext[pos.y+2].Select(_ => _).ToArray();
                                if (sm3[i] == '#' && char.ToUpper(chars[pos.x + i]) != 'O') { chars[pos.x + i] = chars[pos.x + i] switch { 'O' => 'O', '#' => 'O', '.' => 'o', 'o' => 'o' }; foundAny = true; }
                                iteratedTxtNext[pos.y+2] = new string(chars);
                            }
                        }
                        if (foundAny) { iteratedTxt = iteratedTxtNext; }

                        totalMonsters += monsters.Count;
                    }
                }

                // 1792
                int res2 = iteratedTxt.SelectMany(_ => _).Where(_ => _ == '#').Count();
            }



        }
    }
}
