using System;
using State;

namespace Executor{
    public static class Executor {
        
        private State State;
        public int ExecuteProgram();
        private int ExecuteNextInstruction();
    }
}
