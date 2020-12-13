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
using System.Numerics;

namespace d13
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
            var lns = ih.AsLines().Skip(1).Select(str =>str.FSplit(",")).SelectMany(_ => _).Where(_ =>_!="x").Select(int.Parse);
            var st = ih.AsLines().First().FPipe(int.Parse);

            var fst = lns.OrderBy(bus => st - (st % bus)).First();
            var res1 = (Math.Abs(fst - (st % fst))) * fst;

            var lns2 = ih.AsLines().Skip(1).Select(str => str.FSplit(",")).SelectMany(_ => _).Select((str,idx) => (str, idx))
                .Where(_ => _.Item1 != "x").Select(tpl => (long.Parse(tpl.Item1), tpl.Item2)).ToList();

            //// this failed spectacularly
            //new Z3().Solve(lns2);

            BigInteger basic = lns2.First().Item1;
            BigInteger increment = basic;
            int currentWorking = 1;
            BigInteger res2;
            for (BigInteger x = basic; ; x += increment)
            {
                var ln = lns2[currentWorking];
                if ((x+ln.Item2)%ln.Item1 == 0)
                {
                    increment *= ln.Item1;
                    currentWorking++;
                }

                if (currentWorking == lns2.Count) { res2 = x; break; }
            }
        }
    }
}
