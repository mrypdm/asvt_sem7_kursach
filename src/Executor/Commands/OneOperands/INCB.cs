using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class INCB : OneOperand
{
    public INCB(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IByteRegisterArgument>(arguments[0]);
        var value = validatedArgument.GetByte() + 1;
        validatedArgument.SetByte((byte)value);
    }

    public override ushort Opcode => Convert.ToUInt16("105200", 8);
}