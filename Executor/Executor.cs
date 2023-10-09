using System;
using State;

namespace Executor{
    
    public record State {
        public ushort PSW; // flags
        public ushort RIP; 
        public ushort MDR; // РДП
        public ushort MAR; // РАП
        public ushort[8] R;
    }

    public static class Executor {
        
        public Executor(State NewState){
            this.CurrentState = NewState;
        }
        private byte[256] Memory;
        private State CurrentState;
        public int ExecuteProgram();

        private int ReadCommand(){
            this.CurrentState.MDR = // ...
        }
        private int ExecuteNextInstruction(){
            this.CurrentState.MAR = this.CurrentState.R[7];
            this.CurrentState.R[7] += 2;
            this.ReadCommand();
        }
        public int LoadProgram(string filename){
            return 0;
        }
    }
}
