using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

public sealed class BVC : BranchOperation
{
    public BVC(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        if (!State.V)
        {
            UpdateProgramCounter(arguments);
        }
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("102000", 8);
}