using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.BranchOperations;

public class BGT : BranchOperation
{
    public BGT(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        if ((State.Z || State.N ^ State.V) == false)
        {
            UpdateProgramCounter(arguments);
        }
    }

    public override ushort Opcode => Convert.ToUInt16("003000", 8);
}