using Executor.Arguments;

namespace Executor.CommandTypes;

public interface ICommand
{
    void Execute(IArgument[] arguments);

    IArgument[] GetArguments(ushort word);

    ushort Opcode { get; }
}