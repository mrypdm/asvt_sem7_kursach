namespace Executor;

public abstract class ConditionCode: ICommand {
    private Memory memory;
    private State state;
    private ushort OpcodeMask = 0b1111_1111_1111_0000;
    private ushort FlagMask = 0b0000_0000_0000_1111;

    public ushort GetRegister(ushort word) {
      return (ushort)(word & FlagMask);
    }
    public ushort GetOpcodeByMask(ushort word) {
      return (ushort)(word & OpcodeMask);
    }
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode {
      get;
    }

    public ConditionCode(State state, Memory memory) {
      this.state = state;
      this.memory = memory;
    }
  }