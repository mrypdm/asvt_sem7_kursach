namespace Executor;

public abstract class FloatingInstructionSet: ICommand {
    private Memory memory;
    private State state;
    private ushort OpcodeMask = 0b1111_1111_1111_1000;
    private ushort RegisterMask = 0b0000_0000_0000_0111;

    public ushort GetRegister(ushort word) {
      return (ushort)(word & RegisterMask);
    }
    public ushort GetOpcodeByMask(ushort word) {
      return (ushort)(word & OpcodeMask);
    }
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode {
      get;
    }

    public FloatingInstructionSet(State state, Memory memory) {
      this.state = state;
      this.memory = memory;
    }
  }