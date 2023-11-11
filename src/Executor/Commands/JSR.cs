namespace Executor;

public class JSR: ICommand {

    private Memory memory;
    private State state;
    private ushort OpcodeMask = 0b1111_1110_0000_0000;
    private ushort Register1Mask = 0b0000_0001_1100_0000;
    private ushort ModeMask = 0b0000_0000_0011_1000;
    private ushort Register2Mask = 0b0000_0000_0000_0111;

    public ushort GetRegister1(ushort word) {
      return (ushort)((word & Register1Mask) >> 6);
    }

    public ushort GetRegister2(ushort word) {
      return (ushort)((word & Register2Mask));
    }

    public ushort GetMode(ushort word) {
      return (ushort)(word & ModeMask);
    }
    public ushort GetOpcode(ushort word) {
      return (ushort)(word & OpcodeMask);
    }

    public IArgument[] GetArguments(ushort word) {
      IArgument[] args = new JSRnBITArg[1];
      args[0] = new JSRnBITArg(GetRegister1(word), GetMode(word), GetRegister2(word), state, memory);
      return args;
    }
    public void Execute(IArgument[] arguments) {
      return;
    }

    public ushort Opcode => 0b0000_1000_0000_0000;

    public JSR(State state, Memory memory) {
      this.memory = memory;
      this.state = state;
    }
}
