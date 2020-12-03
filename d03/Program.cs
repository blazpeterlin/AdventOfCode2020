using AOC.Common;
using System;
using System.Linq;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;
using System.Reflection;

namespace d03
{
    class Program
    {
        //int test = (a, b) switch
        //{
        //    (0, "a") => 2,
        //    (0, _) => 0,
        //    (1, _) => 1,
        //    _ => -1
        //};

        static void Main(string[] args)
        {
            var lns = InputHelper.LoadInput(2020).AsCharListOfLists();
            //var txt = InputHelper.LoadInput(2020).AsText();
            //var tkns = InputHelper.LoadInput(2020).AsTokens<int>();


            //.Select(ln => ln.FSplit("-", " ", ":"))
            //.Select(tkns => tkns.Tuplify(AsInt, AsChar, AsString))
            //.Where(t => t.t1 < t.t2)

            int cntr3 = 0;
            for(int i = 0; i < lns.Count; i++)
            {
                if (lns[i][(i * 3) % lns[0].Count] == '#') { cntr3++; }
            }

            int cntr1 = 0;
            for (int i = 0; i < lns.Count; i++)
            {
                if (lns[i][(i * 1) % lns[0].Count] == '#') { cntr1++; }
            }
            int cntr5= 0;
            for (int i = 0; i < lns.Count; i++)
            {
                if (lns[i][(i *5) % lns[0].Count] == '#') { cntr5++; }
            }
            int cntr7= 0;
            for (int i = 0; i < lns.Count; i++)
            {
                if (lns[i][(i * 7) % lns[0].Count] == '#') { cntr7++; }
            }
            int cntr12 = 0;
            for (int i = 0; i < lns.Count; i++)
            {
                if (i % 2 == 1) { continue; }
                if (lns[i][(i/2) % lns[0].Count] == '#') { cntr12++; }
            }
            var res1 = cntr3;
            var res2 = cntr1 * cntr3 * cntr5 * cntr7 * cntr12;
        }
    }
}
