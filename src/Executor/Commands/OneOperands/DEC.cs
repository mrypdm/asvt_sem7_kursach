using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class DEC : OneOperand
{
    public DEC(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<ushort>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();
        
        var value = (ushort)(source() - 1);

        destination(value);
        _state.SetFlag(Flag.Z, value == 0);
        _state.SetFlag(Flag.N, (value & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, value == Convert.ToUInt16("100000", 8));
    }

    public override ushort Opcode => Convert.ToUInt16("005300", 8);
}