using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.TwoOperands;

public class ADD : TwoOperand
{
    public ADD(IMemory memory, IState state) : base(memory, state)
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
        var validatedArguments = ValidateArguments<IRegisterArgument<ushort>>(arguments);
        var (source0, destination0) = validatedArguments[0].GetSourceAndDestination();
        var (source1, destination1) = validatedArguments[1].GetSourceAndDestination();
        
        var value0 = source0();
        var value1 = source1();

        var carry = value1 + value0 > 0b1111111111111111;
        var sign = ((value1 ^ value0) & 0b1000_0000_0000_0000) == 0;

        var value = (ushort)(value1 + value0);
        
        destination1(value);
        _state.Z = value == 0;
        _state.N = (value & 0b1000_0000_0000_0000) > 0;
        _state.V = sign && _state.N != (value & 0b1000_0000_0000_0000) < 0;
        _state.C = carry;
    }

    public override ushort Opcode => Convert.ToUInt16("060000", 8);
}