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

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new RegisterWordArgument(Storage, State, GetMode(word), GetRegister(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<IRegisterArgument<ushort>>(arguments[0]);
        var (source, destination) = validatedArgument.GetSourceAndDestination();

        if (validatedArgument.Mode == 0)
        {
            throw new InvalidInstructionException("JMP cannot be addressed with mode 0");
        }
        
        State.Registers[7] = source();
    }

    public override ushort Opcode => Convert.ToUInt16("000100", 8);
}