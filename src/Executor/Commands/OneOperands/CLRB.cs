using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class CLRB : OneOperand
{
    public CLRB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<byte>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();
        
        destination(0);
        _state.SetFlag(Flag.Z, true);
        _state.SetFlag(Flag.V, false);
        _state.SetFlag(Flag.C, false);
        _state.SetFlag(Flag.N, false);
    }

    public override ushort Opcode => Convert.ToUInt16("105000", 8);
}