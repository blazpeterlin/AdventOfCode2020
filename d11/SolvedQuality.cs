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

    class SolvedQuality
    {
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var chs = ih.AsCharListOfLists();


            //Helpers.PrintArrayGeneric(chs);



            for (int rep = 0; true; rep++)
            {
                var ch2s = chs.Select(lst => lst.ToList()).ToList();
                bool changed = false;
                var allPos = chs.ToValByPos();

                foreach (var (j, i) in allPos.Keys)
                {
                    if (chs[j][i] == '.') { continue; }

                    if (chs[j][i] == 'L')
                    {
                        var listAdj = chs.Slice2d(false, j - 1, 3, i - 1, 3);
                        int adj = listAdj.Where(_ => _ != (j, i)).Select(_ => chs[_.d1][_.d2]).Where(ch => ch == '#').Count();

                        if (adj == 0)
                        {
                            ch2s[j][i] = '#';
                            changed = true;
                        }
                    }

                    if (chs[j][i] == '#')
                    {
                        var listAdj = chs.Slice2d(false, j - 1, 3, i - 1, 3).ToList();
                        int adj = listAdj.Where(_ => _ != (j, i)).Select(_ => chs[_.d1][_.d2]).Where(ch => ch == '#').Count();

                        if (adj >= 4)
                        {
                            ch2s[j][i] = 'L';
                            changed = true;
                        }
                        else
                        {

                        }
                    }
                }

                chs = ch2s;

                //Console.WriteLine(" ");
                //Helpers.PrintArrayGeneric(chs);

                if (!changed) { break; }
            }

            var res1 = chs.SelectMany(_ => _).Where(ch => ch == '#').Count();


            // reset for part 2
            chs = ih.AsCharListOfLists();
            //Helpers.PrintArrayGeneric(chs);

            var listDirs = FRng(-1, 2).FRng(-1,2).Except(new[] { (0, 0) }).ToList();

            for (int rep = 0; true; rep++)
            {
                var ch2s = chs.Select(lst => lst.ToList()).ToList();
                bool changed = false;
                var allPos = chs.ToValByPos();

                foreach(var (j,i) in allPos.Keys)
                {
                    if (chs[j][i] == '.') { continue; }

                    if (chs[j][i] == 'L')
                    {
                        //var listAdj = chs.Slice2d(false, j - 1, 3, i - 1, 3);
                        //int adj = listAdj.Where(_ => _ != (j, i)).Select(_ => chs[_.d1][_.d2]).Where(ch => ch == '#').Count();


                        int adj = 0;
                        foreach (var dir in listDirs)
                        {
                            var acceptablePos = (j, i).FUnfold(pos => pos.Value.Add(dir) switch { var ipos when allPos.ContainsKey(ipos) => ipos, _ => null });
                            //allPos.ContainsKey(pos.Value.Add(dir)) ? pos.Value.Add(dir) : null);
                            var soughtPos = acceptablePos.FirstOrDefault(pos => allPos[pos.Value] != '.');
                            if (soughtPos != null && allPos[soughtPos.Value] == '#') { adj++; }
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
                        foreach (var dir in listDirs)
                        {
                            var acceptablePos = (j, i).FUnfold(pos => pos.Value.Add(dir) switch { var ipos when allPos.ContainsKey(ipos) => ipos, _ => null });
                            var soughtPos = acceptablePos.FirstOrDefault(pos => allPos[pos.Value] != '.');
                            if (soughtPos != null && allPos[soughtPos.Value] == '#') { adj++; }
                        }

                        if (adj >= 5)
                        {
                            ch2s[j][i] = 'L';
                            changed = true;
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
