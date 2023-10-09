using System;
using State;

namespace Executor{
    public static class Executor {
        
        private byte Memory[256];
        private State State;
        public int ExecuteProgram();
        private int ExecuteNextInstruction();
        public int LoadProgram(){
            return 0;
        }
    }
}
