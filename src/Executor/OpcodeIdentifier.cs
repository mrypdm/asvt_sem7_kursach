using Executor.Commands;
using Executor.CommandTypes;
using Executor.Memories;
using Executor.States;

namespace Executor;

public class OpcodeIdentifier
{
    private readonly ushort[] _masks =
    {
        0b1111_0000_0000_0000,
        0b1111_1111_1100_0000,
        0b1111_1111_0000_0000,
        0b1111_1111_1111_1000,
        0b1111_1111_1111_0000,
        0b1111_1110_0000_0000,
        0b1111_1111_1111_1111
    };

    private readonly Dictionary<ushort, ICommand> _opcodesDictionary;

    public OpcodeIdentifier(IState state, IMemory memory)
    {
        _opcodesDictionary = new ICommand[]
        {
            new MOV(memory, state),
            new MOVB(memory, state),
            new ADD(memory, state),
            new SOB(memory, state),
            new JSR(memory, state),
            new RTS(memory, state)
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

        throw new InvalidOperationException("Invalid Operation Opcode!");
    }
}