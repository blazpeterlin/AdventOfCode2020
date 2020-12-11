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

namespace d10
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
            //var lns = ih.AsLines().Select(AsInt).OrderBy(_ => _).ToList();
            var lns = ih.AsLines().Select(_ => _.AsInt()).OrderBy(_ => _).ToList();
            lns.Insert(0, 0);

            var j1 = lns.Window(2).Where(lst => lst[1] - lst[0] == 1).Count();
            var j3 = lns.Window(2).Where(lst => lst[1] - lst[0] == 3).Count() + 1;

            var res1 = j1 * j3;

            var max = lns.Max() + 3;
            //lns.RemoveAt(0);

            var dynCache = new Dictionary<int, long>();
            dynCache[max] = 1;
            for(int idx = lns.Count-1;idx>=0;idx--)
            {
                DynNum(dynCache, lns, idx);
            }

            var res2 = dynCache[0];
        }

        private static void DynNum(Dictionary<int, long> dynCache, List<int> js, int idx)
        {
            var j = js[idx];
            long res = 0;
            if (dynCache.TryGetValue(j+3, out var num3)) { res += num3; }
            if (dynCache.TryGetValue(j + 2, out var num2)) { res += num2; }
            if (dynCache.TryGetValue(j + 1, out var num1)) { res += num1; }
            dynCache[j] = res;
        }
    }
}
