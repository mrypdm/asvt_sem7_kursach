using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

/// <summary>
/// Branch without condition
/// </summary>
public sealed class BR : BranchOperation
{
    public BR(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments) => UpdateProgramCounter(arguments);

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("000400", 8);
}