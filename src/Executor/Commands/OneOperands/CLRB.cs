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
        var validatedArgument = ValidateArgument<IByteRegisterArgument>(arguments[0]);
        validatedArgument.SetByte(0);
    }

    public override ushort Opcode => Convert.ToUInt16("105000", 8);
}