using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor;

public class CommandParser
{
    private readonly ushort[] _masks =
    {
        //FEDC_BA98_7654_3210
        0b1111_1111_1111_1111, // halt, wait, reset, rtt, rti, iot, bpt
        0b1111_1111_1111_1000, // rts
        0b1111_1111_1110_0000, // flag instruction
        0b1111_1111_1100_0000, // one operand, mark
        0b1111_1111_0000_0000, // branch, trap, emt
        0b1111_1110_0000_0000, // jsr, sob, mul, div
        0b1111_0000_0000_0000, // two operand
    };

    private readonly Dictionary<ushort, ICommand> _opcodesDictionary;

    public CommandParser(IStorage storage, IState state)
    {
        _opcodesDictionary = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(ICommand).IsAssignableFrom(type) && !type.IsAbstract)
            .Select(commandType => Activator.CreateInstance(commandType, storage, state) as ICommand)
            .ToDictionary(command => command!.OperationCode);
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