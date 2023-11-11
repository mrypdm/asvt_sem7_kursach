namespace Executor;

public abstract class TrapInstruction: ICommand {
    private Memory memory;
    private State state;

    private ushort OpcodeMask = 0b1111_1111_0000_0000;

    private ushort OperationCodeMask = 0b0000_0000_1111_1111;

    public ushort GetOpcode(ushort word) {
      return (ushort)(word & OpcodeMask);
    }

    public ushort GetOperationCode(ushort word) {
      return (ushort)(word & OperationCodeMask);
    }
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode {
      get;
    }

    public TrapInstruction(State state, Memory memory) {
      this.state = state;
      this.memory = memory;
    }
}