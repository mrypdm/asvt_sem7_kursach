using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Devices.Managers;
using Devices.Providers;
using Devices.Validators;
using DeviceSdk;
using Domain.Models;
using Executor.States;
using Executor.Storages;

namespace Executor;

public class Executor
{
    private bool _initialized;

    private readonly IState _state;
    private readonly IStorage _memory;
    private readonly IDeviceValidator _deviceValidator;
    private readonly IDevicesManager _devicesManager;
    private readonly IStorage _bus;

    private readonly OpcodeIdentifier _opcodeIdentifier;

    public ushort ProcessorStateWord => _state.ProcessorStateWord;

    public IReadOnlyCollection<ushort> Registers => _state.Registers;

    public IReadOnlyStorage Memory => _memory;

    public IReadOnlyCollection<IDevice> Devices => _devicesManager.Devices;

    public IProject Project { get; private set; }

    public Executor()
    {
        _state = new State();
        _memory = new Memory();
        var provider = new DeviceProvider();
        _devicesManager = new DevicesManager(provider);
        _deviceValidator = new DeviceValidator(provider);
        _bus = new Bus(_memory, _devicesManager);
        _opcodeIdentifier = new OpcodeIdentifier(_state, _bus);
    }

    public void Init()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        foreach (var device in _devicesManager.Devices)
        {
            device.Init();
        }
    }

    public int ExecuteProgram()
    {
        return 0;
    }

    public int ExecuteNextInstruction()
    {
        Init();
        var word = _memory.GetWord(_state.Registers[7]);
        _state.Registers[7] += 2;
        var command = _opcodeIdentifier.GetCommand(word);
        command.Execute(command.GetArguments(word));
        return 0;
    }


    public Task LoadProgram(IProject project)
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

        return Reload();
    }

    public async Task Reload()
    {
        _initialized = false;
        _devicesManager.Clear();
        Array.Fill<ushort>(_state.Registers, 0);

        _state.Registers[6] = Project.StackAddress;
        _state.Registers[7] = Project.ProgramAddress;

        using var reader = new StreamReader(Project.ProjectBinary);

        // file format:
        // 000000 - code section
        // ...
        // 000001' - relocatable address
        // ...
        // 000000
        // #/path/to/device.dll - devices section

        var address = Project.ProgramAddress;
        while (await reader.ReadLineAsync() is { } line)
        {
            if (line.StartsWith("#"))
            {
                AddDevice(line);
                continue;
            }

            var isRelocatable = line.EndsWith('\'');

            var word = isRelocatable
                ? Convert.ToUInt16(line[..6], 8) + Project.ProgramAddress
                : Convert.ToUInt16(line, 8);

            if (word > ushort.MaxValue)
            {
                throw new OutOfMemoryException("Program is too large");
            }

            _memory.SetWord(address, (ushort)word);
            address += 2;
        }
    }

    private void AddDevice(string path)
    {
        _deviceValidator.ThrowIfInvalid(path);
        _devicesManager.Add(path);
    }
}