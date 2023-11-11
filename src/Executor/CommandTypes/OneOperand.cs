namespace Executor;

public abstract class OneOperand: ICommand {
    private Memory memory;
    private State state;
    private ushort OpcodeMask = 0b1111_1111_1100_0000;
    private ushort SourceMask = 0b0000_0000_0011_1000;
    private ushort RegisterMask = 0b0000_0000_0000_0111;

    public ushort GetRegister(ushort word) {
      return (ushort)(word & RegisterMask);
    }
    public ushort GetMode(ushort word) {
      return (ushort)((word & SourceMask) >> 3);
    }
    public ushort GetOpcodeByMask(ushort word) {
      return (ushort)(word & OpcodeMask);
    }
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode {
      get;
    }

    public OneOperand(State state, Memory memory) {
      this.state = state;
      this.memory = memory;
    }
}