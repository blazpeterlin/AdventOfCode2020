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

namespace d23
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
            var nums = ih.AsLines()[0].Select(_ => int.Parse("" + _)).ToList();

            {
                var sortedNums = nums.OrderBy(_ => _).ToList();
                LinkedList<int> ll = new LinkedList<int>(nums);

                int numMoves = ih.P ? 100 : 100;
                var currentCup = nums.First();

                int GetSmallerNum(int num)
                {
                    var idx = sortedNums.IndexOf(num);
                    if (idx == 0) { idx = sortedNums.Count; }
                    idx--;
                    return sortedNums[idx];
                }

                var cc = ll.Find(currentCup);
                for (int i = 0; i < numMoves; i++)
                {
                    var next1 = cc.Next ?? ll.First;
                    var next2 = next1.Next ?? ll.First;
                    var next3 = next2.Next ?? ll.First;
                    ll.Remove(next1); ll.Remove(next2); ll.Remove(next3);

                    var smallerC = GetSmallerNum(cc.Value);
                    while (smallerC == next1.Value || smallerC == next2.Value || smallerC == next3.Value)
                    {
                        smallerC = GetSmallerNum(smallerC);
                    }

                    var llsc = ll.Find(smallerC);
                    ll.AddAfter(llsc, next3); ll.AddAfter(llsc, next2); ll.AddAfter(llsc, next1);

                    cc = cc.Next ?? ll.First;


                    {
                        var cc1 = ll.First;
                        var firstVal = cc1.Value;
                        string res1 = "" + cc1.Value;
                        while ((cc1.Next ?? ll.First).Value != firstVal)
                        {
                            cc1 = cc1.Next ?? ll.First;
                            res1 += cc1.Value;
                        }
                        Console.WriteLine(res1);
                    }
                }

                {
                    var cc1 = ll.Find(1);
                    string res1 = "";
                    while ((cc1.Next ?? ll.First).Value != 1)
                    {
                        cc1 = cc1.Next ?? ll.First;
                        res1 += cc1.Value;
                    }
                }

            }


            {
                int maxNum = 1000000;
                int numMoves = 10000000;

                for (int i = nums.Count+1; i <= maxNum; i++) { nums.Add(i); }

                LinkedList<int> ll = new LinkedList<int>(nums);
                
                var currentCup = nums.First();

                int GetSmallerNum(int num)
                {
                    var res = num - 1;
                    if (res < 1) { res = maxNum; }
                    return res;
                }

                Dictionary<int, LinkedListNode<int>> nodeByVal = new();
                for(var node = ll.First; node != null; node = node.Next)
                {
                    nodeByVal[node.Value] = node;
                }

                var cc = ll.Find(currentCup);
                for (int i = 0; i < numMoves; i++)
                {
                    var next1 = cc.Next ?? ll.First;
                    var next2 = next1.Next ?? ll.First;
                    var next3 = next2.Next ?? ll.First;
                    ll.Remove(next1); ll.Remove(next2); ll.Remove(next3);

                    var smallerC = GetSmallerNum(cc.Value);
                    while (smallerC == next1.Value || smallerC == next2.Value || smallerC == next3.Value)
                    {
                        smallerC = GetSmallerNum(smallerC);
                    }

                    //var llsc = ll.Find(smallerC);
                    var llsc = nodeByVal[smallerC];
                    ll.AddAfter(llsc, next3); ll.AddAfter(llsc, next2); ll.AddAfter(llsc, next1);

                    cc = cc.Next ?? ll.First;


                    //{
                    //    var cc1 = ll.First;
                    //    var firstVal = cc1.Value;
                    //    string res1 = "" + cc1.Value;
                    //    while ((cc1.Next ?? ll.First).Value != firstVal)
                    //    {
                    //        cc1 = cc1.Next ?? ll.First;
                    //        res1 += cc1.Value;
                    //    }
                    //    Console.WriteLine(res1);
                    //}
                }

                {
                    var cc1 = ll.Find(1);
                    long res2;
                    var next1 = cc1.Next ?? ll.First;
                    var next2 = next1.Next ?? ll.First;
                    res2 = (long)next1.Value * (long)next2.Value;
                }

            }
        }
    }
}
