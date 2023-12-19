using Executor.Arguments.Abstraction;
using Executor.Commands;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

/// <summary>
/// Argument for <see cref="MARK"/>
/// </summary>
public class MarkArgument : IArgument
{
    public MarkArgument(ushort number)
    {
        Number = number;
    }

    /// <inheritdoc />
    public object GetValue() => Number;

    /// <inheritdoc />
    public void SetValue(object value) => throw new ReadOnlyArgumentException(GetType());
    
    /// <summary>
    /// Count of arguments in stack
    /// </summary>
    public ushort Number { get; }
}