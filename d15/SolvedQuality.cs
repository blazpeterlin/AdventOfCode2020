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

namespace d15
{
    class SolvedQuality
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
            var nums = ih.AsText().Split(',').Select(int.Parse).ToList();


            Dictionary<int, int> dictStepByNum = new Dictionary<int, int>();
            Dictionary<int, int> dictStepByNumOneBehind = new Dictionary<int, int>();
            Dictionary<int, int> dictNumByStep = new Dictionary<int, int>();

            void AddStep(int step, int num)
            {
                dictStepByNum[num] = step;
                
                if (step > 0)
                {
                    var lastNum = dictNumByStep[step - 1];
                    dictStepByNumOneBehind[lastNum] = step - 1;
                }
                dictNumByStep[step] = num;
            }

            for (int i = 0; i < nums.Count; i++)
            {
                AddStep(i, nums[i]);
            }
            for (int i = nums.Count; i < 30000000; i++)
            {
                int newNum;
                if (dictStepByNumOneBehind.TryGetValue(dictNumByStep[i - 1], out var step)) { newNum = i - step - 1; } else { newNum = 0; }
                AddStep(i, newNum);
            }
            var res1 = dictNumByStep[2019];
            var res2 = dictNumByStep[29999999];

        }
    }
}

