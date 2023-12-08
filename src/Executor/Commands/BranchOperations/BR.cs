using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

public class BR : BranchOperation
{
    public BR(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments) => UpdateProgramCounter(arguments);

    public override ushort Opcode => Convert.ToUInt16("000400", 8);
}