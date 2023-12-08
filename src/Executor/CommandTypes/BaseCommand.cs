using Executor.Arguments.Abstraction;
using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class BaseCommand : ICommand
{
    protected IStorage Storage { get; }
    
    protected IState _state { get; }
    
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode { get; }

    protected BaseCommand(IStorage storage, IState state)
    {
        Storage = storage;
        _state = state;
    }
}