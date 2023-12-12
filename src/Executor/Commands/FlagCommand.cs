using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands;

public class FlagCommand : BaseCommand
{
    public FlagCommand(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<FlagArgument>(arguments[0]);

        if (validatedArgument.ToSet) // SCC
        {
            State.C = validatedArgument.C || State.C; // SEC
            State.V = validatedArgument.V || State.V; // SEV
            State.Z = validatedArgument.Z || State.Z; // SEZ
            State.N = validatedArgument.N || State.N; // SEN
        }
        else // CCC, NOP if all is false
        {
            State.C = !validatedArgument.C && State.C; // CLC
            State.V = !validatedArgument.V && State.V; // CLV
            State.Z = !validatedArgument.Z && State.Z; // CLZ
            State.N = !validatedArgument.N && State.N; // CLN
        }
    }

    public override IArgument[] GetArguments(ushort word) => new IArgument[] { new FlagArgument(word) };

    public override ushort Opcode => Convert.ToUInt16("000240", 8);
}