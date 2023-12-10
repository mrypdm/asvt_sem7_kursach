namespace Executor.Arguments.Abstraction;

/// <summary>
/// Argument
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