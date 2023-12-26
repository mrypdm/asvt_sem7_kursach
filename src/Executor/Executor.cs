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
using Executor.Models;
using Executor.States;
using Executor.Storages;

namespace Executor;

/// <summary>
/// Executor
/// </summary>
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
        typeof(InterruptReturn)
    };

    private readonly IState _state;
    private readonly IStorage _memory;
    private readonly IDeviceValidator _deviceValidator;
    private readonly IDevicesManager _devicesManager;
    private readonly Bus _bus;

    private readonly CommandParser _commandParser;
    private readonly Dictionary<ushort, string> _symbols = new();
    private readonly HashSet<ushort> _breakpoints = new();

    /// <summary>
    /// Value of PSW
    /// </summary>
    public ushort ProcessorStateWord => _state.ProcessorStateWord;

    /// <summary>
    /// Values of registers
    /// </summary>
    public IReadOnlyCollection<ushort> Registers => _state.Registers;

    /// <summary>
    /// Memory
    /// </summary>
    public IReadOnlyStorage Memory => _memory;

    /// <summary>
    /// Connected devices
    /// </summary>
    public IEnumerable<Device> Devices => _devicesManager.Devices.Select(DeviceExtensions.ToDto);

    /// <summary>
    /// Current program commands
    /// </summary>
    public IEnumerable<Command> Commands
    {
        get
        {
            for (var address = Project.ProgramAddress;
                 address < Project.ProgramAddress + _lengthOfProgram;
                 address += 2)
            {
                yield return new Command(address, _memory.GetWord(address), _breakpoints.Contains(address),
                    _symbols[address]);
            }
        }
    }

    /// <summary>
    /// Opened project
    /// </summary>
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

    /// <summary>
    /// Start execution
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of execution. true - program is not finished, false - program is finished</returns>
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

    /// <summary>
    /// Execute next instruction
    /// </summary>
    /// <returns>Result of execution. true - program is not finished, false - program is finished</returns>
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
            else if (_lastCommand is InterruptReturn)
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

    /// <summary>
    /// Execute next instruction asynchronously
    /// </summary>
    /// <returns>Result of execution. true - program is not finished, false - program is finished</returns>
    public Task<bool> ExecuteNextInstructionAsync() => Task.Run(ExecuteNextInstruction);

    /// <summary>
    /// Load project to executor
    /// </summary>
    /// <param name="project">Project to open</param>
    /// <exception cref="InvalidOperationException">
    ///     If <see cref="IProject.ProgramAddress"/> or <see cref="IProject.StackAddress"/> is odd
    /// </exception>
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

    /// <summary>
    /// Reloads projects
    /// </summary>
    /// <exception cref="OutOfMemoryException">If program is larger than memory</exception>
    public async Task Reload()
    {
        _initialized = false;
        _trapStack.Clear();
        _symbols.Clear();
        _devicesManager.Clear();
        _memory.Init();
        Array.Fill<ushort>(_state.Registers, 0);
        _state.ProcessorStateWord = 0;

        _state.Registers[6] = Project.StackAddress;
        _state.Registers[7] = Project.ProgramAddress;

        using var reader = new StreamReader(Project.ProjectBinary);

        var address = (int)Project.ProgramAddress;
        while (await reader.ReadLineAsync() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (address > _memory.Data.Count)
            {
                throw new OutOfMemoryException("Program is too large");
            }

            // 000000;symbol
            var tokens = line.Split(';', StringSplitOptions.TrimEntries);

            // 000000'
            var word = tokens[0].EndsWith('\'')
                ? Convert.ToUInt16(tokens[0][..6], 8) + Project.ProgramAddress
                : Convert.ToUInt16(tokens[0], 8);

            _memory.SetWord((ushort)address, (ushort)word);
            _symbols.Add((ushort)address, tokens.ElementAtOrDefault(1));

            address += 2;
        }

        _lengthOfProgram = (ushort)(address - Project.ProgramAddress);

        foreach (var device in Project.Devices)
        {
            _deviceValidator.ThrowIfInvalid(device);
            _devicesManager.Add(device);
        }
    }

    /// <summary>
    /// Add break point to address
    /// </summary>
    public void AddBreakpoint(ushort address) => _breakpoints.Add(address);

    /// <summary>
    /// Remove break point from address
    /// </summary>
    public void RemoveBreakpoint(ushort address) => _breakpoints.Remove(address);

    private void HandleHardwareTrap(Exception e)
    {
        ushort address;

        if (e is BusException)
        {
            if (_trapStack.Any(t => _typesToHalt.Contains(t)) || _lastCommand is TrapInstruction or InterruptReturn)
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