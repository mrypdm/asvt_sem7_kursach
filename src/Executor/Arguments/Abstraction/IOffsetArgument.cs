namespace Executor.Arguments.Abstraction;

/// <summary>
/// Offset argument
/// </summary>
public interface IOffsetArgument : IArgument
{
    /// <summary>
    /// Offset value
    /// </summary>
    sbyte Offset { get; }
}