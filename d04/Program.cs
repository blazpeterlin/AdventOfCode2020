using AOC.Common;
using System;
using System.Linq;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;

namespace d04
{
    class Program
    {
        // AsInt
        // Graph<(int, int)>.From2dWithMoves(pts, Moves.PLUS).Dijkstra((0,0),(3,1)).Path();
        static void Main(string[] args)
        {
            var ih = InputHelper.LoadInputP(2020);
            // simplify input - passports split by "-" instead of empty line
            ih.ModifyLines(ln => { if (ln == "") return "-"; else return ln; });
            var lns = ih.AsLines();
            var txt = ih.AsText();
            var tkns = ih.AsTokens<string>();
            var chch = ih.AsCharListOfLists();

            var valid = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" };
            var except = new[] { "cid" };

            var res1 =
                tkns
                .Split("-")
                .Where(val =>
                {
                    var acceptable = val
        .Select(t => t.Split(":").First())
        .Where(_ => !except.Contains(_.ToLower())).ToList();
                    if (acceptable.All(_ => valid.Contains(_)) && acceptable.Count == 7)
                    {
                        return true;
                    }
                    return false;
                })
                .Count();

            var res2 = tkns
                .Split("-")
                .Select(val =>
                {
                    var acceptable = val
        .Select(t => t.Split(":").First())
        .Where(_ => !except.Contains(_.ToLower())).ToList();
                    if (acceptable.All(_ => valid.Contains(_)) && acceptable.Count == 7)
                    {
                        var kvs = val.Select(t => t.Split(":").Tuplify(AsString, AsString)).ToDictionary(tpl => tpl.t1, tpl => tpl.t2);
                        foreach(var kvp in kvs)
                        {
                            var k = kvp.Key;
                            var v = kvp.Value;
                            if (k == "byr") { if (!int.TryParse(v, out var i) || i < 1920 || i > 2002) { return false; } }
                            if (k == "iyr") { if (!int.TryParse(v, out var i) || i < 2010 || i > 2020) { return false; } }
                            if (k == "eyr") { if (!int.TryParse(v, out var i) || i < 2020 || i > 2030) { return false; } }
                            if (k == "hgt")
                            {
                                if (v.EndsWith("cm"))
                                {
                                    var cm = v.Substring(0, v.Length - 2);
                                    if (!int.TryParse(cm, out var cmi) || cmi < 150 || cmi > 193) { return false; }
                                }
                                else if (v.EndsWith("in"))
                                {
                                    var @in = v.Substring(0,v.Length - 2);
                                    if (!int.TryParse(@in, out var ini) || ini < 59 || ini > 76) { return false; }
                                }
                                else return false;
                            }
                            if (k == "hcl")
                            {
                                if (!v.StartsWith("#")) { return false; }
                                var v2 = v.Substring(1);
                                if (v2.Length != 6) { return false; }
                                if (!v2.All(_ => char.IsDigit(_) || new[] { 'a','b','c','d','e','f' }.Contains(_))){ return false; }
                            }
                            if (k == "ecl")
                            {
                                if (!(new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(v))) { return false; }
                            }
                            if (k == "pid")
                            {
                                if (v.Length != 9) { return false; }
                                if (!int.TryParse(v, out var i)) { return false; }
                            }

                        }
                        return true;
                    }
                    return false;
                }
                //.Except(except)
                ).Where(_ => _ == true)
                .Count();



            var final = (res1, res2);
            //.Select(ln => ln.FSplit("-", " ", ":"))
            //.Select(tkns => tkns.Tuplify(AsInt, AsChar, AsString))
            //.Where(t => t.t1 < t.t2)

        }
    }
}
