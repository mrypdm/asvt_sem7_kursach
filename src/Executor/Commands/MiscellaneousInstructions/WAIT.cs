using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.MiscellaneousInstructions;

public class WAIT : BaseCommand
{
    public WAIT(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
    }

    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    public override ushort Opcode => Convert.ToUInt16("000001", 8);
}