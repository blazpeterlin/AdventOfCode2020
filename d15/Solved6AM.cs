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

namespace d15
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
            var lns = ih.AsText().Split(',').Select(int.Parse).ToList();

            {
                Dictionary<int, int> d = new Dictionary<int, int>();
                Dictionary<int, int> dPrev = new Dictionary<int, int>();
                Dictionary<int, int> d2 = new Dictionary<int, int>();
                for (int i = 0; i < lns.Count(); i++)
                {
                    dPrev = new Dictionary<int, int>(d);
                    d[lns[i]] = i;
                    d2[i] = lns[i];
                }
                for (int i = lns.Count(); i < 2020; i++)
                {
                    int newNum;
                    if (dPrev.TryGetValue(d2[i - 1], out var step)) { newNum = i - step - 1; } else { newNum = 0; }
                    // i regret nothing
                    dPrev = new Dictionary<int, int>(d);
                    d[newNum] = i;
                    d2[i] = newNum;
                }
                var res1 = d2[2019];
            }


            {
                Dictionary<long, Queue<long>> d = new Dictionary<long, Queue<long>>();
                Dictionary<long, long> d2 = new Dictionary<long, long>();
                for (int i = 0; i < lns.Count(); i++)
                {
                    if (!d.ContainsKey(lns[i])) { d[lns[i]] = new Queue<long>(); }
                    var q = d[lns[i]];
                    q.Enqueue(i);
                    if (q.Count > 2) { q.Dequeue(); }
                    
                    d2[i] = lns[i];
                }
                for (long i = lns.Count(); i < 30000000; i++)
                {
                    long newNum;
                    if (d.TryGetValue(d2[i - 1], out var steps) && steps.First() < i - 1) { newNum = i - steps.First() - (long)1; } else { newNum = 0; }



                    if (!d.ContainsKey(newNum)) { d[newNum] = new Queue<long>(); }
                    var q = d[newNum];
                    q.Enqueue(i);
                    if (q.Count > 2) { q.Dequeue(); }
                    

                    d2[i] = newNum;
                }

                var res2 = d2[29999999];
            }
             
        }
    }
}

