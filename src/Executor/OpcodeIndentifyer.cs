using System;

namespace Executor {
  public class OpcodeIndentifyer {
    private OpcodeIndentifyer Instance;
    private ushort[] Masks = {
      0b1111_0000_0000_0000,
      0b1111_1111_1100_0000,
      0b1111_1111_0000_0000,
      0b1111_1111_1111_1000,
      0b1111_1111_1111_0000,
      0b1111_1110_0000_0000,
      0b1111_1111_1111_1111
    };

    private List<ICommand> Opcodes;

    private Dictionary < ushort, ICommand > OpcodesDictionary;

    public OpcodeIndentifyer(State state, Memory memory) {
      Opcodes = new List<ICommand>{
        new MOV(state, memory),
        new MOVB(state, memory),
        new ADD(state, memory),
        new SOB(state, memory),
        new JSR(state, memory),
        new RTS(state, memory)
      };
      OpcodesDictionary = Opcodes.ToDictionary(command => command.Opcode, command => command);
    }
    public ICommand GetCommand(ushort Word) {
      ushort opcode;
      ICommand command;
      foreach(ushort mask in Masks){
        opcode = (ushort)(Word & mask);
        if (OpcodesDictionary.TryGetValue(opcode, out command)){
          return command;
        }
      }
      throw new InvalidOperationException("Invalid Operation Opcode!");
    }
  }

}