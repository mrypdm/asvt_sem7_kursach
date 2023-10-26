class SOB: ICommand{
        private ushort OpcodeMask = 0b1111_1110_0000_0000;
        private ushort RegisterMask = 0b0000_0001_1100_0000;
        private ushort OffsetMask = 0b0000_0000_0011_1111;


        public SOB();

        private ushort GetRegister(ushort word){
            return (word & RegisterMask) >> 5;
        }
        private ushort GetOffset(ushort word){
            return (word & OffsetMask);
        }
        private ushort GetOpcode(ushort word){
            return (word & OpcodeMask);
        }

        public Argument GetArguments(ushort word){
            

        }
        public void Execute (State state, Memory memory){

        }

    }
