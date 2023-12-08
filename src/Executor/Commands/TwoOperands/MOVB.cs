using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.TwoOperands;

public class MOVB : TwoOperand
{
    public MOVB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<IByteRegisterArgument>(arguments);
        validatedArguments[1].SetByte(validatedArguments[0].GetByte());
    }

    public override ushort Opcode => Convert.ToUInt16("110000", 8);
}