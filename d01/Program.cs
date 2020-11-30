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
            
            int num = tkns.Count;
            var res =
                tkns
                .Select(x => x)
                .FFold((x, y) => x + y, 0)
                ;

            Helpers.SetClipboard(""+res);
        }
    }
}
