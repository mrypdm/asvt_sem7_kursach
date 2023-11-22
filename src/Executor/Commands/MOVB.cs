namespace Executor;

public class MOV : TwoOperands
{
    private Memory memory;
    private State state;
    public MOV(State state, Memory memory) : base(state, memory) { }

    public override IArgument[] GetArguments(ushort word)
    {
        IArgument[] args = new TwoOperandsArgW[2];
        args[0] = new TwoOperandsArgB(GetMode1(word), GetRegister1(word), state, memory);
        args[1] = new TwoOperandsArgB(GetMode2(word), GetRegister2(word), state, memory);
        return args;
    }
    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        arguments[1].SetValue(value);
        return;
    }

    public override ushort Opcode => 0b1_0000_0000_0000;

}