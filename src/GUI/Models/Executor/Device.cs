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
        ControlValue = Convert.ToString(controlValue, 8).PadLeft(6, '0');
        BufferAddress = Convert.ToString(bufferAddress, 8).PadLeft(6, '0');
        BufferValue = Convert.ToString(bufferValue, 8).PadLeft(6, '0');
        InterruptVectorAddress = Convert.ToString(interruptAddress, 8).PadLeft(6, '0');
    }
    
    /// <summary>
    /// Name of device
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Address of control register
    /// </summary>
    public string ControlAddress { get; }

    /// <summary>
    /// Value of control register
    /// </summary>
    public string ControlValue { get; }

    /// <summary>
    /// Address of buffer register
    /// </summary>
    public string BufferAddress { get; }

    /// <summary>
    /// Value of buffer register
    /// </summary>
    public string BufferValue { get; }

    /// <summary>
    /// Address of interrupt vector
    /// </summary>
    public string InterruptVectorAddress { get; }
}