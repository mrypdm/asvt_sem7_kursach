namespace Executor;

public class SOB: ICommand {

    private Memory memory;
    private State state;
    private ushort OpcodeMask = 0b1111_1110_0000_0000;
    private ushort RegisterMask = 0b0000_0001_1100_0000;
    private ushort OffsetMask = 0b0000_0000_0011_1111;

    public ushort GetRegister(ushort word) {
      return (ushort)((word & RegisterMask) >> 6);
    }
    public ushort GetOffset(ushort word) {
      return (ushort)(word & OffsetMask);
    }
    public ushort GetOpcodeByMask(ushort word) {
      return (ushort)(word & OpcodeMask);
    }

    public IArgument[] GetArguments(ushort word) {
      IArgument[] args = new SOBArg[1];
      args[0] = new SOBArg(GetRegister(word), GetOffset(word), state, memory);
      return args;
    }
    public void Execute(IArgument[] arguments) {
      return;
    }

    public ushort Opcode => 0b0111_1110_0000_0000;

    public SOB(State state, Memory memory) {
      this.memory = memory;
      this.state = state;
    }
}