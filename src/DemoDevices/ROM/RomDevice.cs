using System;
using System.IO;
using System.Threading.Tasks;
using DeviceSdk;

namespace ROM;

/// <summary>
/// External ROM<br/>
/// </summary>
public sealed class RomDevice : IDevice
{
    private const ushort ErrorMask = 0b1111_0000_0000_0000;
    private const ushort BusyMask = 0b0000_1000_0000_0000;
    private const ushort ReadyMask = 0b0000_0000_1000_0000;
    private const ushort InterruptMask = 0b0000_0000_0100_0000;
    private const ushort FunctionMask = 0b0000_0000_0011_1110;
    private const ushort EnabledMask = 0b0000_0000_0000_0001;
    private const ushort UserMask = InterruptMask | FunctionMask | EnabledMask;

    private ushort? _address;
    private ushort? _value;

    private ushort _control;

    private Stream _stream;

    private Task _workingTask;

    /// <inheritdoc />
    public string Name => GetType().Name;

    /// <inheritdoc />
    public ushort BufferRegisterAddress { get; } = Convert.ToUInt16("177000", 8);

    /// <inheritdoc />
    public ushort ControlRegisterAddress { get; } = Convert.ToUInt16("177002", 8);

    /// <inheritdoc />
    public ushort InterruptVectorAddress { get; } = Convert.ToUInt16("60", 8);

    /// <inheritdoc />
    public bool HasInterrupt { get; private set; }

    /// <inheritdoc />
    public ushort BufferRegisterValue { get; set; }

    /// <inheritdoc />
    /// <remarks>
    ///     <b>Bits</b>:<br/>
    ///     b15-12 - error code<br/>
    ///     b11 - if device is busy<br/>
    ///     b10-8 - not using<br/>
    ///     b7 - device is ready<br/>
    ///     b6 - enable interrupts<br/>
    ///     b5-1 - function<br/>
    ///     b0 - enable<br/>
    /// </remarks>
    /// <remarks>
    ///     <b>Functions</b>:<br/>
    ///     00000 - 
    ///     00001 - read from address
    ///     00010 - write to address
    ///     00011 - set address
    ///     01000 - set data
    /// </remarks>
    /// <remarks>
    ///     <b>Errors</b>:<br/>
    ///     0000 - no error
    ///     0001 - odd address
    ///     0010 - invalid function
    ///     0011 - no address provided
    ///     0100 - no value provided 
    /// </remarks>
    public ushort ControlRegisterValue
    {
        get => _control;
        set => UpdateControl(value);
    }

    /// <inheritdoc />
    public int Init()
    {
        try
        {
            _stream = File.Open("memory.bin", FileMode.OpenOrCreate);
            _stream.SetLength(ushort.MaxValue + 1);
            IsReady = true;
            return 0;
        }
        catch (IOException)
        {
            return 1;
        }
        catch (UnauthorizedAccessException)
        {
            return 2;
        }
        catch (Exception)
        {
            return 255;
        }
    }

    public void AcceptInterrupt()
    {
        HasInterrupt = false;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _stream.Dispose();
    }

    private bool IsEnabled => (_control & EnabledMask) != 0;

    private Function Function
    {
        get => (Function)((_control & FunctionMask) >> 1);
        set => _control = (ushort)((_control & ~FunctionMask) | ((ushort)value << 1));
    }

    private bool IsInterruptEnabled => (_control & InterruptMask) != 0;

    private bool IsReady
    {
        set
        {
            _control = (ushort)((_control & ~ReadyMask) | ((value ? 1 : 0) << 7));

            if (IsInterruptEnabled && value)
            {
                HasInterrupt = true;
            }
        }
    }

    private bool IsBusy
    {
        get => (_control & BusyMask) != 0;
        set => _control = (ushort)((_control & ~BusyMask) | ((value ? 1 : 0) << 11));
    }

    private Error Error
    {
        set => _control = (ushort)((_control & ~ErrorMask) | ((ushort)value << 12));
    }

    private void UpdateControl(ushort value)
    {
        if (IsBusy)
        {
            return;
        }

        _control = (ushort)((_control & ~UserMask) | (value & UserMask));
        RunTask();
    }

    private void RunTask()
    {
        if (!IsEnabled)
        {
            return;
        }

        switch (Function)
        {
            case Function.NoFunction:
                break;
            case Function.Read:
                RunTask(ReadAsync);
                break;
            case Function.Write:
                RunTask(WriteAsync);
                break;
            case Function.SetAddress:
                RunTask(SetAddressAsync);
                break;
            case Function.SetValue:
                RunTask(SetValueAsync);
                break;
            default:
                RunTask(HandleInvalidFunctionAsync);
                break;
        }
    }

    private void RunTask(Func<Task> action)
    {
        Error = Error.NoError;
        IsBusy = true;
        IsReady = false;
        _workingTask = Task.Run(action);
    }

    private Task ReadAsync()
    {
        if (!_address.HasValue)
        {
            Error = Error.NoAddress;
            return EndTask();
        }

        var address = _address.Value;

        _stream.Seek(address, SeekOrigin.Begin);
        var low = _stream.ReadByte();
        var high = _stream.ReadByte();

        BufferRegisterValue = (ushort)((high << 8) | low);

        _address = null;

        return EndTask();
    }

    private Task WriteAsync()
    {
        if (!_address.HasValue)
        {
            Error = Error.NoAddress;
            return EndTask();
        }

        if (!_value.HasValue)
        {
            Error = Error.NoValue;
            return EndTask();
        }

        var address = _address.Value;
        var value = _value.Value;

        var low = (byte)(value & 255);
        var high = (byte)(value >> 8);

        _stream.Seek(address, SeekOrigin.Begin);
        _stream.WriteByte(low);
        _stream.WriteByte(high);

        _address = null;
        _value = null;

        return EndTask();
    }

    private Task SetAddressAsync()
    {
        if (BufferRegisterValue % 2 == 1)
        {
            Error = Error.OddAddress;
        }
        else
        {
            _address = BufferRegisterValue;
        }

        return EndTask();
    }

    private Task SetValueAsync()
    {
        _value = BufferRegisterValue;
        return EndTask();
    }

    private Task HandleInvalidFunctionAsync()
    {
        Error = Error.InvalidFunction;
        return EndTask();
    }

    private Task EndTask()
    {
        Function = Function.NoFunction;
        IsBusy = false;
        IsReady = true;
        return Task.CompletedTask;
    }
}