using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class SDCB : OneOperand
{
    public SDCB(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IByteRegisterArgument>(arguments[0]);
        var delta = _state.GetFlag(Flag.C) ? 1 : 0;
        var value = validatedArgument.GetByte() - delta;
        validatedArgument.SetByte((byte)value);
    }

    public override ushort Opcode => Convert.ToUInt16("105600", 8);
}