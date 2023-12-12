using Executor.Arguments.Abstraction;
using Executor.Commands;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

/// <summary>
/// Argument for <see cref="MARK"/>
/// </summary>
public class MarkArgument : BaseArgument
{
    public MarkArgument(IStorage storage, IState state, ushort number) : base(storage, state)
    {
        Number = number;
    }

    /// <inheritdoc />
    public override object GetValue() => Number;

    /// <inheritdoc />
    public override void SetValue(object value) => throw new ReadOnlyArgumentException(GetType());
    
    /// <summary>
    /// Count of arguments in stack
    /// </summary>
    public ushort Number { get; }
}