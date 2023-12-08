using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class JMP : OneOperand
{
    public JMP(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new RegisterWordArgument(_memory, _state, GetMode(word), GetRegister(word))
        };
    }

    public override void Execute(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IRegisterArgument<ushort>>(arguments);
        var (source, destination) = validatedArgument.GetSourceAndDestination();

        if (validatedArgument.Mode == 0)
        {
            throw new InvalidInstructionException("JMP cannot be addressed with mode 0");
        }
        
        _state.Registers[7] = source();
    }

    public override ushort Opcode => Convert.ToUInt16("000100", 8);
}