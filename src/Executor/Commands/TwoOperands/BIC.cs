using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.TwoOperands;

public class BIC : TwoOperand
{
    public BIC(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<IRegisterArgument<ushort>>(arguments);
        var (source0, destination0) = validatedArguments[0].GetSourceAndDestination();
        var (source1, destination1) = validatedArguments[1].GetSourceAndDestination();

        var value = (ushort)(source1() & ~source0());

        destination1(value);
        _state.Z = value == 0;
        _state.N = (value & 0b1000_0000_0000_0000) != 0;
        _state.V = false;
    }

    public override ushort Opcode => Convert.ToUInt16("040000", 8);
}