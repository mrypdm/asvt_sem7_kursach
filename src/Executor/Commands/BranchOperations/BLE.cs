using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

/// <summary>
/// Branch if Z || (N ^ V)
/// </summary>
public sealed class BLE : BranchOperation
{
    public BLE(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        if (State.Z || State.N ^ State.V)
        {
            UpdateProgramCounter(arguments);
        }
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("003400", 8);
}