using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public class JSR : TwoOperand
{
    public JSR(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override IArgument[] GetArguments(ushort word) => new IArgument[]
    {
        new RegisterArgument(Storage, State, GetRegister1(word)),
        new RegisterAddressArgument(Storage, State, GetMode2(word), GetRegister2(word))
    };

    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 2);
        var reg = ValidateArgument<RegisterWordArgument>(arguments[0]);
        var dstArgument = ValidateArgument<RegisterAddressArgument>(arguments[1]);

        if (reg.Mode != 0)
        {
            throw new ArgumentException("REG argument of JSR must be addressing with mode 0");
        }

        var (getRegValue, setRegValue) = reg.GetSourceAndDestination();
        var dst = dstArgument.GetSource();

        var temp = dst(); // because dst can refer to stack, which we change
        Storage.PushToStack(State, getRegValue());
        setRegValue(State.Registers[7]);
        State.Registers[7] = temp;
    }

    public override ushort Opcode => Convert.ToUInt16("004000", 8);
}