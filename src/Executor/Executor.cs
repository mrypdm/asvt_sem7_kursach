using System;

namespace Executor{

    
    public class State {
        
        private static State Instance;
        public ushort ProcessorStateWord; // flags
        public ushort MemoryAddressRegister; // РАП
        public ushort[8] R;

        public State GetInstance(){
            if (Instance == null){
                Instance = new State();
            }
            return Instance;
        }

        public SetFlag();

        private State(){
            Array.Fill(R, 0);
            ProcessorStateWord = 0;
            MemoryAddressRegister = 0;
        }

    }

    public static class Executor {
        
        private static Executor Instance;

        public Executor GetInstance(){
            if (Instance == null){
                Instance = new Executor(State.GetInstance());
            }
            
        }
        private Executor(State NewState){
            CurrentState = NewState;
        }

        private State CurrentState;
        
        private Command CurrentCommand;
        
        public int ExecuteProgram();

        private int ReadCommand(){
            CurrentCommand = Command(CurrentState.MemoryAddressRegister);
            
        }
        public int ExecuteNextInstruction(){
            CurrentState.MemoryAddressRegister = CurrentState.R[7];
            CurrentState.R[7] += 2;
            ReadCommand(CurrentState[7]);

        }
        public int LoadProgram(string filename){
            return 0;
        }
    }
}
