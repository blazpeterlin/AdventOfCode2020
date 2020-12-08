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

namespace d08
{
    enum VMHaltState
    {
        Running,
        Halt,
        //InfiniteLoop,
    }

    class VirtualMachine
    {
        public List<List<string>> InstructionSet { get; set; }
        public VirtualMachine(List<string> lines)
        {
            InstructionSet = lines.Select(ln => ln.Split(" ").ToList()).ToList();
            HaltCondition = () => !IdxInBounds();
        }

        public Func<bool> HaltCondition { get; set; }
        public bool IdxInBounds()
        {
            return Idx >= 0 && Idx < InstructionSet.Count;
        }

        public List<int> GetLinesOfInstruction(string instruction)
        {
            return InstructionSet
                .Select((ins, idx) => (ins[0], idx))
                .Choose(tpl => (tpl.Item1 == instruction, tpl.idx))
                .ToList();
        }

        public int Idx { get; set; } = 0;
        public long Acc { get; set; } = 0;
        public VMHaltState HaltState { get; set; } = VMHaltState.Running;

        public void RunUntilTrue(Func<bool> until)
        {
            while(!until())
            {
                Step();
            }
        }

        public VMHaltState Step()
        {
            if (HaltState == VMHaltState.Halt) { return HaltState; }

            List<string> args = InstructionSet[Idx];
            switch (args[0])
            {
                case "acc": Acc += args[1].AsLong(); Idx++; break;
                case "nop": Idx++; break;
                case "jmp": Idx += args[1].AsInt(); break;
                default: throw new Exception($"Unknown instruction '{args[0]}'");
            };

            if (HaltCondition()) { HaltState = VMHaltState.Halt; }
            return HaltState;
        }
    }

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
        //.Select(tkns => tkns.Tuplify(AsInt, AsChar, AsString))
        //.Where(t => t.t1 < t.t2)

        // var x = FRng(10, 20).Select((x, idx) => idx).ToList();
        // FRng(10,20).Permutations(6,false)

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
            var lns = ih.AsLines();

            long res1;
            {
                var vm = new VirtualMachine(lns);
                var visited = new HashSet<int>();
                bool rut()
                {
                    return !visited.Add(vm.Idx);
                }

                vm.RunUntilTrue(rut);
                res1 = vm.Acc;
            }

            var vmIns = new VirtualMachine(lns);
            List<int> jmps = vmIns.GetLinesOfInstruction("jmp");
            List<int> nops = vmIns.GetLinesOfInstruction("nop");

            long res2;
            foreach (var jmpIdx in jmps)
            {
                var vmj = new VirtualMachine(lns);
                vmj.InstructionSet[jmpIdx][0] = "nop";
                var visited = new HashSet<int>();
                bool rut()
                {
                    return !visited.Add(vmj.Idx);
                }
                vmj.RunUntilTrue(rut);
                if (vmj.HaltState == VMHaltState.Halt) { res2 = vmj.Acc; break; }
            }

            foreach (var nopIdx in nops)
            {
                var vmj = new VirtualMachine(lns);
                vmj.InstructionSet[nopIdx][0] = "jmp";
                var visited = new HashSet<int>();
                bool rut()
                {
                    return !visited.Add(vmj.Idx);
                }
                vmj.RunUntilTrue(rut);
                if (vmj.HaltState == VMHaltState.Halt) { res2 = vmj.Acc; break; }
            }
        }
    }
}
