using Executor.Arguments.Abstraction;
using Executor.Exceptions;

namespace Executor.Arguments;

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

    public object GetValue() => (ToSet, N, Z, V, C);

    public void SetValue(object obj) => throw new ReadOnlyArgumentException(GetType());

    public bool ToSet { get; }
    
    public bool C { get; }

    public bool V { get; }

    public bool Z { get; }

    public bool N { get; }
}