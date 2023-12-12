using System.Collections.Generic;
using System.Linq;
using Executor.Commands;
using Executor.Commands.BranchOperations;
using Executor.Commands.MiscellaneousInstructions;
using Executor.Commands.OneOperands;
using Executor.Commands.Traps;
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

    public OpcodeIdentifier(IStorage storage, IState state)
    {
        _opcodesDictionary = new ICommand[]
        {
            new CLR(storage, state), new CLRB(storage, state),
            new COM(storage, state), new COMB(storage, state),
            new INC(storage, state), new INCB(storage, state),
            new DEC(storage, state), new DECB(storage, state),
            new NEG(storage, state), new NEGB(storage, state),
            new ADC(storage, state), new ADCB(storage, state),
            new SBC(storage, state), new SBCB(storage, state),
            new TST(storage, state), new TSTB(storage, state),
            new ROR(storage, state), new RORB(storage, state),
            new ROL(storage, state), new ROLB(storage, state),
            new ASR(storage, state), new ASRB(storage, state),
            new ASL(storage, state), new ASLB(storage, state),
            new JMP(storage, state),
            new SWAB(storage, state),
            new MFPS(storage, state), new MTPS(storage, state),
            new SXT(storage, state),
            new XOR(storage, state),
            new BIT(storage, state), new BITB(storage, state),
            new BIC(storage, state), new BICB(storage, state),
            new BIS(storage, state), new BISB(storage, state),
            new ADD(storage, state),
            new SUB(storage, state),
            new MOV(storage, state), new MOVB(storage, state),
            new CMP(storage, state), new CMPB(storage, state),
            new BR(storage, state),
            new BNE(storage, state), new BEQ(storage, state),
            new BPL(storage, state), new BMI(storage, state),
            new BVC(storage, state), new BVS(storage, state),
            new BCC(storage, state), new BCS(storage, state),
            new BGE(storage, state), new BGT(storage, state),
            new BLE(storage, state), new BLT(storage, state),
            new BHI(storage, state),
            new BLOS(storage, state),
            new JSR(storage, state), new RTS(storage, state),
            new BPT(storage, state),
            new IOT(storage, state),
            new EMT(storage, state), new TRAP(storage, state),
            new MARK(storage, state),
            new SOB(storage, state),
            new RTI(storage, state), new RTT(storage, state),
            new HALT(storage, state), new WAIT(storage, state), new RESET(storage, state),
            new FlagCommand(storage, state)
        }.ToDictionary(command => command.OperationCode, command => command);
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