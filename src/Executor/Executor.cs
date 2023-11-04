using System;

namespace Executor{

    public class Executor {
        
        private static Executor Instance;

        public static Executor GetInstance(){
            if (Instance == null){
                Instance = new Executor(State.GetInstance());
            }
            return Instance;
        }
        private Executor(State NewState){
            CurrentState = NewState;
        }

        private State CurrentState;
        
        public int ExecuteProgram(){
            return 0;
        }

        private int ReadCommand(ushort addr){
            return 0;
        }
        public int ExecuteNextInstruction(){
            CurrentState.MemoryAddressRegister = CurrentState.R[7];
            CurrentState.R[7] += 2;
            ReadCommand(CurrentState.R[7]);
            return 0;
        }
        public int LoadProgram(string filename){
            return 0;
        }
    }
}
