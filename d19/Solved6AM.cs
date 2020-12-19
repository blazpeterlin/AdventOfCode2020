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

namespace d19
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
        static Dictionary<int, List<List<string>>> dictRules = new();
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var grps = ih.AsLines().Split("").ToList();

            var rules = grps[0].ToList();
            var msgs = grps[1].ToList();

            foreach(var r in rules)
            {
                var tkns = r.Split(":");
                int num = tkns[0].AsInt();

                List<List<string>> myR = new();
                var ors = tkns[1].Split("|").ToList();
                foreach(var orr in ors)
                {
                    myR.Add(orr.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList());
                }

                dictRules[num] = myR;
            }

            int res1 = 0;
            foreach(var msg in msgs)
            {
                if (EatWithRule(0, new(), msg).Any(_ => _.str == "")) { res1++; }
            }


            // part2

            dictRules[8] = new List<List<string>> { new List<string> { "42" }, new List<string> { "42","8" } };
            dictRules[11] = new List<List<string>> { new List<string> { "42", "31" }, new List<string> { "42", "11", "31" } };


            int res2 = 0;
            foreach (var msg in msgs)
            {
                if (EatWithRule(0, new(), msg).Any(_ => _.str == "")) { res2++; }
            }
        }

        static IEnumerable<(string str, HashSet<(int, int)> hs)> EatWithRule(int num, HashSet<(int, int)> hs, string str)
        {
            var options = dictRules[num];
            var resHs = new HashSet<(int, int)>(hs);
            resHs.Add((num, str.Length));
            foreach(var opt in options)
            {
                var listRes = EatWithRule(str, resHs, opt);
                foreach (var res in listRes)
                {
                    if (res.str != null)
                    {
                        yield return (res.str, res.hs);
                    }
                }
            }
            
        }
        static List<(string str, HashSet<(int, int)> hs)> EatWithRule(string str, HashSet<(int, int)> hs, List<string> rules)
        {
            List<(string, HashSet<(int, int)>)> temps =new[] { (str, hs) }.ToList();
            foreach(var r in rules)
            {
                List<(string, HashSet<(int, int)>)> tempsNext = new();
                foreach (var ths in temps)
                {
                    var temp = ths.Item1;
                    if (temp == null) { return null; }
                    if (r.Contains("\""))
                    {
                        if (temp.Any() && temp[0] == r[1])
                        {
                            tempsNext.Add((temp.Substring(1), new HashSet<(int, int)>()));
                        }
                    }
                    else
                    {
                        var ruleNum = int.Parse(r);
                        if (!hs.Contains((ruleNum, temp.Length)))
                        {
                            tempsNext.AddRange(EatWithRule(ruleNum, hs, temp));
                        }
                        else
                        {

                        }
                    }
                }


                temps = tempsNext;
            }
            return temps;
        }
    }
}

