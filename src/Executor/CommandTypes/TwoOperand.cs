using System;
using System.Linq;
using Executor.Arguments;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class TwoOperand : BaseCommand
{
    private const ushort SourceMask1 = 0b0000_1110_0000_0000;
    private const ushort RegisterMask1 = 0b0000_0001_1100_0000;
    private const ushort SourceMask2 = 0b0000_0000_0011_1000;
    private const ushort RegisterMask2 = 0b0000_0000_0000_0111;

    protected ushort GetMode1(ushort word) => (ushort)((word & SourceMask1) >> 9);

    protected ushort GetRegister1(ushort word) => (ushort)((word & RegisterMask1) >> 6);

    protected ushort GetMode2(ushort word) => (ushort)((word & SourceMask2) >> 3);

    protected ushort GetRegister2(ushort word) => (ushort)(word & RegisterMask2);

    public override IArgument[] GetArguments(ushort word)
    {
        if ((Opcode & 0b1000_0000_0000_0000) > 0)
        {
            return new IArgument[]
            {
                new RegisterByteArgument(Storage, State, GetMode1(word), GetRegister1(word)),
                new RegisterByteArgument(Storage, State, GetMode2(word), GetRegister2(word))
            };
        }

        return new IArgument[]
        {
            new RegisterWordArgument(Storage, State, GetMode1(word), GetRegister1(word)),
            new RegisterWordArgument(Storage, State, GetMode2(word), GetRegister2(word))
        };
    }

    protected TType[] ValidateArguments<TType>(IArgument[] arguments) where TType : class
    {
        ValidateArgumentsCount(arguments, 2);
        var arg0 = ValidateArgument<TType>(arguments[0]);
        var arg1 = ValidateArgument<TType>(arguments[1]);
        return new[] { arg0, arg1 };
    }

    protected TwoOperand(IStorage storage, IState state) : base(storage, state)
    {
    }
}