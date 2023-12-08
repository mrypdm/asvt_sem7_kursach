using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.OneOperands;

public class DEC : OneOperand
{
	public DEC(IMemory memory, IState state) : base(memory, state)
	{
	}

	public override void Execute(IArgument[] arguments)
	{
		var validatedArgument = ValidateArgument<IWordRegisterArgument>(arguments[0]);
		var value = validatedArgument.GetWord() - 1;
		validatedArgument.SetWord((ushort)value);
	}

	public override ushort Opcode => Convert.ToUInt16("005300", 8);
}