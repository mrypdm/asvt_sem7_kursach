using System;

namespace Executor{
    public class State {
        
        // public State Instance;
        public ushort ProcessorStateWord; // flags
        public ushort MemoryAddressRegister; // РАП
        public ushort[] R = new ushort[8];

        // public static State GetInstance(){
        //     if (Instance == null){
        //         Instance = new State();
        //     }
        //     return Instance;
        // }

        public void SetFlag(){
            
        }

        public State(){
            Array.Fill(this.R, (ushort)0);
            ProcessorStateWord = 0;
            MemoryAddressRegister = 0;
        }

    }

}