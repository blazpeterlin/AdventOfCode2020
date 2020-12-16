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

namespace d16
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
            var lns = ih.ModifyTxt(_ => _).AsLines().Split("").ToList();

            var grp1 = lns[0].ToList();
            var ranges = new List<(int, int, string)>();
            grp1.ForEach(ln =>
            {
                var meat = ln.Split(':').Last();
                var rngs = meat.FSplit(" ", "or", "-");
                string name = ln.Split(':').First();
                for(int i = 0; i <rngs.Length; i+=2)
                {
                    ranges.Add((rngs[i].AsInt(), rngs[i + 1].AsInt(), name));
                }
            });

            var tckts = lns[2].ToList();
            int res1 = 0;
            var goodTcktNums = new List<List<int>>();
            foreach (var tckt in tckts.Skip(1))
            {
                bool goodTicket = true;
                var tcktNums = tckt.Split(',').Select(int.Parse).ToList();
                foreach (var num in tcktNums)
                {
                    if (!ranges.Any(r  => r.Item1 <= num && r.Item2 >= num))
                    {
                        res1 += num;
                        goodTicket = false;
                    }
                }
                if(goodTicket)
                {
                    goodTcktNums.Add(tcktNums);
                }
            }

            Dictionary<int, HashSet<int>> rngIdxByPos = new();
            for(int i = 0; i < goodTcktNums.First().Count;i++)
            {
                rngIdxByPos[i] = new HashSet<int>();
                for(int j = 0; j < ranges.Count;j++)
                {
                    rngIdxByPos[i].Add(j);
                }
            }

            foreach(var gtn in goodTcktNums)
            {
                for (int i = 0; i < gtn.Count; i++)
                    //for (int i = 0; i < 1; i++)
                {
                    var num = gtn[i];
                    var ris = rngIdxByPos[i];
                    foreach (var ri in ris.ToList().Windowed(2).Where(lst => lst.ElementAt(0) %2==0))
                    {
                        var r1 = ranges[ri.ElementAt(0)];
                        var r2 = ranges[ri.ElementAt(1)];
                        if ((r1.Item1 > num || r1.Item2 < num) && (r2.Item1 > num || r2.Item2 < num))
                        { 
                            ris.Remove(ri.ElementAt(0));
                            ris.Remove(ri.ElementAt(1));
                        }
                    }
                }
            }

            var candidatesPerIdx = new Dictionary<int, List<string>>();
            foreach(var rip in rngIdxByPos)
            {
                if (!candidatesPerIdx.ContainsKey(rip.Key)) { candidatesPerIdx[rip.Key] = new List<string>(); }
                foreach(var ri in rip.Value)
                {
                    candidatesPerIdx[rip.Key].Add(ranges[ri].Item3);
                }
            }

            var c2 = string.Join(Environment.NewLine, candidatesPerIdx.Select(kvp => kvp.Key + " - " + string.Join(",",kvp.Value)).ToList());


            var availableStr = ranges.Select(_ => _.Item3).Distinct().ToList().FPipe(_ => new HashSet<string>(_));
            Dictionary<int, string> strByPos = new();
            while(availableStr.Any())
            {
                foreach(var cpi in candidatesPerIdx)
                {
                    var goodVals = cpi.Value.Where(_ => availableStr.Contains(_)).Distinct().ToList();
                    if (goodVals.Count() == 1)
                    {
                        availableStr.Remove(goodVals.Single());
                        strByPos[cpi.Key] = goodVals.Single();
                    }
                }
            }

            var goodPos = strByPos.Where(kvp => kvp.Value.ToLower().StartsWith("departure")).Select(_ => _.Key).ToList();
            var mine = lns[1].Skip(1).Single().FSplit(',').Select(int.Parse).ToList();
            var mineNums = goodPos.Select(gp => mine[gp]).ToList();
            var res2 = mineNums.Aggregate((long)1, (x, y) => x * y);
        }
    }
}
