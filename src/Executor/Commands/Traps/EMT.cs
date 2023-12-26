using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.Traps;

/// <summary>
/// Emulator trap
/// </summary>
public sealed class EMT : TrapInstruction
{
    private const ushort InterruptVectorAddress = 24; // 0o30

    public EMT(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments) => HandleTrap(InterruptVectorAddress);

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("104000", 8);
}