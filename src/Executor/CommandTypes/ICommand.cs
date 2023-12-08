using Executor.Arguments.Abstraction;

namespace Executor.CommandTypes;

public interface ICommand
{
    void Execute(IArgument[] arguments);

    IArgument[] GetArguments(ushort word);

    ushort Opcode { get; }
}