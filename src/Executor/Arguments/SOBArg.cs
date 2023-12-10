using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

public class SOBArg : BaseArgument
{
    public SOBArg(IStorage storage, IState state, ushort register, byte offset) : base(storage, state)
    {
        Register = register;
        Offset = offset;
    }

    public override object GetValue() => (Register, Offset);

    public override void SetValue(object word) => throw new ReadOnlyArgumentException(typeof(SOBArg));

    public ushort Register { get; }

    public byte Offset { get; }
}