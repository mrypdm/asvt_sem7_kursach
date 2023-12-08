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
        var validatedArguments = ValidateArguments<IWordRegisterArgument>(arguments);
        validatedArguments[1].SetValue((ushort)(validatedArguments[1].GetWord() & ~validatedArguments[0].GetWord()));
    }

    public override ushort Opcode => Convert.ToUInt16("040000", 8);
}