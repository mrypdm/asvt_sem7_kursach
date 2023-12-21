using Executor.Arguments;
using Executor.States;
using Executor.Arguments.Abstraction;
using Executor.Storages;

namespace Executor.CommandTypes;

public abstract class BranchOperation : BaseCommand
{
    private const ushort OffsetMask = 0b0000_0000_1111_1111;

    private static sbyte GetOffset(ushort word) => (sbyte)(word & OffsetMask);

    protected BranchOperation(IStorage storage, IState state) : base(storage, state)
    {
    }

    public override IArgument[] GetArguments(ushort word) => new IArgument[] { new OffsetArgument(GetOffset(word)) };

    protected void UpdateProgramCounter(IArgument[] arguments)
    {
        ValidateArgumentsCount(arguments, 1);
        var validatedArgument = ValidateArgument<IOffsetArgument>(arguments[0]);
        State.Registers[7] = (ushort)(State.Registers[7] + 2 * validatedArgument.Offset);
    }
}