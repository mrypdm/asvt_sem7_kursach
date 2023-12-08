using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class ADC : OneOperand
{
    public ADC(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IWordRegisterArgument>(arguments);
        var delta = _state.GetFlag(Flag.C) ? 1 : 0;
        var oldValue = validatedArgument.GetWord();
        var value = (byte)(oldValue + delta);

        validatedArgument.SetWord(value);
        _state.SetFlag(Flag.Z, value == 0);
        _state.SetFlag(Flag.N, (value & 0b1000_0000_0000_0000) != 0);
        _state.SetFlag(Flag.V, oldValue == Convert.ToUInt16("077777", 8) && delta == 1);
        _state.SetFlag(Flag.C, oldValue == Convert.ToUInt16("177777", 8) && delta == 1);
    }

    public override ushort Opcode => Convert.ToUInt16("005500", 8);
}