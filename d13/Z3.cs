using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d13
{
    class Z3
    {
        public void Solve(List<(long num, int skip)> input)
        {

            Dictionary<string, string> z3settings = new();
            z3settings["model"] = "true";

            var ctx = new Context(z3settings);
            var opt = ctx.MkOptimize();

            var var_X = ctx.MkIntConst("x");

            // konstante
            var ZERO = ctx.MkNumeral(0, ctx.MkIntSort()) as ArithExpr;
            var ONE = ctx.MkNumeral(1, ctx.MkIntSort()) as ArithExpr;

            opt.Add(ctx.MkGt(var_X, ZERO));

            var nums = new List<IntExpr>();

            // proste spremenljivke
            for (int i = 0; i < input.Count; i++)
            {
                var (num, skip) = input[i];

                var constNUM = ctx.MkNumeral(num, ctx.MkIntSort()) as IntExpr;
                var constSKIP = ctx.MkNumeral(skip, ctx.MkIntSort()) as ArithExpr;

                var constXtimesSKIP = ctx.MkIntConst($"x_SKIPPED_{i}");
                opt.Add(ctx.MkEq(constXtimesSKIP, ctx.MkAdd(var_X, constSKIP)));

                opt.Add(ctx.MkEq(ZERO, ctx.MkMod(constXtimesSKIP, constNUM)));
            }



            //opt.MkMaximize(var_inRangeCount);
            opt.MkMinimize(var_X);

            var status = opt.Check();
            var resModel = opt.Model;

          

            var res = resModel.Eval(var_X);

        }
    }
}
