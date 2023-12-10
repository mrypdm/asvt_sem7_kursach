using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.Traps;

public class EMT : TrapInstruction
{
    private const ushort InterruptVectorAddress = 24; // 0o30

    public EMT(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments) => HandleTrap(InterruptVectorAddress);

    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    public override ushort Opcode => Convert.ToUInt16("104000", 8);
}