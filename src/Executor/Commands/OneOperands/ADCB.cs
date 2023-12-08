using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class ADCB : OneOperand
{
    public ADCB(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IByteRegisterArgument>(arguments);
        var delta = _state.GetFlag(Flag.C) ? 1 : 0;
        var oldValue = validatedArgument.GetByte();
        var value = (byte)(oldValue + delta);

        validatedArgument.SetByte(value);
        _state.SetFlag(Flag.Z, value == 0);
        // TODO byte?
        _state.SetFlag(Flag.N, (value & 0b1000_0000_0000_0000) != 0);
        _state.SetFlag(Flag.V, oldValue == Convert.ToUInt16("077777", 8) && delta == 1);
        _state.SetFlag(Flag.C, oldValue == Convert.ToUInt16("177777", 8) && delta == 1);
    }

    public override ushort Opcode => Convert.ToUInt16("105500", 8);
}