using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.TwoOperands;

public class BIS : TwoOperand
{
    public BIS(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<IWordRegisterArgument>(arguments);
        validatedArguments[1].SetWord((ushort)(validatedArguments[0].GetWord() | validatedArguments[1].GetWord()));
    }

    public override ushort Opcode => Convert.ToUInt16("050000", 8);
}