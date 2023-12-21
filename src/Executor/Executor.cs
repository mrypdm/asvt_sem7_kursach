using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Devices.Managers;
using Devices.Providers;
using Devices.Validators;
using Domain.Models;
using Executor.Commands.MiscellaneousInstructions;
using Executor.Commands.Traps;
using Executor.CommandTypes;
using Executor.Exceptions;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;
using Executor.Models;

namespace Executor;

public class Executor
{
    private bool _initialized;
    private ushort _lengthOfProgram;
    private ICommand _lastCommand;

    private readonly Stack<Type> _trapStack = new();

    private readonly HashSet<Type> _typesToHalt = new()
    {
        typeof(BusException),
        typeof(TrapInstruction),
        typeof(TrapReturn)
    };

    private readonly IState _state;
    private readonly IStorage _memory;
    private readonly IDeviceValidator _deviceValidator;
    private readonly IDevicesManager _devicesManager;
    private readonly Bus _bus;

    private readonly CommandParser _commandParser;
    private readonly Dictionary<ushort, string> _symbols = new();
    private readonly HashSet<ushort> _breakpoints = new();

    public ushort ProcessorStateWord => _state.ProcessorStateWord;

    public IReadOnlyCollection<ushort> Registers => _state.Registers;

    public IReadOnlyStorage Memory => _memory;

    public IEnumerable<Device> Devices => _devicesManager.Devices.Select(DeviceExtensions.ToDto);

    public IEnumerable<Command> Commands
    {
        get
        {
            for (var i = Project.ProgramAddress; i < Project.ProgramAddress + _lengthOfProgram; i += 2)
            {
                yield return new Command(i, _memory.GetWord(i), _breakpoints.Contains(i), _symbols[i]);
            }
        }
    }

    public IProject Project { get; private set; }

    public Executor()
    {
        _state = new State();
        _memory = new Memory();
        var provider = new DeviceProvider();
        _devicesManager = new DevicesManager(provider);
        _deviceValidator = new DeviceValidator(provider);
        _bus = new Bus(_memory, _devicesManager);
        _commandParser = new CommandParser(_bus, _state);
    }

    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        Init();

        var res = true;

        while (!cancellationToken.IsCancellationRequested && res)
        {
            res = await ExecuteNextInstructionAsync();

            if (_breakpoints.Contains(_state.Registers[7]))
            {
                break;
            }

            await Task.Yield();
        }

        return res;
    }

    public bool ExecuteNextInstruction()
    {
        Init();

        var interruptedDevice = _bus.GetInterrupt(_state.Priority);
        if (interruptedDevice != null)
        {
            interruptedDevice.AcceptInterrupt();
            HandleInterrupt(interruptedDevice.GetType(), interruptedDevice.InterruptVectorAddress);
            return true;
        }

        if (_lastCommand is WAIT)
        {
            return true;
        }

        try
        {
            var needTrace = _state.T;
            var word = _memory.GetWord(_state.Registers[7]);
            _state.Registers[7] += 2;

            if (_lastCommand is TrapInstruction)
            {
                _trapStack.Push(_lastCommand.GetType());
            }
            else if (_lastCommand is TrapReturn)
            {
                _trapStack.Pop();
            }

            _lastCommand = _commandParser.GetCommand(word);
            _lastCommand.Execute(_lastCommand.GetArguments(word));

            if (needTrace && _lastCommand is not RTT and not TrapInstruction and not WAIT)
            {
                HandleInterrupt(typeof(Trace), Trace.InterruptVectorAddress);
            }
        }
        catch (HaltException e)
        {
            if (e.IsExpected)
            {
                return false;
            }

            throw;
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
        _trapStack.Clear();
        _symbols.Clear();
        _devicesManager.Clear();
        _state.ProcessorStateWord = 0;
        _memory.Init();
        Array.Fill<ushort>(_state.Registers, 0);

        _state.Registers[6] = Project.StackAddress;
        _state.Registers[7] = Project.ProgramAddress;

        using var reader = new StreamReader(Project.ProjectBinary);

        var address = Project.ProgramAddress;
        while (await reader.ReadLineAsync() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
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

        _lengthOfProgram = (ushort)(address - Project.ProgramAddress);

        foreach (var device in Project.Devices)
        {
            _deviceValidator.ThrowIfInvalid(device);
            _devicesManager.Add(device);
        }
    }

    public void AddBreakpoint(ushort address) => _breakpoints.Add(address);

    public void RemoveBreakpoint(ushort address) => _breakpoints.Remove(address);

    private void HandleHardwareTrap(Exception e)
    {
        ushort address;

        if (e is BusException)
        {
            if (_trapStack.Any(t => _typesToHalt.Contains(t)) || _lastCommand is TrapInstruction or TrapReturn)
            {
                throw new HaltException(false,
                    $"Get bus error while already in trap. Trap stack: {string.Join("->", _trapStack.Select(m => m.Name))}");
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

        HandleInterrupt(e.GetType(), address);
    }

    private void HandleInterrupt(Type type, ushort address)
    {
        TrapInstruction.HandleInterrupt(_bus, _state, address);
        _trapStack.Push(type);
    }

    private void Init()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;
        _bus.Init();
    }
}