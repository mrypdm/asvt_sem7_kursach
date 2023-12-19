using Executor.Arguments.Abstraction;
using Executor.Exceptions;

namespace Executor.Arguments;

/// <inheritdoc cref="IOffsetArgument"/>
public class OffsetArgument : IOffsetArgument
{
    /// <inheritdoc />
    public object GetValue() => Offset;

    /// <inheritdoc />
    public void SetValue(object obj) => throw new ReadOnlyArgumentException(typeof(OffsetArgument));

    /// <inheritdoc />
    public sbyte Offset { get; }

    public OffsetArgument(sbyte offset)
    {
        Offset = offset;
    }
}