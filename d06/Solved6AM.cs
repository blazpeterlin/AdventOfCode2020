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

namespace d06
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
        public static void Solve()
        {
            int a = 1;
            var ih = InputHelper.LoadInputP(2020);
            var lns = ih.AsLines().Split("");

            var sum = 0;
            foreach(var grp in lns)
            {
                var ch = string.Join("", grp).Distinct().Count();
                sum += ch;
            }

            var sum2 = 0;
            foreach (var grp in lns)
            {
                var chs = string.Join("", grp).Distinct();
                var goodCh = chs.Where(ch => grp.All(g => g.Contains(ch))).Count();
                sum2 += goodCh;
            }

            var res1 = sum;
            var res2 = sum2;
        }
    }
}
