using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class SDC : OneOperand
{
    public SDC(IMemory memory, IState state) : base(memory, state)
    {
    }


    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IWordRegisterArgument>(arguments[0]);
        var delta = _state.GetFlag(Flag.C) ? 1 : 0;
        var value = (ushort)(validatedArgument.GetWord() - delta);
        
        validatedArgument.SetWord(value);
        _state.SetFlag(Flag.Z, value == 0);
        _state.SetFlag(Flag.N, (value & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, value == 0b1000_0000_0000_0000);
        _state.SetFlag(Flag.C, value == 0 && delta == 1);
    }

    public override ushort Opcode => Convert.ToUInt16("005600", 8);
}