using System;
using Executor.Arguments.Abstraction;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor.CommandTypes;

/// <summary>
/// Base class for commands
/// </summary>
public abstract class BaseCommand : ICommand
{
    /// <summary>
    /// Storage
    /// </summary>
    protected IStorage Storage { get; }

    /// <summary>
    /// State
    /// </summary>
    protected IState State { get; }

    /// <inheritdoc />
    public abstract void Execute(IArgument[] arguments);

    /// <inheritdoc />
    public abstract IArgument[] GetArguments(ushort word);

    /// <inheritdoc />
    public abstract ushort OperationCode { get; }

    protected BaseCommand(IStorage storage, IState state)
    {
        Storage = storage;
        State = state;
    }

    /// <summary>
    /// Validate argument
    /// </summary>
    /// <param name="argument">Abstract argument</param>
    /// <typeparam name="TType">Expected type of argument</typeparam>
    /// <returns>Converted argument</returns>
    /// <exception cref="InvalidArgumentTypeException">If type is not <typeparamref name="TType"/></exception>
    protected static TType ValidateArgument<TType>(IArgument argument) where TType : class
    {
        if (argument is not TType type)
        {
            throw new InvalidArgumentTypeException(typeof(TType), argument.GetType());
        }

        return type;
    }

    /// <summary>
    /// Validate arguments count
    /// </summary>
    /// <param name="arguments">Arguments</param>
    /// <param name="count">Expected count</param>
    /// <exception cref="ArgumentException">
    ///     If <paramref name="arguments"/>.<see cref="Array.Length"/> is not equal to <paramref name="count"/>
    /// </exception>
    protected static void ValidateArgumentsCount(IArgument[] arguments, int count)
    {
        if (arguments.Length != count)
        {
            throw new ArgumentException($"Count of arguments must be {count}", nameof(arguments));
        }
    }
}