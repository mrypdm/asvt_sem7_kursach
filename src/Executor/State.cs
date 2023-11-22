using System;

namespace Executor{

    public enum Flag{
        Z,
        N,
        V, 
        C
    }

    public class State {
        
        public ushort ProcessorStateWord; // flags
        public ushort MemoryAddressRegister; // РАП
        public ushort[] R;
        
        public void SetFlag(Flag flag, bool val){
            int value = val ? 1 : 0; 
           
            switch(flag){
                case Flag.Z:
                    ProcessorStateWord &=0b1111_1111_1111_1011;
                    ProcessorStateWord |=(ushort)(value << 2);
                    break;
                case Flag.N:
                    ProcessorStateWord &=0b1111_1111_1111_0111;
                    ProcessorStateWord |=(ushort)(value << 3);
                    break;
                case Flag.V:
                    ProcessorStateWord &=0b1111_1111_1111_1101;
                    ProcessorStateWord |=(ushort)(value << 1 );
                    break;
                case Flag.C:
                    ProcessorStateWord &=0b1111_1111_1111_1110;
                    ProcessorStateWord |=(ushort)(value);
                    break;                
            }
            Console.WriteLine($"PSW {ProcessorStateWord}.");
        
        }

        public int GetFlag(Flag flag){
            int FlagValue = -1;

            switch(flag){
                case Flag.Z:
                    FlagValue = (ProcessorStateWord & 0b100) >> 2;
                    break;
                case Flag.N:
                    FlagValue = (ProcessorStateWord & 0b1000) >> 3;
                    break;
                case Flag.V:
                    FlagValue = (ProcessorStateWord & 0b10) >> 1;
                    break;
                case Flag.C:
                    FlagValue = (ProcessorStateWord & 0b1);
                    break;
                                    
            }
            return FlagValue;
        }

        public State(){
            this.R = new ushort[8];
            Array.Fill(this.R, (ushort)0);
            ProcessorStateWord = 0;
            MemoryAddressRegister = 0;
        }

    }

}