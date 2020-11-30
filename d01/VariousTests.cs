using AOC.Common;
//using AOC.CommmonFS;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace d01
{
    class Tests
    {
        public static void Test()
        {
            Test_CS9_Switch();
            TEST_Dijkstra();
            //TEST_Input();

           
        }

        static void TEST_Dijkstra()
        {
            Console.WriteLine("Hello World!");
            var pts = new[] { (0, 0), (0, 1), (1, 1), (1, 2), (2, 2), (3, 2), (3,1), };
            var p = Graph<(int, int)>.From2dWithMoves(pts, Moves.PLUS).Dijkstra((0,0),(3,1)).Path();
            //var wt = Dijkstra.Dijkstra2D_PointList_Moves4Plus(ListModule.OfSeq(pts), (0, 0), (2, 3));
            //Dijkstra
        }

        static void Test_CS9_Switch()
        {
            A x = new A() { a = 123 };
            var y = x.a switch
            {
                <= 100 => 1,
                >= 200 => 2,
                _ => 3
            };

            int a = 1;
            string b = "b";
            int test = (a, b) switch
            {
                (0, "a") => 2,
                (0, _) => 0,
                (1, _) => 1,
                _ => -1
            };
        }

        static void TEST_Input()
        {


            InputHelper.LoadInput(2018);
            HashSet<int> s = new HashSet<int>();
            var tkns = InputHelper.AsTokens<int>();
            var res =
                tkns
                //.Where(_ => true)
                //.GroupAdjacent(_ =>_/3)
                //.Select(_ => _ / 2 + 3)
                //.Repeat(999999)
                //.FFold((x, y) => x + y, 0)
                .FScan((x, y) => x + y, 0)
                .TakeUntil(x =>
                {
                    if (s.Contains(x)) { return true; }
                    s.Add(x);
                    return false;
                })
                .Last();
            ;

            Helpers.SetClipboard("" + res);
        }

        static void DefaultMain()
        {
            HashSet<int> s = new HashSet<int>();
            //var str = InputHelper.ToAllLines()
            var tkns = InputHelper.LoadInput(2020).AsTokens<int>();

            int num = tkns.Count;
            var res =
                tkns
                //.Where(_ => true)
                //.Index().Where(kvp => kvp.Key %3==0).Select(kvp => kvp.Value)
                //.Select(_ => _ / 2 + 3)
                //.Repeat(999999)
                .FFold((x, y) => x + y, 0)
                //.FScan((x, y) => x + y, 0)
                //.TakeUntil(x =>
                //{
                //    if (s.Contains(x)) { return true; }
                //    s.Add(x);
                //    return false;
                //})
                //.Last();
                ;

            Helpers.SetClipboard("" + res);
        }
    }
}
