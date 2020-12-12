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
    class Z3Sudoku
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
            var sudokuInput =
                File.ReadAllLines("input-sudoku-3.txt")
                .Select(ln => ln.Select(ch => int.TryParse(ch.ToString(), out var i) ? i : new int?()).ToList())
                .ToList();

            Dictionary<string, string> z3settings = new();
            z3settings["model"] = "true";

            var ctx = new Context(z3settings);
            var opt = ctx.MkOptimize();

            // konstante
            var ONE = ctx.MkNumeral(1, ctx.MkIntSort()) as ArithExpr;
            var NINE = ctx.MkNumeral(9, ctx.MkIntSort()) as ArithExpr;

            // proste spremenljivke
            IntExpr[,] nums = new IntExpr[9, 9];
            if (sudokuInput.Count != 9) { throw new Exception(); }
            for (int i = 0; i < sudokuInput.Count; i++)
            {
                var ln = sudokuInput[i];
                if (ln.Count != 9) { throw new Exception(); }

                for (int j = 0; j < ln.Count; j++)
                {
                    var num = ln[j];
                    nums[i, j] = ctx.MkIntConst($"num_{i}_{j}");
                    if (num.HasValue)
                    {
                        opt.Add(ctx.MkEq(nums[i, j], ctx.MkNumeral(num.Value, ctx.MkIntSort())));
                    }
                    else
                    {
                        opt.Add(ctx.MkGe(nums[i, j], ONE));
                        opt.Add(ctx.MkLe(nums[i, j], NINE));
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {

                // distinct cols
                var numsCol = nums.SliceD1(i).SelectMany(_ =>_).ToArray();
                opt.Add(ctx.MkDistinct(numsCol));
                // distinct rows
                var numsRow = nums.SliceD2(i).SelectMany(_ => _).ToArray();
                opt.Add(ctx.MkDistinct(numsRow));
            }
            foreach (var (sq1, sq2) in FRng(0, 3).FRng(0, 3))
            {
                // distinct 3x3 squares
                var square =
                    nums
                    .SliceD1(sq1 * 3, sq1 * 3 + 1, sq1 * 3 + 2)
                    .SliceD2(sq2 * 3, sq2 * 3 + 1, sq2 * 3 + 2)
                    .SelectMany(_ => _)
                    .ToArray();
                opt.Add(ctx.MkDistinct(square));
            }


            //opt.MkMaximize(var_inRangeCount);
            //opt.MkMinimize(var_DistFromZero);

            var status = opt.Check();
            var resModel = opt.Model;

            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < 9; j++)
                {
                    //Console.WriteLine($"{i},{j}: {resModel.Eval(nums[i, j])}");
                    Console.Write($"{resModel.Eval(nums[i, j])}");
                }
            }

            //var res_distFromZero = resModel.Eval(var_DistFromZero);
        }
    }
}
