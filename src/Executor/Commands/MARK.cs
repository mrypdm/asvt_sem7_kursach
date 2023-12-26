using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands;

/// <summary>
/// Mark stack command
/// </summary>
public sealed class MARK : BaseCommand
{
    private const ushort ArgumentMask = 0b0000_0000_0011_1111;

    private static ushort GetArgument(ushort word) => (ushort)(word & ArgumentMask);

    public MARK(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var argument = ValidateArgument<MarkArgument>(arguments[0]);

        State.Registers[6] += (ushort)(2 * (argument.Number + 1));
        State.Registers[7] = State.Registers[5];
        State.Registers[5] = Storage.PopFromStack(State);
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => new IArgument[] { new MarkArgument(GetArgument(word)) };

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("006400", 8);
}