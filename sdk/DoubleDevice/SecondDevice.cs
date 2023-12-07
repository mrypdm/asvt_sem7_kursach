using System;
using DeviceSdk;

namespace DoubleDevice;

/// <inheritdoc />
public sealed class SecondDevice : IDevice
{
    /// <inheritdoc />
    public string Name => GetType().Name;

    /// <inheritdoc />
    public ushort BufferRegisterAddress => Convert.ToUInt16("177562", 8);

    /// <inheritdoc />
    public ushort ControlRegisterAddress => Convert.ToUInt16("177560", 8);

    /// <inheritdoc />
    public ushort InterruptVectorAddress => Convert.ToUInt16("60", 8);

    /// <inheritdoc />
    public bool HasInterrupt => false;

    /// <inheritdoc />
    public ushort BufferRegisterValue { get; set; }

    /// <inheritdoc />
    public ushort ControlRegisterValue { get; set; }

    /// <inheritdoc />
    public int Init()
    {
        BufferRegisterValue = 0;
        ControlRegisterValue = 0;
        return 0;
    }

    public void AcceptInterrupt()
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}