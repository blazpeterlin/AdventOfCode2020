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

namespace d12
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
            var lns = ih.AsLines().Select(ln => (ln.First(), int.Parse(new string(ln.Skip(1).ToArray())))).ToList();

            {
                var dir = (1, 0);
                var pos = (0, 0);
                foreach (var (ins, num) in lns)
                {
                    if (ins == 'N') { pos = pos.Add((0, num)); }
                    if (ins == 'S') { pos = pos.Add((0, -num)); }
                    if (ins == 'W') { pos = pos.Add((-num, 0)); }
                    if (ins == 'E') { pos = pos.Add((num, 0)); }

                    if (ins == 'F') { pos = pos.Add((dir.Item1 * num, dir.Item2 * num)); }

                    if (ins == 'L') { if (num == 90) { dir = Moves.TurnLeft(dir); } else if (num == 270) { dir = Moves.TurnRight(dir); } else if (num == 180) { dir = (-dir.Item1, -dir.Item2); } }
                    if (ins == 'R') { if (num == 90) { dir = Moves.TurnRight(dir); } else if (num == 270) { dir = Moves.TurnLeft(dir); } else if (num == 180) { dir = (-dir.Item1, -dir.Item2); } }
                    //if (ins == 'R') { dir = Moves.TurnRight(dir); }

                    //Console.WriteLine(pos.ToString() + "  |  " + dir.ToString());
                }

                var res1 = Math.Abs(pos.Item1) + Math.Abs(pos.Item2);
            }

            var wp = (10, 1);

            {
                //var dir = (1, 0);
                var pos = (0, 0);
                foreach (var (ins, num) in lns)
                {
                    if (ins == 'N') { wp = wp.Add((0, num)); }
                    if (ins == 'S') { wp = wp.Add((0, -num)); }
                    if (ins == 'W') { wp = wp.Add((-num, 0)); }
                    if (ins == 'E') { wp = wp.Add((num, 0)); }

                    if (ins == 'F') { pos = pos.Add(( wp.Item1*num, wp.Item2 * num)); }

                    if (ins == 'L') { if (num == 90) { wp = Moves.TurnLeft(wp); } else if (num == 270) { wp = Moves.TurnRight(wp); } else if (num == 180) { wp = (-wp.Item1, -wp.Item2); } }
                    if (ins == 'R') { if (num == 90) { wp = Moves.TurnRight(wp); } else if (num == 270) { wp = Moves.TurnLeft(wp); } else if (num == 180) { wp = (-wp.Item1, -wp.Item2); } }
                    //if (ins == 'R') { dir = Moves.TurnRight(dir); }

                    //Console.WriteLine(pos.ToString() + "  |  " + wp.ToString());
                }

                var res2 = Math.Abs(pos.Item1) + Math.Abs(pos.Item2);
            }
        }
    }
}
