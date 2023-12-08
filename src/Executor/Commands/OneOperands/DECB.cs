using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class DECB : OneOperand
{
    public DECB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<byte>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();
        
        var value = (byte)(source() - 1);

        destination(value);
        _state.Z = value == 0;
        // TODO byte?
        _state.N = (value & 0b1000_0000) > 0;
        _state.V = value == 0b1000_0000;
    }

    public override ushort Opcode => Convert.ToUInt16("105300", 8);
}