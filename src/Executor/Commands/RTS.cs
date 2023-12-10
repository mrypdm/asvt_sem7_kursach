using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands;

public class RTS : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_1111_1000;
    private const ushort RegisterMask = 0b0000_0000_0000_0111;

    public RTS(IStorage storage, IState state) : base(storage, state)
    {
    }

    protected ushort GetRegister(ushort word) => (ushort)(word & RegisterMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[] { new RTSArg(Storage, State, GetRegister(word)) };
    }

    public override void Execute(IArgument[] arguments)
    {
    }

    public override ushort Opcode => Convert.ToUInt16("000200", 8);
}