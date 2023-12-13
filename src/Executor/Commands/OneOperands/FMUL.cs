using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.CommandTypes;
using Executor.States;
using Executor.Storages;

namespace Executor.Commands.OneOperands;

public class FMUL : OneOperand
{
    public FMUL(IStorage storage, IState state) : base(storage, state)
    {
    }

    /// <inheritdoc />
    public override IArgument[] GetArguments(ushort word) => new IArgument[]
        { new RegisterWordArgument(Storage, State, 0, GetRegister(word)) };

    /// <inhertidoc />
    public override void Execute(IArgument[] arguments)
    {
        var reg = ValidateArgument<RegisterWordArgument>(arguments);

        if (reg.Mode != 0)
        {
            throw new ArgumentException("Argument of FADD must be addressing with mode 0");
        }

        var rightHigh = Storage.GetWord(State.Registers[reg.Register]);
        var rightLow = Storage.GetWord((ushort)(State.Registers[reg.Register] + 2));

        var leftHigh = Storage.GetWord((ushort)(State.Registers[reg.Register] + 4));
        var leftLow = Storage.GetWord((ushort)(State.Registers[reg.Register] + 6));

        var rightOp = BitConverter.Int32BitsToSingle((rightHigh << 16) | rightLow);
        var leftOp = BitConverter.Int32BitsToSingle((leftHigh << 16) | leftLow);

        var result = leftOp * rightOp;
        var value = BitConverter.SingleToUInt32Bits(result);

        Storage.SetWord((ushort)(State.Registers[reg.Register] + 4), (ushort)((value & 0xFFFF0000) >> 8));
        Storage.SetWord((ushort)(State.Registers[reg.Register] + 6), (ushort)(value & 0xFFFF));

        State.C = false;
        State.V = false;
        State.N = result == 0;
        State.Z = result < 0;
    }

    /// <inhertidoc />
    public override ushort OperationCode => Convert.ToUInt16("075020", 8);
}