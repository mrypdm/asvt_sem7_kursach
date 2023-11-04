using System;

namespace Executor{
    interface ICommand{
        void Execute (IArgument[] arguments);
        IArgument[] GetArguments(ushort word);
    }
    
    abstract class TwoOperands : ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = (ushort)0b1111_0000_0000_0000;
        private ushort SourceMask1 = (ushort)0b0000_1110_0000_0000;
        private ushort SourceMask2 =  (ushort)0b0000_0000_0011_1000;
        private ushort RegisterMask1 = (ushort)0b0000_0001_1100_0000;
        private ushort RegisterMask2 =  (ushort)0b0000_0000_0000_0111;

        public ushort GetRegister1(ushort word){
            return (ushort)((word & RegisterMask1) >> 6);
        }
        public ushort GetRegister2(ushort word){
            return (ushort)(word & RegisterMask2);
        }
        public ushort GetMode2(ushort word){
            return (ushort)((word & SourceMask2) >> 3);
        }
        public ushort GetMode1(ushort word){
            return (ushort)((word & SourceMask1) >> 9);
        }
        public ushort GetOpcode(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);

        public TwoOperands (State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }

    }

    abstract class OneOperand : ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1111_1100_0000;
        private ushort SourceMask = 0b0000_0000_0011_1000;
        private ushort RegisterMask =  0b0000_0000_0000_0111;

        public ushort GetRegister(ushort word){
            return (ushort)(word & RegisterMask);
        }
        public ushort GetMode(ushort word){
            return (ushort)((word & SourceMask) >> 3);
        }
        public ushort GetOpcode(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);

        public OneOperand (State state, Memory memory){
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
            return (ushort)(word & OffsetMask);
        }
        public ushort GetOpcode(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);
        
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
            return (ushort)(word & RegisterMask);
        }
        public ushort GetOpcode(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);
        
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
            return (ushort)(word & FlagMask);
        }
        public ushort GetOpcode(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);

        public ConditionCode(State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }

}