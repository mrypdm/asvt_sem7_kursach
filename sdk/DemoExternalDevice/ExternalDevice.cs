using System;
using ExternalDeviceSdk;

namespace DemoExternalDevice;

/// <summary>
/// Demo external device - TTY input. Example taken from the book
/// "Organization and Assembly Language Programming for the PDP-1 and VAX-11" by Wen C. Lin Computer
/// </summary>
public sealed class ExternalDevice : IExternalDevice
{
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

    public void Dispose()
    {
    }
}