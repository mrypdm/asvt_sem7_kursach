namespace Executor;

public abstract class BitOperations: ICommand {
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
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode {
      get;
    }

    public BitOperations(State state, Memory memory) {
      this.state = state;
      this.memory = memory;
    }
}