namespace Executor;

public abstract class TrapReturn: ICommand {
    private Memory memory;
    private State state;

    private ushort OpcodeMask = 0b1111_1111_1111_1111;

    public ushort GetOpcode(ushort word) {
      return (ushort)(word & OpcodeMask);
    }
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode {
      get;
    }

    public TrapReturn(State state, Memory memory) {
      this.state = state;
      this.memory = memory;
    }
}