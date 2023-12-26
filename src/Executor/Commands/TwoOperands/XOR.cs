using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.TwoOperands;

public sealed class XOR : TwoOperand
{
    public XOR(IStorage storage, IState state) : base(storage, state)
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
            throw new ArgumentException("REG argument of XOR must be addressing with mode 0");
        }

        var value = (ushort)(reg.Value ^ dst.Value);

        dst.Value = value;

        State.Z = value == 0;
        State.N = value.IsNegative();
        State.V = false;
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("074000", 8);
}