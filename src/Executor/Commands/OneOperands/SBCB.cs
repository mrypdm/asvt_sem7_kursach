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
        var validatedArgument = ValidateArgument<IRegisterArgument<byte>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();

        var delta = _state.C ? 1 : 0;
        var value = (byte)(source() - delta);

        destination(value);
        _state.Z = value == 0;
        // TODO byte?
        _state.N = (value & 0b1000_0000) > 0;
        _state.V = value == 0b1000_0000;
        _state.C = value == 0 && delta == 1;
    }

    public override ushort Opcode => Convert.ToUInt16("105600", 8);
}