using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

public class BLT : BranchOperation
{
    public BLT(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        if (State.N != State.V)
        {
            UpdateProgramCounter(arguments);
        }
    }

    public override ushort Opcode => Convert.ToUInt16("002400", 8);
}