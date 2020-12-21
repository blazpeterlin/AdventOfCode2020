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

namespace d21
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

            var unionsPerAllergen = new Dictionary<string, List<List<string>>>();
            List<string> allIngs = new List<string>();
            foreach(var ln in lns)
            {
                var ings = ln.Split("(")[0].FSplit(' ').ToList();
                var agns = ln.FSplit("(contains", ")")[1].FSplit(",", " ").ToList();
                foreach (var agn in agns)
                {
                    if (!unionsPerAllergen.ContainsKey(agn)) { unionsPerAllergen[agn] = new(); }
                    unionsPerAllergen[agn].Add(ings);
                }
                allIngs.AddRange(ings);
            }
            var allIngsDistinct = allIngs.Distinct().ToList();

            var distinctUnionsPerAllergen = new Dictionary<string, List<string>>();
            foreach(var key in unionsPerAllergen.Keys)
            {
                distinctUnionsPerAllergen[key] = unionsPerAllergen[key].Aggregate((grp1, grp2) => grp1.Intersect(grp2).ToList());
            }

            var allergIngs = distinctUnionsPerAllergen.SelectMany(_ => _.Value).Distinct().FPipe(_ => new HashSet<string>(_));
            var res1 = allIngs.Where(ing => !allergIngs.Contains(ing)).Count();

            Dictionary<string, string> ingByAgn = new();
            while(distinctUnionsPerAllergen.Keys.Any())
            {
                var smallest = distinctUnionsPerAllergen.OrderBy(kvp => kvp.Value.Count).First();
                ingByAgn[smallest.Key] = smallest.Value.Single();
                distinctUnionsPerAllergen.Remove(smallest.Key);
                foreach(var remainingK in distinctUnionsPerAllergen.Keys)
                {
                    distinctUnionsPerAllergen[remainingK] = distinctUnionsPerAllergen[remainingK].Except(smallest.Value).ToList();
                }
            }
            var res2 = ingByAgn.OrderBy(_ => _.Key).Select(_ => _.Value).FPipe(_ => string.Join(",", _));
        }
    }
}
