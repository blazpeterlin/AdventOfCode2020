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
    class Z3aoc2018_23_part2
    {
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
                File.ReadAllLines("input-z3.txt")
                .Select(ln => ln.FSplit("pos","=", "<", ",", " ", ">", "r"))
                .Select(tkns => tkns.Tuplify(int.Parse, int.Parse, int.Parse, int.Parse))
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
            
            // konstante
            var ZERO = ctx.MkNumeral(0, ctx.MkIntSort()) as ArithExpr;
            var ONE = ctx.MkNumeral(1, ctx.MkIntSort()) as ArithExpr;

            // funkcije, kalkulirane spr.
            Func<ArithExpr, ArithExpr> ZAbs = (ArithExpr x) => ctx.MkITE(ctx.MkLt(x, ZERO), ctx.MkUnaryMinus(x), x) as ArithExpr;
            var var_DistFromZero = ctx.MkIntConst("distFromZero");
            opt.Add(ctx.MkEq(var_DistFromZero, ctx.MkAdd(ZAbs(var_X), ZAbs(var_Y), ZAbs(var_Z))));

            Func<ArithExpr, ArithExpr, ArithExpr> ZDist = (ArithExpr x, ArithExpr y) => ZAbs(ctx.MkSub(x, y));
            Func<Claim, ArithExpr> Z_IsDroneInRange1else0 = (drone) =>
            {
                var droneX = ctx.MkNumeral(drone.X, ctx.MkIntSort()) as ArithExpr;
                var droneY = ctx.MkNumeral(drone.Y, ctx.MkIntSort()) as ArithExpr;
                var droneZ = ctx.MkNumeral(drone.Z, ctx.MkIntSort()) as ArithExpr;
                var droneRng = ctx.MkNumeral(drone.R, ctx.MkIntSort()) as ArithExpr;

                var manhRng = ctx.MkAdd(ZDist(var_X, droneX), ZDist(var_Y, droneY), ZDist(var_Z, droneZ));
                var isInRng = ctx.MkLe(manhRng, droneRng);
                return ctx.MkITE(isInRng, ONE, ZERO) as ArithExpr;
            };

            var countDronesInRange = claims.Select(cl => Z_IsDroneInRange1else0(cl)).ToList();
            var var_inRangeCount = ctx.MkIntConst("inRangeCount");
            opt.Add(ctx.MkEq(var_inRangeCount, ctx.MkAdd(countDronesInRange)));

            opt.MkMaximize(var_inRangeCount);
            opt.MkMinimize(var_DistFromZero);

            var status = opt.Check();
            var resModel = opt.Model;

            var res_distFromZero = resModel.Eval(var_DistFromZero);
        }
    }
}
