using Executor.Arguments;
using Executor.Memories;
using Executor.States;

namespace Executor.CommandTypes;

public abstract class BaseCommand : ICommand
{
    protected IMemory Memory { get; }
    
    protected IState State { get; }
    
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode { get; }

    protected BaseCommand(IMemory memory, IState state)
    {
        Memory = memory;
        State = state;
    }
}