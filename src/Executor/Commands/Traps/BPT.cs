using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.Traps;

/// <summary>
/// Breakpoint interrupt
/// </summary>
public sealed class BPT : TrapInstruction
{
    private const ushort InterruptVectorAddress = 12; // 0o14

    /// <inheritdoc />
    public BPT(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments) => HandleTrap(InterruptVectorAddress);

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("000003", 8);
}