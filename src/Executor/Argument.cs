namespace Executor{

    interface IArgument{
        ushort GetValue();
        void SetValue(ushort word);
    }
    class TwoOperandsArg: IArgument{
        private ushort Mode;
        private ushort Register;
        private State state;
        private Memory memory;
        TwoOperandsArg(ushort Mode, ushort Register, State state, Memory memory){
            this.memory = memory;
            this.state = state;
            this.Register = Register;
            this.Mode = Mode;
        }
        
        ushort GetValue();
        void SetValue(ushort word);
    }
    class OneOperandArg: IArgument{

        private ushort Mode;
        private ushort Register;
        private State state;
        private Memory memory;
        ushort GetValue();
        void SetValue(ushort word);
        OneOperandArg(ushort Mode, ushort Register, State state, Memory memory){
            
        }
    }
}