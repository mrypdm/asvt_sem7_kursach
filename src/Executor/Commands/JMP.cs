using Executor.Arguments;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands;

public class JMP : OneOperand
{
    public JMP(IMemory memory, IState state) : base(memory, state)
    {
    }

    public override void Execute(IArgument[] arguments)
    {
        var value = arguments[0].GetValue();
        _state.Registers[7] = value;
    }

    public override ushort Opcode => (ushort)Convert.ToUInt16("000100", 8);;
}