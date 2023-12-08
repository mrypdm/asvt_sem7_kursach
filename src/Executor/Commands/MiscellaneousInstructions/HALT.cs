using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.MiscellaneousInstructions;

public class HALT : BaseCommand
{
    public HALT(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    public override void Execute(IArgument[] arguments)
    {
        throw new HaltException(true);
    }

    public override ushort Opcode => Convert.ToUInt16("000000", 8);
}