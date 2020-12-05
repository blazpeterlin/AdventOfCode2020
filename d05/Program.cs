using AOC.Common;
using System;
using System.Linq;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;
using System.Collections.Generic;
using static System.Linq.Enumerable;

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
            Solved6AM.Solve();
            SolvedQuality.Solve();
        }
    }
}
