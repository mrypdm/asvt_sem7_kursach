using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.MiscellaneousInstructions;

public class FlagCommand : BaseCommand
{
    public FlagCommand(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        if (arguments.Length != 1)
        {
            throw new ArgumentException("Count of arguments must be 1", nameof(arguments));
        }
        
        if (arguments[0] is not FlagArgument)
        {
            throw new InvalidArgumentTypeException(new[] { typeof(FlagArgument) }, new[] { arguments[0].GetType() });
        }

        var validatedArgument = (FlagArgument)arguments[0];

        if (validatedArgument.ToSet)
        {
            State.C = validatedArgument.C || State.C;
            State.V = validatedArgument.V || State.V;
            State.Z = validatedArgument.Z || State.Z;
            State.N = validatedArgument.N || State.N;
        }
        else
        {
            State.C = !validatedArgument.C && State.C;
            State.V = !validatedArgument.V && State.V;
            State.Z = !validatedArgument.Z && State.Z;
            State.N = !validatedArgument.N && State.N;
        }
    }

    public override IArgument[] GetArguments(ushort word) => new IArgument[] { new FlagArgument(word) };

    public override ushort Opcode => Convert.ToUInt16("000240", 8);
}