using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.Traps;

/// <summary>
/// IO trap
/// </summary>
public sealed class IOT : TrapInstruction
{
    private const ushort InterruptVectorAddress = 16; // 0o20

    public IOT(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments) => HandleTrap(InterruptVectorAddress);

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("000004", 8);
}