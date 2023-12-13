using Executor.Arguments;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class OneOperand : BaseCommand
{
    private const ushort SourceMask = 0b0000_0000_0011_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    /// <summary>
    /// Get argument addressing mode
    /// </summary>
    protected static ushort GetMode(ushort word) => (ushort)((word & SourceMask) >> 3);

    /// <summary>
    /// Get argument register
    /// </summary>
    protected static ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    public override IArgument[] GetArguments(ushort word) => (OperationCode & 0x8000) != 0
        ? new IArgument[] { new RegisterByteArgument(Storage, State, GetMode(word), GetRegister(word)) }
        : new IArgument[] { new RegisterWordArgument(Storage, State, GetMode(word), GetRegister(word)) };

    protected static TType ValidateArgument<TType>(IArgument[] arguments) where TType : class
    {
        ValidateArgumentsCount(arguments, 1);
        return ValidateArgument<TType>(arguments[0]);
    }

    protected OneOperand(IStorage storage, IState state) : base(storage, state)
    {
    }
}