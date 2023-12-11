using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

public class BLE : BranchOperation
{
    public BLE(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        if (State.Z || State.N ^ State.V)
        {
            UpdateProgramCounter(arguments);
        }
    }

    public override ushort Opcode => Convert.ToUInt16("003400", 8);
}