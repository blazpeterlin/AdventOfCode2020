using AOC.Common;
using System;
using System.Linq;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;
using System.Collections.Generic;
using static System.Linq.Enumerable;
using static AOC.Common.Func;
using static AOC.Common.Helpers;

namespace d05
{
    class SolvedQuality
    {
        public static (long res1, long res2) Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var lns = ih.AsLines();

            int maxX = 7, maxY = 127;

            var planeSeats = lns.Select(ln =>
            {
                var ps = new PlaneSeat { MaxX= maxX, MaxY= maxY };
                foreach (var ch in ln)
                {
                    ps = ch switch
                    {
                        'L' => ps with { MaxX = ps.MinX + (ps.MaxX - ps.MinX) / 2 },
                        'R' => ps with { MinX = ps.MinX + (ps.MaxX - ps.MinX) / 2 + 1 },
                        'F' => ps with { MaxY = ps.MinY + (ps.MaxY - ps.MinY) / 2 },
                        'B' => ps with { MinY = ps.MinY + (ps.MaxY - ps.MinY) / 2 + 1 },
                        _ => throw new Exception("Unknown char"),
                    };
                }
                return ps;
            });

            int res1 = planeSeats.Max(_ => _.Id);

            HashSet<int> hsIds = new (planeSeats.Select(_ => _.Id));
            HashSet<(int, int)> hsSeats = new (planeSeats.Select(_ => (_.MinY, _.MinX)));

            var res2seat =
                FRng(1, maxY - 1)
                .FRng(0, maxX)
                .Where(t => !hsSeats.Contains(t))
                .Select(t => new PlaneSeat { MinY = t.fst, MaxY = t.fst, MinX = t.snd, MaxX = t.snd, })
                .Where(ps => hsIds.Contains(ps.Id - 1) && hsIds.Contains(ps.Id + 1))
                .Single();
            int res2 = res2seat.Id;

            var dictMap = 
                FRng(0, maxY)
                .FRng(0, maxX)
                .ToDictionary(tpl => tpl, tpl => hsSeats.Contains(tpl) ? CHAR_BLOCK : ((res2seat.MinY, res2seat.MinX) == tpl ? '#' : ' '));
            dictMap.PrintScreenDictYX(' ');

            return (res1, res2);
        }

        record PlaneSeat
        {
            public int MinX;
            public int MaxX;
            public int MinY;
            public int MaxY;

            public int Id
            {
                get
                {
                    if (MinX != MaxX) { throw new Exception("Plane seat X not affixed"); }
                    if (MinY != MaxY) { throw new Exception("Plane seat Y not affixed"); }
                    var x = MinX;
                    var y = MinY;
                    return y * 8 + x;
                }
            }
        }
    }
}
