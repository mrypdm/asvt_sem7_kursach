using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class SDC : OneOperand
{
    public SDC(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<ushort>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();

        var delta = _state.C ? 1 : 0;
        var value = (ushort)(source() - delta);

        destination(value);
        _state.Z = value == 0;
        _state.N = (value & 0b1000_0000_0000_0000) > 0;
        _state.V = value == 0b1000_0000_0000_0000;
        _state.C = value == 0 && delta == 1;
    }

    public override ushort Opcode => Convert.ToUInt16("005600", 8);
}