namespace Executor;

public class MOV: TwoOperands {
    private Memory memory;
    private State state;
    public MOV(State state, Memory memory): base(state, memory) {}

    public override IArgument[] GetArguments(ushort word) {
      IArgument[] args = new TwoOperandsArg[2];
      args[0] = new TwoOperandsArg(GetMode1(word), GetRegister1(word), state, memory);
      args[1] = new TwoOperandsArg(GetMode2(word), GetRegister2(word), state, memory);
      return args;
    }
    public override void Execute(IArgument[] arguments) {
      return;
    }

    public override ushort Opcode => 0b1_0000_0000_0000;

}