using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.TwoOperands;

public class BISB : TwoOperand
{
    public BISB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<IByteRegisterArgument>(arguments);
        var value = (byte)(validatedArguments[0].GetByte() | validatedArguments[1].GetByte());

        validatedArguments[1].SetValue(value);
        _state.SetFlag(Flag.Z, value == 0);
        _state.SetFlag(Flag.N, (value & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, false);
    }

    public override ushort Opcode => Convert.ToUInt16("150000", 8);
}