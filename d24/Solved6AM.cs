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

namespace d24
{
    class Solved6AM
    {
        //// Reminders:

        //var txt = ih.AsText();
        //var tkns = ih.AsTokens<int>();
        //var chch = ih.AsCharListOfLists();

        // AsInt
        // Graph<(int, int)>.From2dWithMoves(pts, Moves.PLUS).Dijkstra((0,0),(3,1)).Path();
        // ih.ModifyLines(ln => { if (ln == "") return "-"; else return ln; });

        //.Select(ln => ln.FSplit("-", " ", ":"))
        //.Select(tkns => tkns.Tuplify(int.Parse, Enumerable.First, x => x)) // int, char, string
        //.Where(t => t.t1 < t.t2)

        // var allPos = chs.ToValByPos();

        // var vm = new VirtualMachine(lns);
        // var visited = new HashSet<int>();
        // bool rut()
        // {
        //     return !visited.Add(vm.Idx);
        // }
        // vm.RunUntilTrue(rut);
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var lns = ih.AsLines();

            Dictionary<(int x, int y), bool> blackByPos = new();

            foreach(var ln in lns)
            {
                (int x, int y) pos = (0, 0);
                for(int i = 0; i < ln.Length; i++)
                {
                    string s = ""+ln[i];
                    if (s == "n" || s == "s") { i++; s += ln[i]; }

                    var delta = s switch
                    {
                        "nw" => (pos.y % 2 == 0 ? -1 : 0, -1),
                        "ne" => (pos.y % 2 == 0 ? 0 : 1, -1),
                        "w" => (-1, 0),
                        "e" => (+1, 0),
                        "sw" => (pos.y % 2 == 0 ? -1 : 0, 1),
                        "se" => (pos.y % 2 == 0 ? 0 : 1, 1),
                        _ => throw new Exception(),
                    };

                    pos = (pos.x + delta.Item1, pos.y + delta.Item2);
                }

                var prevVal = false;
                if (blackByPos.TryGetValue(pos, out var prevValDict)) { prevVal = prevValDict; }
                blackByPos[pos] = !prevVal;
            }

            int res1 = blackByPos.Values.Where(_ => _).Count();



            var minX = blackByPos.Keys.Select(_ => _.x).Min();
            var maxX = blackByPos.Keys.Select(_ => _.x).Max();
            var minY = blackByPos.Keys.Select(_ => _.y).Min();
            var maxY = blackByPos.Keys.Select(_ => _.y).Max();

            int steps = 100;
            int n = steps + 1;

            foreach(var (x,y) in FRng(minX - n, maxX+ n + 1).FRng(minY - n, maxY + n + 1))
            {
                if (!blackByPos.ContainsKey((x,y)))
                {
                    blackByPos[(x, y)] = false;
                }
            }

            var dict = blackByPos;
            for (int step = 0; step < steps; step++)
            {
                var dict2 = new Dictionary<(int x, int y), bool>(dict);

                foreach (var pos in FRng(minX - steps, maxX + steps + 1).FRng(minY - steps, maxY + steps + 1))
                {
                    var moves = new[] { "nw", "ne", "w", "e", "sw", "se" };
                    var deltas = moves.Select(m => m switch
                    {
                        "nw" => (pos.snd % 2 == 0 ? -1 : 0, -1),
                        "ne" => (pos.snd % 2 == 0 ? 0 : 1, -1),
                        "w" => (-1, 0),
                        "e" => (+1, 0),
                        "sw" => (pos.snd % 2 == 0 ? -1 : 0, 1),
                        "se" => (pos.snd % 2 == 0 ? 0 : 1, 1),
                        _ => throw new Exception(),
                    });
                    var neighbours = deltas.Select(delta => (pos.fst + delta.Item1, pos.snd + delta.Item2)).ToList();
                    var meBlack = dict[pos];
                    var blackN = neighbours.Select(neigh => dict[neigh] ? 1 : 0).Sum();
                    if (meBlack && (blackN == 0 || blackN > 2)) { dict2[pos] = false; }
                    if (!meBlack && (blackN == 2)) { dict2[pos] = true; }
                }

                dict = dict2;
            }


            int res2 = dict.Values.Where(_ => _).Count();
        }
    }
}
