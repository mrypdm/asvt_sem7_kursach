using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class JMP : OneOperand
{
    public JMP(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override IArgument[] GetArguments(ushort word) => new IArgument[]
        { new RegisterWordArgument(Storage, State, GetMode(word), GetRegister(word)) };

    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<RegisterWordArgument>(arguments[0]);
        State.Registers[7] = validatedArgument.Address ??
                             throw new InvalidOperationException("JMP cannot be addressing by register");
    }

    public override ushort Opcode => Convert.ToUInt16("000100", 8);
}