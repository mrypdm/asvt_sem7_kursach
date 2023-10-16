using System;
using Argument;

namespace Executor{

    public record Argument{
        
    }
    public class Command {
        private Argument Arguments[2];
        public State Execute(State CurrentState);

    }

    public int MatchArgumentsNumber(ushort opcode){

    }


}
