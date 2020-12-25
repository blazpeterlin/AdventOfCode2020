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

namespace d25
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
        //.Select(tkns => tkns.Tuplify(AsInt2, AsChar2, AsString2)) // int, char, string
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

            long transform(long loopSize, long origNum, long factor)
            {
                //int loopSize = 123;
                //long origNum = 234;
                long num = origNum;
                for (long i = 0; i < loopSize; i++)
                {
                    num *= factor;
                    num = num % 20201227;
                }
                return num;
            }

            long num1 = lns[0].AsLong();
            long num2 = lns[1].AsLong();
            long num = 1;

            long loopSize1 = -1;

            for (long i = 1; i < long.MaxValue; i++)
            {
                num = transform(1, num, 7);
                if (num == num1)
                {
                    loopSize1 = i;
                    break;
                }
            }

            var res1 = transform(loopSize1, 1, num2);
        }
    }
}
