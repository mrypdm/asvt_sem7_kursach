using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Devices.Managers;
using Devices.Providers;
using Devices.Validators;
using DeviceSdk;
using Domain.Models;
using Executor.Commands.MiscellaneousInstructions;
using Executor.Commands.Traps;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.States;
using Executor.Storages;

namespace Executor;

public class Executor
{
    private bool _initialized;
    private ICommand _lastCommand;

    private readonly Stack<string> _trapStack = new();

    private readonly HashSet<string> _trapsToHalt = new()
    {
        nameof(BusException),
        nameof(OddAddressException),
        nameof(EMT),
        nameof(TRAP),
        nameof(IOT),
        nameof(BPT),
        "Trace"
    };

    private readonly IState _state;
    private readonly IStorage _memory;
    private readonly IDeviceValidator _deviceValidator;
    private readonly IDevicesManager _devicesManager;
    private readonly Bus _bus;

    private readonly OpcodeIdentifier _opcodeIdentifier;
    private readonly Dictionary<ushort, string> _symbols = new();
    private readonly HashSet<ushort> _breakpoints = new();

    public ushort ProcessorStateWord => _state.ProcessorStateWord;

    public IReadOnlyCollection<ushort> Registers => _state.Registers;

    public IReadOnlyStorage Memory => _memory;

    public IReadOnlyCollection<IDevice> Devices => _devicesManager.Devices;

    public IReadOnlyDictionary<ushort, string> Symbols => _symbols;

    public IReadOnlySet<ushort> Breakpoints => _breakpoints;

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

        _bus.Init();
    }

    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        Init();

        var res = true;

        while (!cancellationToken.IsCancellationRequested && res)
        {
            if (_breakpoints.Contains(_state.Registers[7]))
            {
                break;
            }

            res = await ExecuteNextInstructionAsync();
        }

        return res;
    }

    public bool ExecuteNextInstruction()
    {
        Init();

        if (_state.T && _lastCommand is not RTT and not TrapInstruction and not WAIT)
        {
            HandleInterrupt("Trace", 12); // 0o14
        }

        var interruptedDevice = _bus.GetInterrupt(_state.Priority);
        if (interruptedDevice != null)
        {
            interruptedDevice.AcceptInterrupt();
            HandleInterrupt(interruptedDevice.GetType().Name, interruptedDevice.InterruptVectorAddress);
        }
        else if (_lastCommand is WAIT)
        {
            return true;
        }

        try
        {
            var word = _memory.GetWord(_state.Registers[7]);
            _state.Registers[7] += 2;

            _lastCommand = _opcodeIdentifier.GetCommand(word);
            _lastCommand.Execute(_lastCommand.GetArguments(word));

            if (_lastCommand is TrapInstruction)
            {
                _trapStack.Push(_lastCommand.GetType().Name);
            }
            else if (_lastCommand is TrapReturn)
            {
                _trapStack.Pop();
            }
        }
        catch (HaltException e) when (e.IsExpected)
        {
            return false;
        }
        catch (Exception e)
        {
            HandleHardwareTrap(e);
        }

        return true;
    }

    public Task<bool> ExecuteNextInstructionAsync() => Task.Run(ExecuteNextInstruction);

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
        // 000000;HALT - symbol for line
        // #/path/to/device.dll - devices section

        var address = Project.ProgramAddress;
        while (await reader.ReadLineAsync() is { } line)
        {
            if (line.StartsWith("#"))
            {
                AddDevice(line);
                continue;
            }

            var tokens = line.Split(';', StringSplitOptions.TrimEntries);

            var code = tokens[0];
            var isRelocatable = code.EndsWith('\'');

            var word = isRelocatable
                ? Convert.ToUInt16(code[..6], 8) + Project.ProgramAddress
                : Convert.ToUInt16(code, 8);

            if (word > ushort.MaxValue)
            {
                throw new OutOfMemoryException("Program is too large");
            }

            _memory.SetWord(address, (ushort)word);

            var symbol = tokens.ElementAtOrDefault(1);
            _symbols.Add(address, symbol);

            address += 2;
        }
    }

    public void AddBreakpoint(ushort address) => _breakpoints.Add(address);

    public void RemoveBreakpoint(ushort address) => _breakpoints.Remove(address);

    private void AddDevice(string path)
    {
        _deviceValidator.ThrowIfInvalid(path);
        _devicesManager.Add(path);
    }

    private void HandleHardwareTrap(Exception e)
    {
        ushort address;

        if (e is BusException or OddAddressException)
        {
            if (_trapStack.Any(t => _trapsToHalt.Any(m => m == t)))
            {
                throw new HaltException(false,
                    $"Get bus error while already in trap. Trap stack: {string.Join("->", _trapStack)}");
            }

            address = 4;
        }
        else if (e is InvalidInstructionException)
        {
            address = 4;
        }
        else if (e is ReservedInstructionException)
        {
            address = 8; // 0o10
        }
        else
        {
            throw new Exception($"Unknown error '{e.GetType()}', '{e.Message}'");
        }

        HandleInterrupt(e.GetType().Name, address);
    }

    private void HandleInterrupt(string name, ushort address)
    {
        TrapInstruction.HandleInterrupt(_bus, _state, address);
        _trapStack.Push(name);
    }
}