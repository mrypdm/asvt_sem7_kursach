using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.Traps;

public sealed class RTT : TrapReturn
{
    public RTT(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments) => HandleReturn();

    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    public override ushort OperationCode => Convert.ToUInt16("000006", 8);
}