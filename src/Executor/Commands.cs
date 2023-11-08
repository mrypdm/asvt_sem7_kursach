using System;

namespace Executor{
    public class SOB: ICommand{

        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1110_0000_0000;
        private ushort RegisterMask = 0b0000_0001_1100_0000;
        private ushort OffsetMask = 0b0000_0000_0011_1111;


        public ushort GetRegister(ushort word){
            return (ushort)((word & RegisterMask) >> 6);
        }
        public ushort GetOffset(ushort word){
            return (ushort)(word & OffsetMask);
        }
        public ushort GetOpcodeByMask(ushort word){
            return (ushort)(word & OpcodeMask);
        }

        public IArgument[] GetArguments(ushort word){
            IArgument[] args = new SOBArg[1];
            args[0] = new SOBArg(GetRegister(word), GetOffset(word), state, memory);
            return args;
        }
        public void Execute (IArgument[] arguments){
            return;
        }

        public ushort GetOpcode(){
            return 0b0111_1110_0000_0000;
        }

        public SOB(State state, Memory memory){
            this.memory = memory;
            this.state = state;
        }
}

    public class MOV: TwoOperands{
        private Memory memory;
        private State state;
        public MOV(State state, Memory memory): base(state, memory){}

        public override IArgument[] GetArguments(ushort word){
            IArgument[] args = new TwoOperandsArg[2];
            args[0] = new TwoOperandsArg(GetMode1(word), GetRegister1(word), state, memory);
            args[1] = new TwoOperandsArg(GetMode2(word), GetRegister2(word), state, memory);
            return args;
        }
        public override void Execute (IArgument[] arguments){
            return;
        }

        public ushort GetOpcode(){
            return 0b1_0000_0000_0000;
        }


    }
}
