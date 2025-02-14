using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

/// <summary>
/// Go ot subroutine
/// </summary>
public sealed class JSR : TwoOperand
{
    public JSR(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => new IArgument[]
    {
        new RegisterWordArgument(Storage, State, 0, GetLeftArgumentRegister(word)),
        new RegisterWordArgument(Storage, State, GetRightArgumentAddressingMode(word), GetRightArgumentRegister(word))
    };

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 2);
        var reg = ValidateArgument<RegisterWordArgument>(arguments[0]);
        var dst = ValidateArgument<RegisterWordArgument>(arguments[1]);

        if (reg.Mode != 0)
        {
            throw new ArgumentException("REG argument of JSR must be addressing with mode 0");
        }

        var temp = dst.Address ?? // because dst can refer to stack, which we change
                   throw new InvalidInstructionException("JSR destination cannot be addressing by register");
        Storage.PushToStack(State, reg.Value);
        reg.Value = State.Registers[7];
        State.Registers[7] = temp;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("004000", 8);
}