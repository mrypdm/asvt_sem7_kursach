using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class JMP : OneOperand
{
    public JMP(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IWordRegisterArgument>(arguments[0]);
        _state.Registers[7] = validatedArgument.GetWord();
    }

    public override ushort Opcode => Convert.ToUInt16("000100", 8);
}