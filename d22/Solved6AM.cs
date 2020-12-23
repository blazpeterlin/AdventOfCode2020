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

namespace d22
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
            var lns = ih.AsLines();

            

            {
                var decks = lns.Split("").Select(grp => grp.Skip(1).Select(ln => ln.AsInt()).ToList()).ToList();
                var q1 = new Queue<int>(decks[0]);
                var q2 = new Queue<int>(decks[1]);
                while (q1.Any() && q2.Any())
                {
                    var i1 = q1.Dequeue();
                    var i2 = q2.Dequeue();
                    if (i1 > i2)
                    {
                        q1.Enqueue(i1); q1.Enqueue(i2);
                    }
                    else
                    {
                        q2.Enqueue(i2); q2.Enqueue(i1);
                    }
                }

                long res1 = 0;
                var q = q1.Any() ? q1 : q2;
                int step = 1;
                var ql = q.ToList();
                ql.Reverse();
                foreach (var elt in ql)
                {
                    res1 += step * elt;
                    step++;
                }

                // res1 solved here
            }

            

            {
                var decks = lns.Split("").Select(grp => grp.Skip(1).Select(ln => ln.AsInt()).ToList()).ToList();


                var q1 = new Queue<int>(decks[0]);
                var q2 = new Queue<int>(decks[1]);

                var (resNum,resQ) = SubGame(q1, q2, false);

                long res2 = 0;
                var q = resQ;
                int step = 1;
                var ql = q.ToList();
                ql.Reverse();
                foreach (var elt in ql)
                {
                    res2 += step * elt;
                    step++;
                }
                // res2 solved here


                // not 33054
                // 32949
            }

        }

        static string Descript(Queue<int> q)
        {
            return string.Join(",", q);
        }

        static (int, Queue<int>) SubGame(Queue<int> q1, Queue<int> q2, bool isSubGame)
        {
            //if (isSubGame)
            //{
            //    var nums = q1.ToList();
            //    nums.AddRange(q2);
            //    {
            //        var largestNum = nums.Max();
            //        var cnt = nums.Where(_ => _ == largestNum).Count();
            //        if (cnt == 1 && largestNum > nums.Count && q1.Contains(largestNum))
            //        {
            //            return q1.Contains(largestNum) ? (1, q1) : (2, q2);
            //        }
            //    }
            //}


            var hs = new HashSet<(string, string)>();

            while (q1.Any() && q2.Any())
            {
                var (s1, s2) = (Descript(q1), Descript(q2));
                if (hs.Contains((s1, s2)))
                {
                    return (1,q1);
                }
                hs.Add((s1, s2));
                var i1 = q1.Dequeue();
                var i2 = q2.Dequeue();

                int res = i1 > i2 ? 1 : 2;
                if (q1.Count >= i1 && q2.Count >= i2)
                {
                    (res,_) = SubGame(new Queue<int>(q1.Take(i1)), new Queue<int>(q2.Take(i2)), true);
                }

                if (res == 1)
                {
                    q1.Enqueue(i1); q1.Enqueue(i2);
                }
                else
                {
                    q2.Enqueue(i2); q2.Enqueue(i1);
                }
            }

            return q1.Any() ? (1,q1) : (2,q2);
           
        }
    }
}
