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

namespace d14
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
            var grps = ih.AsLines().Split("").ToList();

            {
                Dictionary<int, long> finalNums = new();

                foreach (var grp in grps)
                {
                    var mask = grp.First().Split(" ").Last();//.Select((num, idx) => (num, idx)).ToList();
                    var maskOR = Convert.ToInt64(new string(mask.Select(x => x switch { '1' => '1', _ => '0' }).ToArray()), 2);
                    var maskAND = Convert.ToInt64(new string(mask.Select(x => x switch { '0' => '0', _ => '1' }).ToArray()), 2);

                    foreach (var ln in grp.Skip(1))
                    {
                        var idx = ln.Split('[', ']')[1].AsInt();
                        var num = ln.Split(' ').Last().AsLong();

                        finalNums[idx] = num & maskAND | maskOR;
                    }
                }

                var res1 = finalNums.Values.ToList().Sum();
            }

            {
                Dictionary<long, long> finalNums = new();

                foreach (var grp in grps)
                {
                    string mask = grp.First().Split(" ").Last();//.Select((num, idx) => (num, idx)).ToList();

                    long maskOR = Convert.ToInt64(new string(mask.Select(x => x switch { '1' => '1', _ => '0' }).ToArray()), 2);
                    long maskAND = Convert.ToInt64(new string(mask.Select(x => x switch { '0' => '1', 'X' => '0', _ => '1' }).ToArray()), 2);

                    var idxX = mask.Reverse().Select((num, idx) => (num, idx)).Where(tpl => tpl.Item1 == 'X').ToList();

                    List<long> maskOR2 = new List<long>();
                    foreach (var comb in idxX.Select(_ => _.Item2).Subsets())
                    { 
                        long combNum = comb.Aggregate((long)0, (a, b) => { long x = ((long)1 << b); return a + x; });
                        maskOR2.Add(combNum);
                    }


                    foreach (var ln in grp.Skip(1))
                    {
                        long idx = ln.Split('[', ']')[1].AsLong();
                        long num = ln.Split(' ').Last().AsLong();


                        List<long> writingTo = new List<long>();
                        foreach (long mor in maskOR2)
                        {
                            var idxActual = (idx | maskOR) & maskAND;
                            idxActual = idxActual | mor;
                            writingTo.Add(idxActual);
                        }

                        foreach(long wt in writingTo)
                        {
                            finalNums[wt] = num;
                        }
                    }
                }

                // not 2083108314262
                var res2 = finalNums.Values.ToList().Sum();
            }
        }
    }
}
