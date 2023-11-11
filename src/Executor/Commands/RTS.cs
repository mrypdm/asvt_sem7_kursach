namespace Executor;

public class RTS: ICommand {

    private Memory memory;
    private State state;
    private ushort OpcodeMask = 0b1111_1111_1111_1000;
    private ushort RegisterMask = 0b0000_0000_0000_0111;

    public ushort GetRegister(ushort word) {
      return (ushort)((word & RegisterMask));
    }
    public ushort GetOpcodeByMask(ushort word) {
      return (ushort)(word & OpcodeMask);
    }

    public IArgument[] GetArguments(ushort word) {
      IArgument[] args = new RTSArg[1];
      args[0] = new RTSArg(GetRegister(word), state, memory);
      return args;
    }
    public void Execute(IArgument[] arguments) {
      return;
    }

    public ushort Opcode => 0b0000_0000_1000_0000;

    public RTS(State state, Memory memory) {
      this.memory = memory;
      this.state = state;
    }
}
