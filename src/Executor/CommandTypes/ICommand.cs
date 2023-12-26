using Executor.Arguments.Abstraction;

namespace Executor.CommandTypes;

/// <summary>
/// Interface of command
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes command with given arguments
    /// </summary>
    /// <param name="arguments">Arguments of command</param>
    void Execute(IArgument[] arguments);

    /// <summary>
    /// Parse arguments of command from command-word
    /// </summary>
    /// <param name="word">Code of command</param>
    /// <returns>Array of arguments</returns>
    IArgument[] GetArguments(ushort word);

    /// <summary>
    /// Operation code of command
    /// </summary>
    ushort OperationCode { get; }
}