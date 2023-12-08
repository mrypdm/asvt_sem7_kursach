using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class INCB : OneOperand
{
    public INCB(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IByteRegisterArgument>(arguments[0]);
        var value = (byte)(validatedArgument.GetByte() + 1);

        validatedArgument.SetByte(value);
        _state.SetFlag(Flag.Z, value == 0);
        _state.SetFlag(Flag.N, (value & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, value == Convert.ToUInt16("077777", 8));
    }

    public override ushort Opcode => Convert.ToUInt16("105200", 8);
}