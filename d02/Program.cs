using AOC.Common;
using System;
using System.Linq;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;
namespace d02
{
    class Program
    {
        static void Main(string[] args)
        {
            //AsText
            var txt = InputHelper.LoadInput(2020).AsLines();
            var res1 = txt
                .Select(ln => ln.Split(new[] { "-", " ", ":" }, StringSplitOptions.RemoveEmptyEntries))
                .Select(tkns => (int.Parse(tkns[0]), int.Parse(tkns[1]), tkns[2], tkns[3]))
                .Where(t => t.Item4.Count(ch => ch == t.Item3[0]) >= t.Item1 && t.Item4.Count(ch => ch == t.Item3[0]) <= t.Item2)
                .Count();

            var res2 = txt
               .Select(ln => ln.Split(new[] { "-", " ", ":" }, StringSplitOptions.RemoveEmptyEntries))
               .Select(tkns => (int.Parse(tkns[0]), int.Parse(tkns[1]), tkns[2], tkns[3]))
               .Where(t => (t.Item4[t.Item1 - 1] == t.Item3[0] ? 1 : 0) + (t.Item4[t.Item2 - 1] == t.Item3[0] ? 1 : 0) == 1)
               .Count();




            var res1nicer = txt
                .Select(ln => ln.Split(new[] { "-", " ", ":" }, StringSplitOptions.RemoveEmptyEntries))
                .Select(tkns => tkns.Tuplify(AsInt, AsInt, AsChar, AsString))
                .Where(t => t.t4.Count(ch => ch == t.t3) >= t.t1 && t.t4.Count(ch => ch == t.t3) <= t.t2)
                .Count();
        }
    }
}
