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
using System.IO;
using Microsoft.Z3;

namespace experiments
{ 
    class Z3
    {
        // Reminders:
        // AsInt
        // Graph<(int, int)>.From2dWithMoves(pts, Moves.PLUS).Dijkstra((0,0),(3,1)).Path();
        // ih.ModifyLines(ln => { if (ln == "") return "-"; else return ln; });

        record Claim
        {
            public int X;
            public int Y;
            public int Z;
            public int R;
        }

        public static void Solve()
        {
            var claims =
                File.ReadAllLines("input-z3-t.txt")
                .Select(ln => ln.FSplit("pos","=", "<", ",", " ", ">", "r"))
                .Select(tkns => tkns.Tuplify(AsInt, AsInt, AsInt, AsInt))
                .Select(tpl => new Claim { X = tpl.t1, Y = tpl.t2, Z = tpl.t3, R = tpl.t4 })
                .ToList();

            Dictionary<string, string> z3settings = new ();
            z3settings["model"] = "true";

            var ctx = new Context(z3settings);
            var opt = ctx.MkOptimize();

            // proste spremenljivke
            var var_X = ctx.MkIntConst("x");
            var var_Y = ctx.MkIntConst("y");
            var var_Z = ctx.MkIntConst("z");

            var ZERO = ctx.MkNumeral(0, ctx.MkIntSort()) as ArithExpr;
            var ONE = ctx.MkNumeral(1, ctx.MkIntSort()) as ArithExpr;

            Func<ArithExpr, ArithExpr> ZAbs = (ArithExpr x) => ctx.MkITE(ctx.MkLt(x, ZERO), ctx.MkUnaryMinus(x), x) as ArithExpr;

        }
    }
}
