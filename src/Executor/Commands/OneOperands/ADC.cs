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
        var validatedArgument = ValidateArgument<IWordRegisterArgument>(arguments[0]);
        var delta = _state.GetFlag(Flag.C) ? 1 : 0;
        var value = validatedArgument.GetWord() + delta;
        validatedArgument.SetWord((ushort)value);
    }

    public override ushort Opcode => Convert.ToUInt16("005500", 8);
}