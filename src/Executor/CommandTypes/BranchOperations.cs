using Executor.Arguments;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class BranchOperation : BaseCommand
{
    private const ushort OpcodeMask = 0b1111_1111_0000_0000;
    private const ushort OffsetMask = 0b0000_0000_1111_1111;

    private sbyte GetOffset(ushort word) => (sbyte)(word & OffsetMask);

    protected ushort GetOpcodeByMask(ushort word) => (ushort)(word & OpcodeMask);

    protected BranchOperation(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override IArgument[] GetArguments(ushort word)
    {
        return new IArgument[]
        {
            new OffsetArgument(Storage, State, GetOffset(word))
        };
    }

    protected void UpdateProgramCounter(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<IOffsetArgument>(arguments[0]);
        State.Registers[7] = (ushort)(State.Registers[7] + 2 * validatedArgument.Offset);
    }
}