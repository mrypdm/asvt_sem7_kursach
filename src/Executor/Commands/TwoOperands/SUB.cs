using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.TwoOperands;

public class SUB : TwoOperand
{
    public SUB(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new RegisterWordArgument(_memory, _state, GetMode1(word), GetRegister1(word)),
            new RegisterWordArgument(_memory, _state, GetMode2(word), GetRegister2(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArguments = ValidateArguments<IWordRegisterArgument>(arguments);

        var value0 = validatedArguments[0].GetWord();
        var value1 = validatedArguments[1].GetWord();

        var carry = value1 - value0 > 0b1111111111111111;
        var sign = ((value1 ^ value0) & 0b1000_0000_0000_0000) != 0;

        var value = (ushort)(value1 - value0);
        
        validatedArguments[1].SetWord(value);
        _state.SetFlag(Flag.Z, value == 0);
        _state.SetFlag(Flag.N, (value & 0b1000_0000_0000_0000) > 0);
        _state.SetFlag(Flag.V, sign && _state.GetFlag(Flag.N) != (value & 0b1000_0000_0000_0000) < 0);
    }

    public override ushort Opcode => Convert.ToUInt16("160000", 8);
}