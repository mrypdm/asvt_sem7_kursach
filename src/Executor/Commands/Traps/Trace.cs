using System;
using Executor.Arguments.Abstraction;
using Executor.Attributes;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.Traps;

/// <summary>
/// Trace trap
/// </summary>
[NotCommand]
public sealed class Trace : TrapInstruction
{
    public const ushort InterruptVectorAddress = 12; // 0o14
    
    public Trace(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments) => throw new NotSupportedException();

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => throw new NotSupportedException();

    /// <inheritdoc />
    public override ushort OperationCode => 0;
}