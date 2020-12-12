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
                    pos = ins switch
                    {
                        'N' => pos.Add((0, num)),
                        'S' => pos.Add((0, -num)),
                        'W' => pos.Add((-num, 0)),
                        'E' => pos.Add((num, 0)),
                        'F' => pos.Add(dir.Multi(num)),
                        _ => pos
                    };

                    dir = ins switch
                    {
                        'L' => Moves.TurnDegreesCCW(num)(dir),
                        'R' => Moves.TurnDegreesCW(num)(dir),
                        _ => dir
                    };
                }

                var res1 = Math.Abs(pos.Item1) + Math.Abs(pos.Item2);
            }


            {
                var wp = (10, 1);
                var pos = (0, 0);
                foreach (var (ins, num) in lns)
                {
                    pos = ins switch
                    {
                        'F' => pos.Add(wp.Multi(num)),
                        _ => pos
                    };

                    wp = ins switch
                    {
                        'N' => wp.Add((0, num)),
                        'S' => wp.Add((0, -num)),
                        'W' => wp.Add((-num, 0)),
                        'E' => wp.Add((num, 0)),
                        'L' => Moves.TurnDegreesCCW(num)(wp),
                        'R' => Moves.TurnDegreesCW(num)(wp),
                        _ => wp,
                    };
                }

                var res2 = Math.Abs(pos.Item1) + Math.Abs(pos.Item2);
            }
        }
    }
}
