namespace Executor.Arguments.Abstraction;

public interface IOffsetArgument : IArgument
{
    sbyte Offset { get; }
}