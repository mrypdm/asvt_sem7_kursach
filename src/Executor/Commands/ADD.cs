namespace Executor;

public class ADD: TwoOperands {
    private Memory memory;
    private State state;
    public ADD(State state, Memory memory): base(state, memory) {}

    public override IArgument[] GetArguments(ushort word) {
      IArgument[] args = new TwoOperandsArg[2];
      args[0] = new TwoOperandsArg(GetMode1(word), GetRegister1(word), state, memory);
      args[1] = new TwoOperandsArg(GetMode2(word), GetRegister2(word), state, memory);
      return args;
    }
    public override void Execute(IArgument[] arguments) {
      
    }

    public override ushort Opcode => 0b110_0000_0000_0000;

}