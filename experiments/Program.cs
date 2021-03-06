﻿using AOC.Common;
using System;
using System.Linq;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;
using System.Collections.Generic;
using static AOC.Common.Func;

namespace experiments
{
    class Program
    {
        static void Main(string[] args)
        {
            Z3Sudoku.Solve();
            Z3aoc2018_23_part2.Solve();
            Parsec.Parse1();
            Parsec.Parse2();
        }
    }
}
