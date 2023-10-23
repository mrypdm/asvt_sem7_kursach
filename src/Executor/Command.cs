using System;
using OpcodeFiller;

namespace Executor{
    public class OpcodeIndentifyer {

        private OpcodeIndentifyer Instance;
        private Dictionary<string, string>[10] OpcodesDictionary;
        
        public OpcodeIndentifyer GetInstance(){
            if (Instance == null){
                Instance = new OpcodeIndentifyer();
            }
            return Instance;
        }
        private OpcodeIndentifyer(){
            OneOperandFill(OpcodesDictionary[0]);
            TwoOperandsFill(OpcodesDictionary[1]);
            BranchOperationC0Fill(OpcodesDictionary[2]);
            BranchOperationC1Fill(OpcodesDictionary[3]);
            TrapFill(OpcodesDictionary[4]);
            ConditionCodeFill(OpcodesDictionary[5]);
            TrapInstructionsFill(OpcodesDictionary[6]);
            FloatingInstructionSetFill(OpcodesDictionary[7]);
            SubroutineInstruction0Fill(OpcodesDictionary[8]);
            SubroutineInstruction1Fill(OpcodesDictionary[9]);
        }
        public string GetCommandName(ushort Word){
            ushort mask;
            string CommandName;
            ushort opcode;

            foreach(Dictionary<string, string> Dict in OpcodesDictionary){
                mask = Int32.parse(Dict["mask"]);
                opcode = Word & mask;
                if (Dict.TryGetValue(Word, out CommandName)){
                    return CommandName;
                }
            }
            throw new InvalidOperationException("Invalid Operation!");
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
