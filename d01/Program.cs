using AOC.Common;
//using AOC.CommmonFS;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace d01
{
    class Program
    {
        static void Main(string[] args)
        {
            var tkns = InputHelper.LoadInput(2020).AsTokens<int>();
            
            //int num = tkns.Count;
            //var res1 =
            //    tkns
            //    //.Select(x => x)
            //    .FFold((x, y) => x + y, 0)
            //    ;

            var setSum1 = new HashSet<int>();
            var dictSum2 = new Dictionary<int,int[]>();
            foreach(var num in tkns)
            {
                setSum1.Add(num);

                if (setSum1.Contains(2020 - num)) { var res1 = num * (2020 - num); }

                foreach(var num2 in tkns)
                {
                    dictSum2[num + num2] = new[] {num,num2 };
                }
            }

            foreach(var num in tkns)
            {
                if(dictSum2.ContainsKey(2020-num))
                {
                    var arr2 = dictSum2[2020 - num];
                    var res2 = arr2[0] * arr2[1] * num;
                }
            }

            //Helpers.SetClipboard(""+res1);


        }
    }
}
