using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor.Commands.TwoOperands;

public class BISB : TwoOperand
{
	public BISB(IMemory memory, IState state) : base(memory, state)
	{
	}

	public override void Execute(IArgument[] arguments)
	{
		var validatedArguments = ValidateArguments<IByteRegisterArgument>(arguments);
		validatedArguments[1].SetValue((byte)(validatedArguments[0].GetByte() | validatedArguments[1].GetByte()));
	}

	public override ushort Opcode => Convert.ToUInt16("150000", 8);
}