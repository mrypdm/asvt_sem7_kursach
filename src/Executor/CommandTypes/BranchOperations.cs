namespace Executor;

public abstract class BranchOperationC: ICommand {
    private Memory memory;
    private State state;
    private ushort OpcodeMask = 0b1111_1111_0000_0000;
    private ushort OffsetMask = 0b0000_0000_1111_1111;

    public ushort GetOffset(ushort word) {
      return (ushort)(word & OffsetMask);
    }
    public ushort GetOpcodeByMask(ushort word) {
      return (ushort)(word & OpcodeMask);
    }
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode {
      get;
    }

    public BranchOperationC(State state, Memory memory) {
      this.state = state;
      this.memory = memory;
    }
  }