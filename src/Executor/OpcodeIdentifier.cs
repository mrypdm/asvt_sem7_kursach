using System;
using System.Collections.Generic;
using System.Linq;
using Executor.Commands;
using Executor.Commands.BranchOperations;
using Executor.Commands.MiscellaneousInstructions;
using Executor.Commands.OneOperands;
using Executor.Commands.TwoOperands;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor;

public class OpcodeIdentifier
{
    private readonly ushort[] _masks =
    {
        0b1111_1111_1111_1111,
        0b1111_1111_1111_1000,
        0b1111_1111_1111_0000,
        0b1111_1111_1100_0000,
        0b1111_1111_0000_0000,
        0b1111_1110_0000_0000,
        0b1111_0000_0000_0000
    };

    private readonly Dictionary<ushort, ICommand> _opcodesDictionary;

    public OpcodeIdentifier(IState state, IStorage storage)
    {
        _opcodesDictionary = new ICommand[]
        {
            new MOV(storage, state),
            new MOVB(storage, state),
            new ADD(storage, state),
            new INC(storage, state),
            new INCB(storage, state),
            new BNE(storage, state),
            new BEQ(storage, state),
            new BR(storage, state),
            new BITB(storage, state),
            new BIT(storage, state),
            new DEC(storage, state),
            new DECB(storage, state),
            new CLR(storage, state),
            new CLRB(storage, state),
            new BMI(storage, state),
            new BLOS(storage, state),
            new BHI(storage, state),
            new BPL(storage, state),
            new BVC(storage, state),
            new BVS(storage, state),
            new HALT(storage, state),
            new BIS(storage, state),
            new BISB(storage, state),
            new BIC(storage, state),
            new BICB(storage, state),
            new SUB(storage, state),
            new SOB(storage, state),
            new JSR(storage, state),
            new JMP(storage, state),
            new RTS(storage, state)
        }.ToDictionary(command => command.Opcode, command => command);
    }

    public ICommand GetCommand(ushort word)
    {
        foreach (var mask in _masks)
        {
            var opcode = (ushort)(word & mask);

            if (_opcodesDictionary.TryGetValue(opcode, out var command))
            {
                return command;
            }
        }

        throw new ReservedInstructionException(word);
    }
}