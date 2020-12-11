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

namespace d11
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
            var chs = ih.AsCharListOfLists();


            //Helpers.PrintArrayGeneric(chs);



            for (int rep = 0; true; rep++)
            {
                var ch2s = chs.Select(lst => lst.ToList()).ToList();
                bool changed = false;

                for (int j=0;j<chs.Count;j++)
                {

                    for (int i = 0; i < chs[j].Count; i++)
                    {
                        if (chs[j][i] == '.') { continue; }

                        if (chs[j][i] == 'L')
                        {
                            int adj = 0;
                            for(int j2 = Math.Max(j-1,0); j2 <= Math.Min(j+1,chs.Count-1);j2++)
                            {
                                for (int i2 = Math.Max(i - 1, 0); i2 <= Math.Min(i + 1, chs[j2].Count - 1); i2++)
                                {
                                    if(i2 == i && j2 == j) { continue; }
                                    adj += chs[j2][i2] == '#' ? 1 : 0;
                                }
                            }
                            if (adj == 0)
                            {
                                ch2s[j][i] = '#';
                                changed = true;
                            }
                        }

                        if (chs[j][i] == '#')
                        {
                            int adj = 0;
                            for (int j2 = Math.Max(j - 1, 0); j2 <= Math.Min(j + 1, chs.Count - 1); j2++)
                            {
                                for (int i2 = Math.Max(i - 1, 0); i2 <= Math.Min(i + 1, chs[j2].Count - 1); i2++)
                                {
                                    if (i2 == i && j2 == j) { continue; }
                                    adj += chs[j2][i2] == '#' ? 1 : 0;
                                }
                            }
                            if (adj >= 4)
                            {
                                ch2s[j][i] = 'L';
                                changed = true;
                            }
                        }
                    }
                }

                chs = ch2s;

                //Console.WriteLine(" ");
                //Helpers.PrintArrayGeneric(chs);

                if (!changed) { break; }
            }

            var res1 = chs.SelectMany(_ => _).Where(ch => ch == '#').Count();

















            chs = ih.AsCharListOfLists();


            //Helpers.PrintArrayGeneric(chs);



            for (int rep = 0; true; rep++)
            {
                var ch2s = chs.Select(lst => lst.ToList()).ToList();
                bool changed = false;

                for (int j = 0; j < chs.Count; j++)
                {

                    for (int i = 0; i < chs[j].Count; i++)
                    {
                        if (chs[j][i] == '.') { continue; }

                        if (chs[j][i] == 'L')
                        {
                            int adj = 0;
                            for (int j2 = Math.Max(j - 1, 0); j2 <= Math.Min(j + 1, chs.Count - 1); j2++)
                            {
                                for (int i2 = Math.Max(i - 1, 0); i2 <= Math.Min(i + 1, chs[j2].Count - 1); i2++)
                                {
                                    if (i2 == i && j2 == j) { continue; }
                                    var i3 = i2;
                                    var j3 = j2;
                                    var diffJ = j3 - j;
                                    var diffI = i3 - i;

                                    while(chs[j3][i3]=='.')
                                    {
                                        j3 += diffJ;
                                        i3 += diffI;

                                        if (j3 < 0 || j3 >= chs.Count || i3 < 0 || i3 >= chs[j3].Count)
                                        {
                                            break;
                                        }
                                    }

                                    if (j3 < 0 || j3 >= chs.Count || i3 < 0 || i3 >= chs[j3].Count)
                                    {
                                        continue;
                                    }

                                    adj += chs[j3][i3] == '#' ? 1 : 0;
                                }
                            }
                            if (adj == 0)
                            {
                                ch2s[j][i] = '#';
                                changed = true;
                            }
                        }

                        if (chs[j][i] == '#')
                        {
                            if (i == 0 && j == 7)
                            {

                            }
                            if (i == 1 && j == 7)
                            {
                                //    Console.WriteLine(" ");
                                //    Helpers.PrintArrayGeneric(chs);
                            }

                            int adj = 0;
                            for (int j2 = Math.Max(j - 1, 0); j2 <= Math.Min(j + 1, chs.Count - 1); j2++)
                            {
                                for (int i2 = Math.Max(i - 1, 0); i2 <= Math.Min(i + 1, chs[j2].Count - 1); i2++)
                                {
                                    if (i2 == i && j2 == j) { continue; }

                                    var i3 = i2;
                                    var j3 = j2;
                                    var diffJ = j3 - j;
                                    var diffI = i3 - i;

                                    while (chs[j3][i3] == '.')
                                    {
                                        j3 += diffJ;
                                        i3 += diffI;

                                        if (j3 < 0 || j3 >= chs.Count || i3 < 0 || i3 >= chs[j3].Count)
                                        {
                                            break;
                                        }
                                    }

                                    if (j3 < 0 || j3 >= chs.Count || i3 < 0 || i3 >= chs[j3].Count)
                                    {
                                        continue;
                                    }

                                    adj += chs[j3][i3] == '#' ? 1 : 0;
                                }
                            }

                            if (adj >= 5)
                            {
                                ch2s[j][i] = 'L';
                                changed = true;
                            }
                        }
                    }
                }

                chs = ch2s;

                //Console.WriteLine(" ");
                //Helpers.PrintArrayGeneric(chs);

                if (!changed) { break; }
            }


            var res2 = chs.SelectMany(_ => _).Where(ch => ch == '#').Count();

            
        }
    }
}
