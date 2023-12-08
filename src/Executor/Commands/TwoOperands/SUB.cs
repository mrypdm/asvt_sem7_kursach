using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.TwoOperands;

public class SUB : TwoOperand
{
    public SUB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new RegisterWordArgument(_memory, _state, GetMode1(word), GetRegister1(word)),
            new RegisterWordArgument(_memory, _state, GetMode2(word), GetRegister2(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        arguments[1].SetValue((ushort)(arguments[1].GetValue() - arguments[0].GetValue()));
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("160000", 8);
}