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
        var value = (ushort)(validatedArguments[1].GetWord() & ~validatedArguments[0].GetWord());

        validatedArguments[1].SetWord(value);
        _state.SetFlag(Flag.Z, value == 0);
        _state.SetFlag(Flag.N, (value & 0b1000_0000_0000_0000) != 0);
        _state.SetFlag(Flag.V, false);
    }

    public override ushort Opcode => Convert.ToUInt16("040000", 8);
}