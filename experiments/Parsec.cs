using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FParsec.CSharp; // extension functions (combinators & helpers)
using static FParsec.CSharp.PrimitivesCS; // combinator functions
using static FParsec.CSharp.CharParsersCS; // pre-defined parsers
using Microsoft.FSharp.Core;
using static FParsec.CharParsers;
using System.IO;

namespace experiments
{
    class Parsec
    {
        public static void Parse1()
        {
            var input = File.ReadAllText("input-parsec.txt");

            // todo
        }

        public static void Parse2()
        {
            {
                // best way to get result
                var x = Many1(Digit).AndR(Upper).Run("123a");
                //// throws exception
                //var rx = x.GetResult();
            }

            { 
                // get as string
                var y = Many1(Digit).AndR(Many1(Upper)).Run("123A");
                Microsoft.FSharp.Collections.FSharpList<char> ry1 = y.GetResult();
                var ry2 = new string(ry1.ToArray());
                
            }

            {
                var x = Many1(Digit).AndR(Upper).Run("123a");
                // does not throw
                var rx = x.UnwrapResult();
            }

            // arithmetic expressions
            {
                var basicExprParser = new OPPBuilder<Unit, int, Unit>()
                    .WithOperators(ops => ops
                        .AddInfix("+", 1, (x, y) => x + y)
                        .AddInfix("*", 2, (x, y) => x * y))
                    .WithTerms(Natural)
                    .Build()
                    .ExpressionParser;

                var recursiveExprParser = new OPPBuilder<Unit, int, Unit>()
                    .WithOperators(ops => ops
                        .AddInfix("+", 1, (x, y) => x + y)
                        .AddInfix("*", 2, (x, y) => x * y))
                    .WithTerms(term => Choice(Natural, Between('(', term, ')')))
                    .Build()
                    .ExpressionParser;

                var calculated = recursiveExprParser.Run("4*(2+3*2)").GetResult();
            }

            // save expr tree as graph
            {
                var naturalTermToBasicElt = Natural.Map(i => new BasicValue { Val = i } as IBasicElt);

                var basicExprParser = new OPPBuilder<Unit, IBasicElt, Unit>()
                    .WithOperators(ops => ops
                        .AddInfix("+", 1, (x, y) => new BasicExprTree("PLUS", x, y))
                        .AddInfix("*", 2, (x, y) => new BasicExprTree("TIMES", x, y)))
                    //.WithTerms(Natural.Map(i => new BasicValue { Val = i } as IBasicElt))
                    .WithTerms(naturalTermToBasicElt)
                    .Build()
                    .ExpressionParser
                    ;

                var recursiveExprParser = new OPPBuilder<Unit, IBasicElt, Unit>()
                    .WithOperators(ops => ops
                        .AddInfix("+", 1, (x, y) => new BasicExprTree("PLUS", x, y))
                        .AddInfix("*", 2, (x, y) => new BasicExprTree("TIMES", x, y)))
                    .WithTerms(term => Choice(naturalTermToBasicElt, Between('(', term, ')')))
                    .Build()
                    .ExpressionParser;

                var calculated = recursiveExprParser.Run("4*(2+3*2)").GetResult();
                var calculatedVal = calculated.Calculate();
            }
        }

        interface IBasicElt
        {
            int Calculate();
        }

        class BasicValue : IBasicElt
        {
            public int Val { get; set; }
            public int Calculate() { return Val; }
        }
        class BasicExprTree : IBasicElt
        {
            public BasicExprTree(string oper, IBasicElt op1, IBasicElt op2)
            {
                Operator = oper;
                Op1 = op1;
                Op2 = op2;
            }
            public string Operator { get; set; }
            public IBasicElt Op1 { get; set; }
            public IBasicElt Op2 { get; set; }

            public int Calculate()
            {
                if (Operator == "PLUS") { return Op1.Calculate() + Op2.Calculate(); }
                if (Operator == "TIMES") { return Op1.Calculate() * Op2.Calculate(); }
                throw new ArgumentException();
            }
        }
    }
}
