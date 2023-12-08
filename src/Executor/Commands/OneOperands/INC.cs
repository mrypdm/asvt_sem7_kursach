using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class INC : OneOperand
{
    public INC(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<ushort>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();

        var value = (byte)(source() + 1);

        destination(value);
        _state.Z = value == 0;
        _state.N = (value & 0b1000_0000_0000_0000) > 0;
        _state.V = value == Convert.ToUInt16("077777", 8);
    }

    public override ushort Opcode => Convert.ToUInt16("005200", 8);
}