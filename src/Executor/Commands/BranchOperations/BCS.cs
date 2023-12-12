using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

/// <summary>
/// BCS or BLO
/// </summary>
public sealed class BCS : BranchOperation
{
    public BCS(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        if (State.C)
        {
            UpdateProgramCounter(arguments);
        }
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("103400", 8);
}