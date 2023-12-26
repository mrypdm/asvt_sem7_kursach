using Executor.Arguments;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Storages;

namespace Executor.CommandTypes;

/// <summary>
/// Base class for commands with one argument
/// </summary>
public abstract class OneOperand : BaseCommand
{
    private const ushort SourceMask = 0b0000_0000_0011_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    /// <summary>
    /// Get argument addressing mode
    /// </summary>
    protected static ushort GetArgumentAddressingMode(ushort word) => (ushort)((word & SourceMask) >> 3);

    /// <summary>
    /// Get argument register
    /// </summary>
    protected static ushort GetArgumentRegister(ushort word) => (ushort)(word & RegisterMask);

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => new IArgument[]
    {
        (OperationCode & 0x8000) != 0
            ? new RegisterByteArgument(Storage, State, GetArgumentAddressingMode(word), GetArgumentRegister(word))
            : new RegisterWordArgument(Storage, State, GetArgumentAddressingMode(word), GetArgumentRegister(word))
    };

    /// <summary>
    /// Validate arguments
    /// </summary>
    /// <param name="arguments">Arguments</param>
    /// <typeparam name="TType">Expected type</typeparam>
    /// <returns>Converted arguments</returns>
    protected static TType ValidateArgument<TType>(IArgument[] arguments) where TType : class
    {
        ValidateArgumentsCount(arguments, 1);
        return ValidateArgument<TType>(arguments[0]);
    }

    protected OneOperand(IStorage storage, IState state) : base(storage, state)
    {
    }
}