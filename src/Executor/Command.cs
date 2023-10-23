using System;
using OpcodeFiller;

namespace Executor{
    public class OpcodeIndentifyer {

        private OpcodeIndentifyer Instance;
        private Dictionary<string, string>[8] OpcodesDictionary;
        
        public OpcodeIndentifyer GetInstance(){
            if (Instance == null){
                Instance = new OpcodeIndentifyer()
            }
            return Instance
        }
        private OpcodeIndentifyer(){
            
        }
        public GetCommandName(ushort Word){
            
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
