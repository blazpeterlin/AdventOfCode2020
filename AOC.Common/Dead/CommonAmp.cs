using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC.Common
{
    public class CommonAmp : SignalListener
    {
        public static bool DEBUG_OUTPUT = false;

        public CommonAmp(string label, List<long> inputs, List<long> opCodes)
        {
            Label = label;
            Inputs = inputs ?? new List<long>();
            for (int i = 0; i < opCodes.Count; i++)
            {
                OpCodes[i] = opCodes[i];
            }
        }
        public CommonAmp(CommonAmp amp)
        {
            Label = Label + "_1";
            Inputs = new List<long>();
            for (int i = 0; i < amp.OpCodes.Count; i++)
            {
                OpCodes[i] = amp.OpCodes[i];
            }
        }

        public Dictionary<long, long> OpCodes = new Dictionary<long, long>();

        public bool Halted = false;
        public long RelativeBase = 0;
        public string Label { get; }
        public int CurrentInputIdx = 0;
        public List<long> Outputs { get; } = new List<long>();
        public List<ISignalListener> Targets = new List<ISignalListener>();


        protected int _CurrentIdx = 0;




        private const long OP1_PLUS = 1;
        private const long OP2_TIMES = 2;
        private const long OP3_READ = 3;
        private const long OP4_PRINT = 4;
        private const long OP5_IFNOT0_GOTO = 5;
        private const long OP6_IF0_GOTO = 6;
        private const long OP7_IS_LESS = 7;
        private const long OP8_IS_EQ = 8;
        private const long OP9_RELATIVE_BASE_ADJUST = 9;
        private const long OP99_HALT = 99;

        protected override void Signalled()
        {
            StartOrResume();
        }
        private long? ReadInput()
        {
            if (Inputs.Count <= CurrentInputIdx)
            {
                return null;
            }
            return Inputs[CurrentInputIdx++];
        }


        public bool StartOrResume()
        {
            if (DEBUG_OUTPUT) { Console.Write(Label); }

            //if (Halted) { throw new Exception("Already halted!"); }

            while (!Halted)
            {
                long par0 = GetOC(_CurrentIdx);
                long par0op = par0 % 100;
                long origPar1 = GetOC(_CurrentIdx + 1);
                long origPar2 = GetOC(_CurrentIdx + 2);
                long origPar3 = GetOC(_CurrentIdx + 3);
                long par1, par2, par3, actualPar1, actualPar2, actualPar3;
                (par1, actualPar1) = GetActualParameter((par0 / 100) % 10, origPar1);
                (par2, actualPar2) = GetActualParameter((par0 / 1000) % 10, origPar2);
                (par3, actualPar3) = GetActualParameter((par0 / 10000) % 10, origPar3);
                //long actualPar2 = (par0 / 1000) % 10 == 0 && par2 >= 0 ? OpCodes[par2] : par2;
                //long actualPar3 = (par0 / 10000) % 10 == 0 && par3 >= 0 ? OpCodes[par3] : par3;

                int increment = 4;

                if (DEBUG_OUTPUT) { Console.Write(" [" + par0op + "]"); }

                if (par0op == OP1_PLUS)
                {
                    OpCodes[par3] = actualPar1 + actualPar2;
                }
                else if (par0op == OP2_TIMES)
                {
                    OpCodes[par3] = actualPar1 * actualPar2;
                }
                else if (par0op == OP3_READ)
                {
                    long? iInput = ReadInput();
                    if (!iInput.HasValue) { return false; }

                    OpCodes[par1] = iInput.Value;
                    increment = 2;
                    if (DEBUG_OUTPUT) { Console.Write(" :" + iInput); }
                }
                else if (par0op == OP4_PRINT)
                {
                    long signal = actualPar1;
                    Outputs.Add(signal);
                    increment = 2;
                    if (DEBUG_OUTPUT) { Console.Write(" -> " + actualPar1); }
                }
                else if (par0op == OP5_IFNOT0_GOTO)
                {
                    increment = 3;
                    if (actualPar1 != 0) { increment = (int)actualPar2 - _CurrentIdx; }
                }
                else if (par0op == OP6_IF0_GOTO)
                {
                    increment = 3;
                    if (actualPar1 == 0) { increment = (int)actualPar2 - _CurrentIdx; }
                }
                else if (par0op == OP7_IS_LESS)
                {
                    OpCodes[par3] = (actualPar1 < actualPar2) ? 1 : 0;
                    increment = 4;
                }
                else if (par0op == OP8_IS_EQ)
                {
                    OpCodes[par3] = (actualPar1 == actualPar2) ? 1 : 0;
                    increment = 4;
                }
                else if (par0op == OP9_RELATIVE_BASE_ADJUST)
                {
                    RelativeBase += actualPar1;
                    increment = 2;
                }
                else { throw new Exception("Unknown opcode " + par0op); }
                _CurrentIdx += increment;

                Halted = OpCodes[_CurrentIdx] == OP99_HALT;

                if (Targets.Any() && par0op == OP4_PRINT)
                {
                    if (DEBUG_OUTPUT) { Console.WriteLine(); }

                    Targets.ForEach(_ => _.AddSignal(Outputs.Last()));
                    //TargetAmplifier.Inputs.Add(Outputs.Last());
                    return true;

                    //break;
                }
            }

            if (Halted)
            {
                if (DEBUG_OUTPUT) { Console.WriteLine(); }
            }

            return !Halted;
        }



        private (long, long) GetActualParameter(long mode, long par)
        {
            long resultIdx = -1;
            switch (mode)
            {
                case 0: resultIdx = par; break;
                case 1: return (par, par);
                case 2: resultIdx = par + RelativeBase; break;
                default: return (0, 0);
            }

            if (resultIdx < 0) { return (par, 0); }

            return (resultIdx, GetOC(resultIdx));
        }

        private long GetOC(long idx)
        {
            return OpCodes.GetValueOrDefault(idx, 0);
        }



        public CommonAmp(CommonAmp a, bool copyInputsOutputs)
        {
            this.OpCodes = new Dictionary<long, long>(a.OpCodes.ToList());
            this.Outputs = a.Outputs;
            this._CurrentIdx = a._CurrentIdx;
            this.Halted = a.Halted;
            this.RelativeBase = a.RelativeBase;

            if (copyInputsOutputs)
            {
                this.Inputs = a.Inputs.ToList();
                this.CurrentInputIdx = a.CurrentInputIdx;
                this.Outputs = a.Outputs.ToList();
            }


            string lbl = a.Label;
            var tokens = lbl.Split('_');
            if (tokens.Count() > 1)
            {
                lbl = tokens[0] + "_" + ((int.Parse(tokens[1])) + 1);
            }
            else
            {
                lbl = a.Label + "_1";
            }
            this.Label = lbl;
        }
    }

    public class InputNotYetGivenException : Exception { }
}
