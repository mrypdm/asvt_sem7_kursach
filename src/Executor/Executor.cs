using System;

namespace Executor{
    
    public record State {
        public ushort ProcessorStateWord; // flags
        public ushort MemoryAddressRegister; // РАП
        public ushort[8] R;
    }

    public static class Executor {
        
        public Executor(State NewState){
            this.CurrentState = NewState;
        }
        private State CurrentState;
        public int ExecuteProgram();

        private int ReadCommand(){
            
        }
        public int ExecuteNextInstruction(){
            this.CurrentState.MemoryAddressRegister = this.CurrentState.R[7];
            this.CurrentState.R[7] += 2;
            this.ReadCommand();
        }
        public int LoadProgram(string filename){
            return 0;
        }
    }
}
