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
        public TwoOperandsArg(ushort Mode, ushort Register, State state, Memory memory){
            this.memory = memory;
            this.state = state;
            this.Register = Register;
            this.Mode = Mode;
        }
        
        public ushort GetValue(){
            return 0;
        }
        public void SetValue(ushort word){
            return;
        }
    }
    class OneOperandArg: IArgument{

        private ushort Mode;
        private ushort Register;
        private State state;
        private Memory memory;
        public ushort GetValue(){
            return 0;
        }
        public void SetValue(ushort word){
            return;
        }
        public OneOperandArg(ushort Mode, ushort Register, State state, Memory memory){
            this.memory = memory;
            this.state = state;
            this.Register = Register;
            this.Mode = Mode;
        }
    }
    class SOBArg: IArgument{

        private ushort Offset;
        private ushort Register;
        private State state;
        private Memory memory;
        public ushort GetValue(){
            return 0;
        }
        public void SetValue(ushort word){
            return;
        }
        public SOBArg(ushort Register, ushort Offset, State state, Memory memory){
            this.memory = memory;
            this.state = state;
            this.Register = Register;
            this.Offset = Offset;
        }
    }
}