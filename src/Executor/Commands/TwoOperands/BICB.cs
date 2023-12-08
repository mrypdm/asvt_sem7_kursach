using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.TwoOperands;

public class BICB : TwoOperand
{
    public BICB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<IByteRegisterArgument>(arguments);
        validatedArguments[1].SetByte((byte)(validatedArguments[0].GetByte() & ~validatedArguments[1].GetByte()));
    }

    public override ushort Opcode => Convert.ToUInt16("140000", 8);
}