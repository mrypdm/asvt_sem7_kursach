using System;
using Memory;

namespace Executor{
    interface ICommand{
        void Execute (IArgument[] arguments);
        IArgument[] GetArgs(ushort word);
    }
    
    abstract class TwoOperands : ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_0000_0000_0000;
        private ushort SourceMask1 = 0b0000_1110_0000_0000;
        private ushort SourceMask2 =  0b0000_0000_0011_1000;
        private ushort RegisterMask1 = 0b0000_0001_1100_0000;
        private ushort RegisterMask2 =  0b0000_0000_0000_0111;

        public ushort GetRegister1(ushort word){
            return (word & RegisterMask1) >> 6;
        }
        public ushort GetRegister2(ushort word){
            return word & RegisterMask2;
        }
        public ushort GetSource2(ushort word){
            return (word & SourceMask2) >> 3;
        }
        public ushort GetSource1(ushort word){
            return (word & SourceMask1) >> 9;
        }
        public ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public virtual void Execute (IArgument[] arguments);

        public virtual IArgument[] GetArgs(ushort word);

    }

    abstract class OneOperand : ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1111_1100_0000;
        private ushort SourceMask = 0b0000_0000_0011_1000;
        private ushort RegisterMask =  0b0000_0000_0000_0111;

        public ushort GetRegister(ushort word){
            return (word & RegisterMask);
        }
        public ushort GetSource(ushort word){
            return (word & SourceMask) >> 3;
        }
        public ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public virtual void Execute (IArgument[] arguments);

        public virtual IArgument[] GetArgs(ushort word);

        public TwoOperands(State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }

    abstract class BranchOperationC : ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1111_0000_0000;
        private ushort OffsetMask = 0b0000_0000_1111_1111;

        public ushort GetOffset(ushort word){
            return (word & OffsetMask);
        }
        public ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public virtual void Execute (IArgument[] arguments);

        public virtual IArgument[] GetArgs(ushort word);
        
        public BranchOperationC(State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }


    abstract class FloatingInstructionSet: ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1111_1111_1000;
        private ushort RegisterMask = 0b0000_0000_0000_0111;

        public ushort GetRegister(ushort word){
            return (word & RegisterMask);
        }
        public ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public virtual void Execute (IArgument[] arguments);

        public virtual IArgument[] GetArgs(ushort word);
        
        public FloatingInstructionSet(State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }
    
    abstract class ConditionCode: ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1111_1111_0000;
        private ushort FlagMask = 0b0000_0000_0000_1111;

        public ushort GetRegister(ushort word){
            return (word & FlagMask);
        }
        public ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }
        public virtual void Execute (IArgument[] arguments);

        public virtual IArgument[] GetArgs(ushort word);

        public ConditionCode(State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }

}