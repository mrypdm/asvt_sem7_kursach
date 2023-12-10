using System;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class BaseCommand : ICommand
{
    protected IStorage Storage { get; }
    
    protected IState State { get; }
    
    public abstract void Execute(IArgument[] arguments);

    public abstract IArgument[] GetArguments(ushort word);

    public abstract ushort Opcode { get; }

    protected TType ValidateArgument<TType>(IArgument arguments) where TType : class
    {
        if (arguments is not TType type)
        {
            throw new InvalidArgumentTypeException(typeof(TType), arguments.GetType());
        }

        return type;
    }

    protected void ValidateArgumentsCount(IArgument[] arguments, int count)
    {
        if (arguments.Length != count)
        {
            throw new ArgumentException($"Count of arguments must be {count}", nameof(arguments));
        }
    }

    protected BaseCommand(IStorage storage, IState state)
    {
        Storage = storage;
        State = state;
    }
}