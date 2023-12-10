using System;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.Traps;

public class TRAP : TrapInstruction
{
    private const ushort InterruptVectorAddress = 28; // 0o34

    public TRAP(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override void Execute(IArgument[] arguments) => HandleTrap(InterruptVectorAddress);

    public override IArgument[] GetArguments(ushort word) => Array.Empty<IArgument>();

    public override ushort Opcode => Convert.ToUInt16("104400", 8);
}