using Executor.Arguments;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Storages;

namespace Executor.CommandTypes;

/// <summary>
/// Commands with two register arguments
/// </summary>
public abstract class TwoOperand : BaseCommand
{
    private const ushort SourceMask1 = 0b0000_1110_0000_0000;
    private const ushort RegisterMask1 = 0b0000_0001_1100_0000;
    private const ushort SourceMask2 = 0b0000_0000_0011_1000;
    private const ushort RegisterMask2 = 0b0000_0000_0000_0111;

    /// <summary>
    /// Get first argument addressing mode
    /// </summary>
    protected static ushort GetLeftArgumentAddressingMode(ushort word) => (ushort)((word & SourceMask1) >> 9);

    /// <summary>
    /// Get first argument register
    /// </summary>
    protected static ushort GetLeftArgumentRegister(ushort word) => (ushort)((word & RegisterMask1) >> 6);

    /// <summary>
    /// Get second argument addressing mode
    /// </summary>
    protected static ushort GetRightArgumentAddressingMode(ushort word) => (ushort)((word & SourceMask2) >> 3);

    /// <summary>
    /// Get second argument register
    /// </summary>
    protected static ushort GetRightArgumentRegister(ushort word) => (ushort)(word & RegisterMask2);

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word)
    {
        if ((OperationCode & 0x8000) != 0)
        {
            return new IArgument[]
            {
                new RegisterByteArgument(Storage, State,
                    GetLeftArgumentAddressingMode(word),
                    GetLeftArgumentRegister(word)),
                new RegisterByteArgument(Storage, State,
                    GetRightArgumentAddressingMode(word),
                    GetRightArgumentRegister(word))
            };
        }

        return new IArgument[]
        {
            new RegisterWordArgument(Storage, State,
                GetLeftArgumentAddressingMode(word),
                GetLeftArgumentRegister(word)),
            new RegisterWordArgument(Storage, State,
                GetRightArgumentAddressingMode(word),
                GetRightArgumentRegister(word))
        };
    }

    protected TwoOperand(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <summary>
    /// Validate arguments
    /// </summary>
    /// <param name="arguments">Arguments</param>
    /// <typeparam name="TType">Expected type</typeparam>
    /// <returns>Converted arguments</returns>
    protected static (TType src, TType dst) ValidateArguments<TType>(IArgument[] arguments) where TType : class
    {
        ValidateArgumentsCount(arguments, 2);
        var arg0 = ValidateArgument<TType>(arguments[0]);
        var arg1 = ValidateArgument<TType>(arguments[1]);
        return (arg0, arg1);
    }
}