namespace Executor.Arguments.Abstraction;

/// <summary>
/// Interface of argument
/// </summary>
public interface IArgument
{
    /// <summary>
    /// Get value
    /// </summary>
    object GetValue();

    /// <summary>
    /// Set value
    /// </summary>
    void SetValue(object obj);
}