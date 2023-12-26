using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.MiscellaneousInstructions;

/// <summary>
/// Stop execution
/// </summary>
public sealed class HALT : BaseCommand
{
    public HALT(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments) => throw new HaltException(true);

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("000000", 8);
}