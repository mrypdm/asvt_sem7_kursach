using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.States;

namespace Executor.Arguments;

/// <summary>
/// Flag Control Operations Argument
/// </summary>
public class FlagArgument : IArgument
{
    public FlagArgument(ushort word)
    {
        C = (word & 1) != 0;
        V = (word & 2) != 0;
        Z = (word & 4) != 0;
        N = (word & 8) != 0;
        ToSet = (word & 16) != 0;
    }

    /// <inheritdoc />
    public object GetValue() => (ToSet, N, Z, V, C);

    /// <inheritdoc />
    public void SetValue(object obj) => throw new ReadOnlyArgumentException(GetType());

    /// <summary>
    /// Set or clear flags
    /// </summary>
    public bool ToSet { get; }

    /// <summary>
    /// If need to change <see cref="IState.C"/>
    /// </summary>
    public bool C { get; }

    /// <summary>
    /// If need to change <see cref="IState.V"/>
    /// </summary>
    public bool V { get; }

    /// <summary>
    /// If need to change <see cref="IState.Z"/>
    /// </summary>
    public bool Z { get; }

    /// <summary>
    /// If need to change <see cref="IState.N"/>
    /// </summary>
    public bool N { get; }
}