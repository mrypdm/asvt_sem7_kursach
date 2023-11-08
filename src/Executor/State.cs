using System;

namespace Executor{
    public class State {
        
        public ushort ProcessorStateWord; // flags
        public ushort MemoryAddressRegister; // РАП
        public ushort[] R = new ushort[8];

        public void SetFlag(){
            
        }

        public State(){
            Array.Fill(this.R, (ushort)0);
            ProcessorStateWord = 0;
            MemoryAddressRegister = 0;
        }

    }

}