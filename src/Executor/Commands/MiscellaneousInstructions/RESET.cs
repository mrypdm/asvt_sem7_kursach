using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.MiscellaneousInstructions;

public sealed class RESET : BaseCommand
{
    public RESET(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        Storage.Init();
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("000005", 8);
}