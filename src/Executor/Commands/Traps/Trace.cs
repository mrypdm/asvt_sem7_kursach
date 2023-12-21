using System;
using Executor.Arguments.Abstraction;
using Executor.Attributes;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.Traps;

/// <summary>
/// Class of Trace Trap
/// </summary>
[NotCommand]
public sealed class Trace : TrapInstruction
{
    public Trace(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments) => throw new NotSupportedException();

    public override IArgument[] GetArguments(ushort word) => throw new NotSupportedException();

    public override ushort OperationCode => 0;
}