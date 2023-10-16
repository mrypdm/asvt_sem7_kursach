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
        private Command CurrentCommand;
        public int ExecuteProgram();

        private int ReadCommand(){
            this.CurrentCommand = Command(this.CurrentState.MemoryAddressRegister);
            
        }
        public int ExecuteNextInstruction(){
            this.CurrentState.MemoryAddressRegister = this.CurrentState.R[7];
            this.CurrentState.R[7] += 2;
            this.ReadCommand(this.CurrentState[7]);

        }
        public int LoadProgram(string filename){
            return 0;
        }
    }
}
