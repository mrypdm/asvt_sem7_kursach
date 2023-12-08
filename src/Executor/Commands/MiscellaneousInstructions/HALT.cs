using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.MiscellaneousInstructions;

public class HALT : BaseCommand
{
    public HALT(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    public override void Execute(IArgument[] arguments)
    {
        throw new HaltException(true);
    }

    public override ushort Opcode => Convert.ToUInt16("000000", 8);
}