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

namespace d05
{
    class SolvedFast
    {
        // Reminders:
        // AsInt
        // Graph<(int, int)>.From2dWithMoves(pts, Moves.PLUS).Dijkstra((0,0),(3,1)).Path();
        // ih.ModifyLines(ln => { if (ln == "") return "-"; else return ln; });
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2017);
            var lns = ih.AsLines();
            //var txt = ih.AsText();
            //var tkns = ih.AsTokens<int>();
            //var chch = ih.AsCharListOfLists();

            //.Select(tkns => tkns.Tuplify(AsInt, AsChar, AsString))
            //.Where(t => t.t1 < t.t2)

            var nums = lns.Select(AsInt).ToList();
            Dictionary<int, int> dictN = new Dictionary<int, int>();
            for(int i = 0; i < nums.Count; i++)
            {
                dictN[i] = nums[i];
            }

            int idx = 0;
            int numSteps = 0;
            while(idx >= 0 && idx < nums.Count)
            {
                var newIdx = idx + dictN[idx];
                if (dictN[idx] >= 3)
                {
                    dictN[idx]--;
                }
                else
                {
                    dictN[idx]++;
                }
                idx = newIdx;
                numSteps++;
            }

        }
    }
}
