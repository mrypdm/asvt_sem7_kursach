using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Exceptions;
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
        new RegisterWordArgument(Storage, State, 0, GetRegister1(word)),
        new RegisterWordArgument(Storage, State, GetMode2(word), GetRegister2(word))
    };

    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 2);
        var reg = ValidateArgument<RegisterWordArgument>(arguments[0]);
        var dstArgument = ValidateArgument<RegisterWordArgument>(arguments[1]);

        if (reg.Mode != 0)
        {
            throw new ArgumentException("REG argument of JSR must be addressing with mode 0");
        }

        var temp = dstArgument.Address ?? // because dst can refer to stack, which we change
                   throw new InvalidInstructionException("JSR destination cannot be addressing by register");
        Storage.PushToStack(State, reg.Value);
        reg.Value = State.Registers[7];
        State.Registers[7] = temp;
    }

    public override ushort Opcode => Convert.ToUInt16("004000", 8);
}