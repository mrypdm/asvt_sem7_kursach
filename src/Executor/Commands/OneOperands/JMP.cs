using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public sealed class JMP : OneOperand
{
    public JMP(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => new IArgument[]
        { new RegisterWordArgument(Storage, State, GetMode(word), GetRegister(word)) };

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments); 
        State.Registers[7] = validatedArgument.Address ??
                             throw new InvalidOperationException("JMP cannot be addressing by register");
    }

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("000100", 8);
}