using System;
using Executor.Arguments;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class OneOperand : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1100_0000;
    private const ushort SourceMask = 0b0000_0000_0011_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    protected ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    protected ushort GetMode(ushort word) => (ushort)((word & SourceMask) >> 3);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    public override IArgument[] GetArguments(ushort word)
    {
        if ((Opcode & 0b1000_0000_0000_0000) > 0)
        {
            return new IArgument[]
            {
                new RegisterByteArgument(Storage, _state, GetMode(word), GetRegister(word))
            };
        }

        return new IArgument[]
        {
            new RegisterWordArgument(Storage, _state, GetMode(word), GetRegister(word))
        };
    }

    protected TType ValidateArgument<TType>(IArgument[] arguments) where TType : class
    {
        if (arguments.Length != 1)
        {
            throw new ArgumentException("Count of arguments must be 1", nameof(arguments));
        }
        
        if (arguments[0] is not TType)
        {
            throw new InvalidArgumentTypeException(new[] { typeof(TType) }, new[] { arguments[0].GetType() });
        }

        return (TType)arguments[0];
    }

    protected OneOperand(IStorage storage, IState state) : base(storage, state)
    {
    }
}