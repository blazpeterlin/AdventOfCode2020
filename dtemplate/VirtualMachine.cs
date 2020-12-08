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

namespace d00
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
            while (!until())
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

}
