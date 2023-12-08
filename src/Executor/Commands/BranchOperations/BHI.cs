using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

public class BHI : BranchOperation
{
    public BHI(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        if (!_state.C && !_state.Z)
        {
            UpdateProgramCounter(arguments);
        }
    }

    public override ushort Opcode => Convert.ToUInt16("101000", 8);
}