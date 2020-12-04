using AOC.Common;
using System;
using System.Linq;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;

namespace d05
{
    class Program
    {
        // Reminders:
        // AsInt
        // Graph<(int, int)>.From2dWithMoves(pts, Moves.PLUS).Dijkstra((0,0),(3,1)).Path();
        // ih.ModifyLines(ln => { if (ln == "") return "-"; else return ln; });
        static void Main(string[] args)
        {
            var ih = InputHelper.LoadInput(2020);
            var lns = ih.AsLines();
            var txt = ih.AsText();
            var tkns = ih.AsTokens<int>();
            var chch = ih.AsCharListOfLists();


            //.Select(ln => ln.FSplit("-", " ", ":"))
            //.Select(tkns => tkns.Tuplify(AsInt, AsChar, AsString))
            //.Where(t => t.t1 < t.t2)

        }
    }
}
