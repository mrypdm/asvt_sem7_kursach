using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class BIS : TwoOperands
{
    public BIS(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        arguments[1].SetValue((ushort)(arguments[1].GetValue() | arguments[0].GetValue()));
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("050000", 8);
}