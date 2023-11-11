namespace Executor;

public abstract class TwoOperands: ICommand {
    private Memory memory;
    private State state;
    private ushort OpcodeMask = 0b1111_0000_0000_0000;
    private ushort SourceMask1 = 0b0000_1110_0000_0000;
    private ushort SourceMask2 = 0b0000_0000_0011_1000;
    private ushort RegisterMask1 = 0b0000_0001_1100_0000;
    private ushort RegisterMask2 = 0b0000_0000_0000_0111;

    public ushort GetRegister1(ushort word) {
      return (ushort)((word & RegisterMask1) >> 6);
    }
    public ushort GetRegister2(ushort word) {
      return (ushort)(word & RegisterMask2);
    }
    public ushort GetMode2(ushort word) {
      return (ushort)((word & SourceMask2) >> 3);
    }
    public ushort GetMode1(ushort word) {
      return (ushort)((word & SourceMask1) >> 9);
    }
    public ushort GetOpcodeByMask(ushort word) {
      return (ushort)(word & OpcodeMask);
    }
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode {
      get;
    }
    public TwoOperands(State state, Memory memory) {
      this.state = state;
      this.memory = memory;
    }

}