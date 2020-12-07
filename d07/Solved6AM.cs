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

namespace d07
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

        // var x = FRng(10, 20).Select((x, idx) => idx).ToList();
        // FRng(10,20).Permutations(6,false)
        public static void Solve()
        {
            

            var ih = InputHelper.LoadInputP(2020);
            var lns = ih.AsLines();

            Dictionary<string, List<(long num, string typeBag)>> dictHas = new();
            Dictionary<string, List<string>> dictContains = new();
            foreach (var ln in lns)
            {
                if (ln.Contains("no other")) { continue; }
                List<string> tkns = ln.Split(new[] { " ", "bags", "contain", "; ", ".","," ,"bag"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                var src = tkns[0] + " " + tkns[1];
                var dsts = new List<(long num, string typeBag)>();
                for (long i = 2; i < tkns.Count; i+=3)
                {
                    string typ = tkns[(int)i + 1] + " " + tkns[(int)i + 2];
                    dsts.Add((AsLong(tkns[(int)i]), typ));
                    if (!dictContains.ContainsKey(typ)) { dictContains[typ] = new List<string>(); }
                    dictContains[typ].Add(src);
                }
                dictHas[src] = dsts;
            }

            var shinysContainers = new[] { "shiny gold" }.ToList();
            while(true)
            {
                var cnt1 = shinysContainers.Count;
                shinysContainers = shinysContainers.Union(shinysContainers.SelectMany(_ => dictContains.TryGetValue(_, out var val) ? val : new())).ToList();
                var cnt2 = shinysContainers.Count;
                if (cnt1 == cnt2) { break; }
            }

            var res1 = shinysContainers.Count - 1;



            var bagsToCount = new List<(long,string)> { (1, "shiny gold") }.ToList();
            long shinyContentsCount = 0;
            while (true)
            {
                var bagsToCountPrev = bagsToCount.ToList();
                bagsToCount.Clear();
                foreach(var btc in bagsToCountPrev)
                {
                    shinyContentsCount += btc.Item1;
                    if (dictHas.TryGetValue(btc.Item2, out var contained))
                    {
                        foreach(var c in contained)
                        {
                            bagsToCount.Add((btc.Item1 * c.num, c.typeBag));
                        }
                    }
                    //else
                    //{
                    //    totalCount += btc.Item1;
                    //}
                }
                //bagsToCount = bagsToCount.SelectMany(_ => dictHas.TryGetValue(_, out var val) ? val : new()).ToList();
                var cnt2 = bagsToCount.Count;
                if (cnt2==0) { break; }
            }

            var res2 = shinyContentsCount - 1;
        }
    }
}
