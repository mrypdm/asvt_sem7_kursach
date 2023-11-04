using System;
using Memory;
using Executor;
class SOB: ICommand{

        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1110_0000_0000;
        private ushort RegisterMask = 0b0000_0001_1100_0000;
        private ushort OffsetMask = 0b0000_0000_0011_1111;


        public SOB();

        public ushort GetRegister(ushort word){
            return (word & RegisterMask) >> 5;
        }
        public ushort GetOffset(ushort word){
            return (word & OffsetMask);
        }
        public ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }

        public Argument GetArguments(ushort word);
        public void Execute (State state, Memory memory);

        public SOB(State state, Memory memory){
            this.memory = memory;
            this.state = state;
        }
}

class MOV: TwoOperands{
   public MOV(State state, Memory memory): base(state, memory){}
   public Argument GetArguments(ushort word){
    
   }
   public void Execute (State state, Memory memory);

}