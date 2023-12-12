using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

public sealed class BGT : BranchOperation
{
    public BGT(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        if ((State.Z || State.N ^ State.V) == false)
        {
            UpdateProgramCounter(arguments);
        }
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("003000", 8);
}