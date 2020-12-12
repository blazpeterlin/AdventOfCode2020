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
    class SolvedQuality
    {
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

                    if (ins == 'L') { dir = Moves.TurnDegreesCCW(num)(dir); }
                    if (ins == 'R') { dir = Moves.TurnDegreesCW(num)(dir); }
                }

                var res1 = Math.Abs(pos.Item1) + Math.Abs(pos.Item2);
            }


            {
                var wp = (10, 1);
                var pos = (0, 0);
                foreach (var (ins, num) in lns)
                {
                    if (ins == 'N') { wp = wp.Add((0, num)); }
                    if (ins == 'S') { wp = wp.Add((0, -num)); }
                    if (ins == 'W') { wp = wp.Add((-num, 0)); }
                    if (ins == 'E') { wp = wp.Add((num, 0)); }

                    if (ins == 'F') { pos = pos.Add(( wp.Item1*num, wp.Item2 * num)); }

                    if (ins == 'L') { wp = Moves.TurnDegreesCCW(num)(wp); }
                    if (ins == 'R') { wp = Moves.TurnDegreesCW(num)(wp); }
                }

                var res2 = Math.Abs(pos.Item1) + Math.Abs(pos.Item2);
            }
        }
    }
}
