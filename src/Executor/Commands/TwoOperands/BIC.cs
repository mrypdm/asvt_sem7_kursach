using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

internal class BIC : TwoOperands
{
    public BIC(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        arguments[1].SetValue((ushort)(arguments[1].GetValue() & (ushort)(~arguments[0].GetValue())));
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("040000", 8);
}