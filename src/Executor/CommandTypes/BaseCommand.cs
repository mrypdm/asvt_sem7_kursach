using Executor.Arguments.Abstraction;
using Executor.Memories;
using Executor.States;

namespace Executor.CommandTypes;

public abstract class BaseCommand : ICommand
{
    protected IMemory _memory { get; }
    
    protected IState _state { get; }
    
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode { get; }

    protected BaseCommand(IMemory memory, IState state)
    {
        _memory = memory;
        _state = state;
    }
}