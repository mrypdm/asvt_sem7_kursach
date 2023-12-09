using System;

namespace GUI.Models.Executor;

/// <summary>
/// Model of device
/// </summary>
public class Device
{
    public Device(string name, ushort controlAddress, ushort controlValue, ushort bufferAddress, ushort bufferValue,
        ushort interruptAddress)
    {
        Name = name;

        ControlAddress = Convert.ToString(controlAddress, 8).PadLeft(6, '0');
        Control = Convert.ToString(controlValue, 8).PadLeft(6, '0');

        BufferAddress = Convert.ToString(bufferAddress, 8).PadLeft(6, '0');
        Buffer = Convert.ToString(bufferValue, 8).PadLeft(6, '0');

        InterruptAddress = Convert.ToString(interruptAddress, 8).PadLeft(6, '0');
    }

    public string Name { get; }

    /// <summary>
    /// Address of control register
    /// </summary>
    public string ControlAddress { get; }

    /// <summary>
    /// Value of control register
    /// </summary>
    public string Control { get; }

    /// <summary>
    /// Address of buffer register
    /// </summary>
    public string BufferAddress { get; }

    /// <summary>
    /// Value of buffer register
    /// </summary>
    public string Buffer { get; }

    /// <summary>
    /// Address of interrupt vector
    /// </summary>
    public string InterruptAddress { get; }
}