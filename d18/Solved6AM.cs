using AOC.Common;
using Microsoft.FSharp.Core;
using FParsec.CSharp;
using static FParsec.CSharp.PrimitivesCS; // combinator functions
using static FParsec.CSharp.CharParsersCS; // pre-defined parsers

namespace d18
{
    class Solved6AM
    {
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var lns = ih.AsLines();

            {
                var basicExprParser = new OPPBuilder<Unit, long, Unit>()
                    .WithOperators(ops => ops
                        .AddInfix("+", 1, (x, y) => x + y)
                        .AddInfix("*", 1, (x, y) => x * y))
                        .WithTerms(term => Choice(Long, Between('(', term, ')')))
                    .Build()
                    .ExpressionParser;

                long sum = 0;
                foreach (var ln in lns)
                {
                    var ln2 = ln.Replace(" ", "");
                    var calculated = basicExprParser.Run(ln2).GetResult();
                    sum += calculated;
                }
                var res1 = sum;
            }

            {
                var basicExprParser = new OPPBuilder<Unit, long, Unit>()
                    .WithOperators(ops => ops
                        .AddInfix("+", 2, (x, y) => x + y)
                        .AddInfix("*", 1, (x, y) => x * y))
                        .WithTerms(term => Choice(Long, Between('(', term, ')')))
                    .Build()
                    .ExpressionParser;

                long sum = 0;
                foreach (var ln in lns)
                {
                    var ln2 = ln.Replace(" ", "");
                    var calculated = basicExprParser.Run(ln2).GetResult();
                    sum += calculated;
                }
                var res2 = sum;
            }
        }
    }
}
