using System;

namespace Executor{
    public interface ICommand{
        void Execute (IArgument[] arguments);
        
        IArgument[] GetArguments(ushort word);

        ushort GetOpcode();
    }
    
    public abstract class TwoOperands : ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_0000_0000_0000;
        private ushort SourceMask1 = 0b0000_1110_0000_0000;
        private ushort SourceMask2 = 0b0000_0000_0011_1000;
        private ushort RegisterMask1 = 0b0000_0001_1100_0000;
        private ushort RegisterMask2 = 0b0000_0000_0000_0111;

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
        public ushort GetOpcodeByMask(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);

        public abstract ushort GetOpcode();

        public TwoOperands (State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }

    }

    public abstract class OneOperand : ICommand{
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
        public ushort GetOpcodeByMask(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);

        public abstract ushort GetOpcode();

        public OneOperand (State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }

    public abstract class BranchOperationC : ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1111_0000_0000;
        private ushort OffsetMask = 0b0000_0000_1111_1111;

        public ushort GetOffset(ushort word){
            return (ushort)(word & OffsetMask);
        }
        public ushort GetOpcodeByMask(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);

        public abstract ushort GetOpcode();
        
        public BranchOperationC(State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }


    public abstract class FloatingInstructionSet: ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1111_1111_1000;
        private ushort RegisterMask = 0b0000_0000_0000_0111;

        public ushort GetRegister(ushort word){
            return (ushort)(word & RegisterMask);
        }
        public ushort GetOpcodeByMask(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);

        public abstract ushort GetOpcode();
        
        public FloatingInstructionSet(State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }
    
    public abstract class ConditionCode: ICommand{
        private Memory memory;
        private State state;
        private ushort OpcodeMask = 0b1111_1111_1111_0000;
        private ushort FlagMask = 0b0000_0000_0000_1111;

        public ushort GetRegister(ushort word){
            return (ushort)(word & FlagMask);
        }
        public ushort GetOpcodeByMask(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);

        public abstract ushort GetOpcode();

        public ConditionCode(State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }

     public abstract class BitOperations: ICommand{
        private Memory memory;
        private State state;

        private ushort OpcodeMask = 0b1111_1110_0000_0000;
        private ushort Register1Mask = 0b0000_0001_1100_0000;
        private ushort ModeMask = 0b0000_0000_0011_1000;
        private ushort Register2Mask = 0b0000_0000_0000_0111;


        public ushort GetRegister1(ushort word){
            return (ushort)((word & Register1Mask) >> 6);
        }


        public ushort GetRegister2(ushort word){
            return (ushort)((word & Register2Mask));
        }

        public ushort GetMode(ushort word){
            return (ushort)(word & ModeMask);
        }
        public ushort GetOpcode(ushort word){
            return (ushort)(word & OpcodeMask);
        }
        public abstract void Execute (IArgument[] arguments);

        public abstract IArgument[] GetArguments(ushort word);

        public abstract ushort GetOpcode();

        public BitOperations(State state, Memory memory){
            this.state = state;
            this.memory = memory;
        }
    }

}