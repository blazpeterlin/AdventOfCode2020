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
    class SolvedQuality
    {
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var inputGrouped = ih.ModifyTxt(_ => _).AsLines().Split("").ToList();

            var grp1 = inputGrouped[0].ToList();
            List<(int min, int max, string name)> ranges = 
                grp1.SelectMany(ln =>
                {
                    var (name,rngsStr) = ln.Split(':').Tuplify(Id,Id);
                    var rngs = rngsStr.FSplit(" ", "or", "-");
                    return rngs.Batch(2).Select(b => b.ToList()).Select(b => (b[0].AsInt(), b[1].AsInt(), name));
                })
                .ToList();

            var listTickets = inputGrouped[2].Skip(1).Select(t => t.Split(',').Select(int.Parse).ToList()).ToList();
            var badTicketNums = listTickets
                .SelectMany(
                  t => t.Where(n =>
                        !ranges.Any(tpl => tpl switch { var (min, max, _) => min <= n && n <= max, })
                       )
                )
                .ToList();
            var res1 = badTicketNums.Sum();


            // part2: your plain old C# solution
            var goodTickets = listTickets.Where(t => !t.Any(n =>
                                    !ranges.Any(tpl => tpl switch { var (min, max, _) => min <= n && n <= max, })
                                )).ToList();

            var rangesByName = ranges.GroupBy(_ => _.name).ToDictionary(kvp => kvp.Key, kvp => kvp.ToList());
            var rngPosCombos =
                rangesByName.Keys.Cartesian(FRng(0, goodTickets[0].Count), (name, tidx) => (name, tidx))
                .Where(tpl => tpl switch { var (rname, tidx) => goodTickets.All(gt => rangesByName[rname].Any(r => r.min <= gt[tidx] && gt[tidx] <= r.max)) })
                .ToList();
            var posByName = rngPosCombos.GroupBy(_ => _.name).ToDictionary(grp => grp.Key, grp => grp.Select(_ => _.tidx).ToList());
            Dictionary<string, int> dictIdxByName = new();
            HashSet<int> foundPos = new();
            List<int> GetRemaining(IEnumerable<int> idxs) => idxs.Where(_ => !foundPos.Contains(_)).ToList();
            while (foundPos.Count < ranges.Count/2)
            {
                var combo = posByName.FirstOrDefault(kvp => GetRemaining(kvp.Value).Count == 1);
                var idx = GetRemaining(combo.Value).Single();
                foundPos.Add(idx);
                dictIdxByName[combo.Key] = idx;
            }

            var res2keys = dictIdxByName.Keys.Where(k => k.StartsWith("departure"));
            var myTicket = inputGrouped[1].Skip(1).Single().Split(',').Select(int.Parse).ToList();
            var res2nums = res2keys.Select(_ => dictIdxByName[_]).Select(idx => myTicket[idx]).ToList();
            long res2 = res2nums.Aggregate((long)1, (x, y) => x * y);

            // TODO: part2 Z3 solution
        }
    }
}

