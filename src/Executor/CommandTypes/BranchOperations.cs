using System;
using Executor.Arguments;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class BranchOperation : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_0000_0000;
    private const ushort OffsetMask = 0b0000_0000_1111_1111;

    private sbyte GetOffset(ushort word) => (sbyte)(word & OffsetMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    protected BranchOperation(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new OffsetArgument(Storage, State, GetOffset(word))
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

    protected void UpdateProgramCounter(IArgument[] arguments)
    {
        var validatedArgument = ValidateArgument<IOffsetArgument>(arguments);
        State.Registers[7] = (ushort)(State.Registers[7] + 2 * validatedArgument.GetOffset());
    }
}