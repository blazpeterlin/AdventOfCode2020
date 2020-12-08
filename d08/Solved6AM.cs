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

namespace d08
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
        //.Select(tkns => tkns.Tuplify(AsInt, AsChar, AsString))
        //.Where(t => t.t1 < t.t2)

        // var x = FRng(10, 20).Select((x, idx) => idx).ToList();
        // FRng(10,20).Permutations(6,false)
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var lns = ih.AsLines();


            int acc = 0;
            int idx = 0;
            int res1;
            HashSet<int> visited = new();
            HashSet<int> jmps = new();
            HashSet<int> nops = new();

            var iset = lns.Select(ln => ln.Split(" ")).ToList();

            while (idx >= 0 && idx < lns.Count)
            {
                string ins = iset[idx][0];
                string[] args = iset[idx];
                //int arg1 = iset[idx][1].AsInt();
                if (visited.Contains(idx))
                {
                    res1 = acc;
                    break;
                }
                if (ins == "jmp")
                {
                    jmps.Add(idx);
                }
                if (ins == "nop")
                {
                    nops.Add(idx);
                }
                visited.Add(idx);
                switch (ins)
                {
                    case "acc": acc += args[1].AsInt(); idx++; break;
                    case "nop": idx++; break;
                    case "jmp": idx += args[1].AsInt(); break;

                };

            }




            int res2;

            foreach (var jmpIdx in jmps)
            {
                visited.Clear();
                idx = 0;
                acc = 0;
                while (idx >= 0 && idx < lns.Count)
                {
                    string ins = iset[idx][0];
                    string[] args = iset[idx];
                    if (visited.Contains(idx))
                    {
                        //res1 = acc;
                        break;
                    }
                    visited.Add(idx);

                    if (idx == jmpIdx)
                    {
                        ins = "nop";
                    }
                    
                    switch (ins)
                    {
                        case "acc": acc += args[1].AsInt(); idx++; break;
                        case "nop": idx++; break;
                        case "jmp": idx += args[1].AsInt(); break;

                    };
                    


                    if (!(idx >= 0 && idx < lns.Count))
                    {
                        res2 = acc;
                    }
                }
            }




            foreach (var nopIdx in nops)
            {
                visited.Clear();
                idx = 0;
                acc = 0;
                while (idx >= 0 && idx < lns.Count)
                {
                    string ins = iset[idx][0];
                    string[] args = iset[idx];
                    if (visited.Contains(idx))
                    {
                        //res1 = acc;
                        break;
                    }
                    visited.Add(idx);

                    if (idx == nopIdx)
                    {
                        ins = "jmp";
                    }
                   
                    switch (ins)
                    {
                        case "acc": acc += args[1].AsInt(); idx++; break;
                        case "nop": idx++; break;
                        case "jmp": idx += args[1].AsInt(); break;

                    };


                    if (!(idx >= 0 && idx < lns.Count))
                    {
                        res2 = acc;
                    }
                }
            }
        }
    }
}
