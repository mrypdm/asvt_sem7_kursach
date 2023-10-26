using System;

namespace Executor{
    interface ICommand{
        void Execute (State state, Memory memory);
    }
    
    abstract class TwoOperands : ICommand{
        private ushort OpcodeMask = 0b1111_0000_0000_0000;
        private ushort SourceMask1 = 0b0000_1110_0000_0000;
        private ushort SourceMask2 =  0b0000_0000_0011_1000;
        private ushort RegisterMask1 = 0b0000_0001_1100_0000;
        private ushort RegisterMask2 =  0b0000_0000_0000_0111;

        private ushort GetRegister1(ushort word){
            return (word & RegisterMask1) >> 6;
        }
        private ushort GetRegister2(ushort word){
            return word & RegisterMask2;
        }
        private ushort GetSource2(ushort word){
            return (word & SourceMask2) >> 3;
        }
        private ushort GetSource1(ushort word){
            return (word & SourceMask1) >> 9;
        }
        private ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public virtual void Execute (State state, Memory memory);
    }

    abstract class OneOperand : ICommand{
        private ushort OpcodeMask = 0b1111_1111_1100_0000;
        private ushort SourceMask = 0b0000_0000_0011_1000;
        private ushort RegisterMask =  0b0000_0000_0000_0111;

        private ushort GetRegister(ushort word){
            return (word & RegisterMask);
        }
        private ushort GetSource(ushort word){
            return (word & SourceMask) >> 3;
        }
        private ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public abstract void Execute (State state, Memory memory);
    }

    abstract class BranchOperationC : ICommand{
        private ushort OpcodeMask = 0b1111_1111_0000_0000;
        private ushort OffsetMask = 0b0000_0000_1111_1111;

        private ushort GetOffset(ushort word){
            return (word & OffsetMask);
        }
        private ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public virtual void Execute (State state, Memory memory);
    }


    abstract class FloatingInstructionSet: ICommand{
        private ushort OpcodeMask = 0b1111_1111_1111_1000;
        private ushort RegisterMask = 0b0000_0000_0000_0111;

        private ushort GetRegister(ushort word){
            return (word & RegisterMask);
        }
        private ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public virtual void Execute (State state, Memory memory);
    }
    
    abstract class ConditionCode: ICommand{
        private ushort OpcodeMask = 0b1111_1111_1111_0000;
        private ushort FlagMask = 0b0000_0000_0000_1111;

        private ushort GetRegister(ushort word){
            return (word & FlagMask);
        }
        private ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public virtual void Execute (State state, Memory memory);
    }

}