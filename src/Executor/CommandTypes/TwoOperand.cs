using System;
using System.Linq;
using Executor.Arguments;
using Executor.Memories;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;

namespace Executor.CommandTypes;

public abstract class TwoOperand : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_0000_0000_0000;
    private const ushort SourceMask1 = 0b0000_1110_0000_0000;
    private const ushort SourceMask2 = 0b0000_0000_0011_1000;
    private const ushort RegisterMask1 = 0b0000_0001_1100_0000;
    private const ushort RegisterMask2 = 0b0000_0000_0000_0111;

    protected ushort GetRegister1(ushort word) => (ushort)((word & RegisterMask1) >> 6);

    protected ushort GetRegister2(ushort word) => (ushort)(word & RegisterMask2);

    protected ushort GetMode2(ushort word) => (ushort)((word & SourceMask2) >> 3);

    protected ushort GetMode1(ushort word) => (ushort)((word & SourceMask1) >> 9);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    public override IArgument[] GetArguments(ushort word)
    {
        if ((Opcode & 0b1000_0000_0000_0000) > 0)
        {
            return new IArgument[]
            {
                new RegisterByteArgument(_memory, _state, GetMode1(word), GetRegister1(word)),
                new RegisterByteArgument(_memory, _state, GetMode2(word), GetRegister2(word))
            };
        }

        return new IArgument[]
        {
            new RegisterWordArgument(_memory, _state, GetMode1(word), GetRegister1(word)),
            new RegisterWordArgument(_memory, _state, GetMode2(word), GetRegister2(word))
        };
    }

    protected TType[] ValidateArguments<TType>(IArgument[] arguments) where TType : class
    {
        if (arguments.Length != 2)
        {
            throw new ArgumentException("Count of arguments must be 2", nameof(arguments));
        }
        
        if (arguments[0] is not TType || arguments[1] is not TType)
        {
            throw new InvalidArgumentTypeException(new[] { typeof(TType) },
                arguments.Select(m => m.GetType()));
        }

        return arguments.Select(m => (TType)m).ToArray();
    }

    protected TwoOperand(IMemory memory, IState state) : base(memory, state)
    {
    }
}