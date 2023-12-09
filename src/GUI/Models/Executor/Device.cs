using System;

namespace GUI.Models.Executor;

/// <summary>
/// Model of device
/// </summary>
public class Device
{
    public Device(ushort controlRegisterAddress,ushort bufferRegisterAddress, ushort interruptVectorAddress, bool hasInterrupt)
    {
        Control = Convert.ToString(controlRegisterAddress, 8).PadLeft(6, '0');
        Buffer = Convert.ToString(bufferRegisterAddress, 8).PadLeft(6, '0');
        Interrupt = Convert.ToString(interruptVectorAddress, 8).PadLeft(6, '0');
        HasInterrupt = hasInterrupt ? 1 : 0;
    }
    
    /// <summary>
    /// Address of control register
    /// </summary>
    public string Control { get; }
    
    /// <summary>
    /// Address of buffer register
    /// </summary>
    public string Buffer { get; }
    
    /// <summary>
    /// Address of interrupt vector
    /// </summary>
    public string Interrupt { get; }
    
    /// <summary>
    /// Is device send interrupt signal
    /// </summary>
    public int HasInterrupt { get; }
}