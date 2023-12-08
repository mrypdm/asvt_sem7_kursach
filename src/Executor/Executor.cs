using Executor.States;
using Executor.Memories;
using Executor.CommandTypes;

namespace Executor;

public class Executor
{
    private readonly IState _state;
    private readonly IMemory _memory;
    private readonly OpcodeIdentifier _opcodeIdentifier;

    public Executor()
    {
        _state = new State();
        _memory = new Memory();
        _opcodeIdentifier = new OpcodeIdentifier(_state, _memory);
    }

    public int ExecuteProgram()
    {
        return 0;
    }

    public int ExecuteNextInstruction()
    {
        var word = _memory.GetWord(_state.Registers[7]);
        _state.Registers[7] += 2;
        var command = _opcodeIdentifier.GetCommand(word);
        command.Execute(command.GetArguments(word));
        return 0;
    }

    public async Task LoadProgram(string filename, ushort initStackAddress, ushort initProgramAddress)
    {
        _state.Registers[6] = initStackAddress;
        _state.Registers[7] = initProgramAddress;

        using var reader = new StreamReader(filename);

        var address = initProgramAddress;
        while (await reader.ReadLineAsync() is { } line)
        {
            var word = Convert.ToUInt16(line, 8);
            _memory.SetWord(address, word);
            address += 2;
        }
    }
}