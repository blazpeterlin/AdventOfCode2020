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

namespace d00
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
        //.Select(tkns => tkns.Tuplify(AsInt, AsChar, AsString))
        //.Where(t => t.t1 < t.t2)

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
            var lns = ih.AsLines().Select(ln => ln.AsLong()).ToList();

            var sum2s = new Dictionary<long,int>();
            var q = new Dictionary<long, List<long>>();

            int numTake = ih.P ? 25 : 5;

            var lns25 = lns.Take(numTake).ToList();
            for(int i = 0; i < lns25.Count;i++)
            {
                q[i] = q.GetValueOrDefault(i, new List<long>());
                for (int j = i+1; j < lns25.Count; j++)
                {
                    var sum = lns25[i] + lns25[j];
                    sum2s[sum] = sum2s.GetValueOrDefault(sum, 0)+1;
                    q[i].Add(sum);
                }
            }

            long res1 = -1;
            int res1idx = -1;

            var lnsRemaining = lns.Skip(numTake).ToList();
            for(int i = numTake; i < lns.Count; i++)
            {
                q[i] = q.GetValueOrDefault(i, new List<long>());
                var num = lns[i];
                if (!sum2s.ContainsKey(num))
                {
                    res1 = num;
                    res1idx = i;
                    break;
                }

                foreach(var sum in q[i-numTake])
                {
                    sum2s[sum] = sum2s[sum] - 1;
                    if (sum2s[sum] == 0)
                    {
                        sum2s.Remove(sum);
                    }
                }

                for(int j = i-numTake; j < i; j++)
                {
                    var sum = lns[j] + lns[i];
                    sum2s[sum] = sum2s.GetValueOrDefault(sum, 0) + 1;
                    q[j].Add(sum);
                }
            }


            // res2
            int minIdx = 0; int maxIdx = -1;
            long res2sum = 0;
            for(int i = 0; i < res1idx; i++)
            {
                res2sum += lns[i];
                maxIdx = i;
                if (res2sum > res1) { break; }
            }

            for (int i = maxIdx + 1; i <= res1idx; i++)
            {
                while(res2sum > res1)
                {
                    res2sum -= lns[minIdx];
                    minIdx++;
                }

                if (res2sum == res1) { break; }

                res2sum += lns[i];
                maxIdx = i;

                if (res2sum == res1) { break; }
            }

            var res2nums = lns.Skip(minIdx-1).Take(maxIdx-minIdx+1);
            var res2 = res2nums.Min() + res2nums.Max();
        }
    }
}
