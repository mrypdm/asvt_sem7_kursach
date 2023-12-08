using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class ADD : TwoOperands
{
    public ADD(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override IArgument[] GetArguments(ushort word) // Исключение
    {
        return new IArgument[]
        {
            new RegisterWordArgument(_memory, _state, GetMode1(word), GetRegister1(word)),
            new RegisterWordArgument(_memory, _state, GetMode2(word), GetRegister2(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        var carry = (arguments[1].GetValue() + arguments[0].GetValue()) > 0b1111111111111111;
        var sign = ((arguments[1].GetValue() ^ arguments[0].GetValue()) & 0b1000_0000_0000_0000) == 0;
        arguments[1].SetValue((ushort)(arguments[1].GetValue() + arguments[0].GetValue()));
        _state.SetFlag(Flag.Z, arguments[1].GetValue() == 0);
        _state.SetFlag(Flag.N, (arguments[1].GetValue() & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, sign && (_state.GetFlag(Flag.N) != (arguments[0].GetValue() & 0b1000_0000_0000_0000) < 0));
        _state.SetFlag(Flag.C, carry);
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("060000", 8);

}