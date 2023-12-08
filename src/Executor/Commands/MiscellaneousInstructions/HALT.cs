using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class HALT : BaseCommand
{
    public HALT(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[] {};
    }

    public override void Execute(IArgument[] arguments)
    {
        _state.Stop = true;
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("000000", 8);
}