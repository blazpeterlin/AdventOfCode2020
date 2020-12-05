using AOC.Common;
using System;
using System.Linq;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;
using System.Collections.Generic;

namespace d05
{
    class Solved6AM
    {
        public static void Solve()
        {
            var ih = InputHelper.LoadInput(2020);
            var lns = ih.AsLines();
            var txt = ih.AsText();
            //var tkns = ih.AsTokens<int>();
            var chch = ih.AsCharListOfLists();


            //.Select(ln => ln.FSplit("-", " ", ":"))
            //.Select(tkns => tkns.Tuplify(AsInt, AsChar, AsString))
            //.Where(t => t.t1 < t.t2)


            var planeSeats = new List<PlaneSeat>();
            foreach(var ln in lns)
            {
                var ps = new PlaneSeat();
                foreach (var ch in ln)
                {
                    switch (ch)
                    {
                        case 'F': ps.F(); break;
                        case 'B': ps.B(); break;
                        case 'L': ps.L(); break;
                        case 'R': ps.R(); break;
                    }
                }
                planeSeats.Add(ps);
            }
            //var ps = lns.Select(ln =>
            //{
            //});

            int res1 = planeSeats.Max(_ => _.Id);

            var hs = new HashSet<int>(planeSeats.Select(_ => _.Id));
            var hsSeats = new HashSet<(int,int)>(planeSeats.Select(_ => (_.MinX, _.MinY)));
            for (int y = 0; y <= 127; y++)
            {
                for(int x = 0; x <= 7; x++)
                {
                    var ps = new PlaneSeat { MinX = x, MinY = y };
                    if (ps.MinY > 0 && ps.MinY < 127 && hs.Contains(ps.Id - 1) && hs.Contains(ps.Id + 1) && !hsSeats.Contains((ps.MinX,ps.MinY)))
                    {
                        var res2 = ps.Id;
                    }
                }
            }

        }

        class PlaneSeat
        {
            public PlaneSeat()
            {
                MaxX = 7;
                MaxY = 127;
            }

            public int MinX;
            public int MaxX;
            public int MinY;
            public int MaxY;

            public void L()
            {
                MaxX = MinX + (MaxX - MinX) / 2;
            }

            public void R()
            {
                MinX = MinX + (MaxX - MinX) / 2 + 1;
            }
            public void B()
            {
                MinY = MinY + (MaxY - MinY) / 2 + 1;
            }
            public void F()
            {
                MaxY = MinY + (MaxY - MinY) / 2;
            }

            public int Id => MinY * 8 + MinX;
        }
    }
}
