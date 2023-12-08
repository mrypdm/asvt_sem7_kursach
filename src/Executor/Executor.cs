using Executor.States;
using Executor.Memories;

namespace Executor;

public class Executor
{
    private readonly IState _state;
    private readonly IMemory _memory;
    private readonly OpcodeIdentifier _opcodeIdentifier;

    public ushort PSW => _state.ProcessorStateWord;

    public IReadOnlyCollection<ushort> Registers => _state.Registers;

    public IReadOnlyMemory Memory => _memory;

    public ushort StartProgramAddress { get; private set; }

    public ushort StartStackAddress { get; private set; }

    public ushort LengthOfProgram { get; private set; }

    public string BinaryFile { get; private set; }

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

    public async Task Reload()
    {
        _state.Registers[6] = StartProgramAddress;
        _state.Registers[7] = StartStackAddress;

        using var reader = new StreamReader(BinaryFile);

        var address = StartProgramAddress;

        while (await reader.ReadLineAsync() is { } line)
        {
            var word = Convert.ToUInt16(line, 8);
            _memory.SetWord(address, word);
            address += 2;
        }

        LengthOfProgram = (ushort)(address - StartProgramAddress);
    }

    public Task LoadProgram(string filename, ushort initStackAddress, ushort initProgramAddress)
    {
        if (initProgramAddress % 2 == 1)
        {
            throw new InvalidOperationException("Start address cannot be odd number");
        }

        if (initStackAddress % 2 == 1)
        {
            throw new InvalidOperationException("Stack address cannot be odd number");
        }

        StartProgramAddress = initProgramAddress;
        StartStackAddress = initStackAddress;
        BinaryFile = filename;

        return Reload();
    }
}