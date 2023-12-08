namespace Executor.Arguments.Abstraction;

public interface IRegisterArgument<TValue> : IArgument
{
    ushort Register { get; }
    
    ushort Mode { get; }

    (Func<TValue> source, Action<TValue> destination) GetSourceAndDestination();
}