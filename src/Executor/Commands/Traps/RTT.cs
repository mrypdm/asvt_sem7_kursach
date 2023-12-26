using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.Traps;

/// <summary>
/// Return from interrupt with prohibit trace trap
/// </summary>
public sealed class RTT : InterruptReturn
{
    public RTT(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override void Execute(IArgument[] arguments) => HandleReturn();

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    /// <inheritdoc />
    public override ushort OperationCode => Convert.ToUInt16("000006", 8);
}