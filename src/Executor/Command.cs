using System;
using Argument;

namespace Executor{
    public class OpcodeIndentifyer {

        private OpcodeIndentifyer Instance;

        private Dictionary<string, string> TwoOperands;
        private Dictionary<string, string> OneOperand;
        private Dictionary<string, string> BranchOperationC0;
        private Dictionary<string, string> BranchOperationC1;
        public OpcodeIndentifyer GetInstance(){
            if (Instance == null){
                Instance = new OpcodeIndentifyer()
            }
            return Instance
        }
        private OpcodeIndentifyer(){
            
        }
        public GetCommandName(){
            
        }
    }

    public record Argument{
        
    }
    public class Command {
        private Argument Arguments[2];
        public State Execute(State CurrentState);

        public Command(Memory memory, ushort address){

        }

    }

}
