using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Domain.Models;
using Executor.States;
using Executor.Memories;

namespace Executor;

public class Executor
{
    private readonly IState _state;
    private readonly IMemory _memory;
    private readonly OpcodeIdentifier _opcodeIdentifier;

    public IReadOnlyMemory Memory => _memory;

    public ushort ProcessorStateWord => _state.ProcessorStateWord;

    public IReadOnlyCollection<ushort> Registers => _state.Registers;
    
    public IProject Project { get; private set; }
    
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
        _state.Registers[6] = Project.StackAddress;
        _state.Registers[7] = Project.ProgramAddress;

        using var reader = new StreamReader(BinaryFile);

        var address = Project.ProgramAddress;
        while (await reader.ReadLineAsync() is { } line)
        {
            var word = Convert.ToUInt16(line, 8);
            _memory.SetWord(address, word);
            address += 2;
        }
    }

    public Task LoadProgram(IProject project, string filename)
    {
        if (project.ProgramAddress % 2 == 1)
        {
            throw new InvalidOperationException("Start program address cannot be odd");
        }
        
        
        if (project.StackAddress % 2 == 1)
        {
            throw new InvalidOperationException("Start stack address cannot be odd");
        }

        Project = project;
        BinaryFile = filename;

        return Reload();
    }
}