using System;

namespace DeviceSdk;

/// <summary>
/// Device
/// </summary>
public interface IDevice : IDisposable
{
    /// <summary>
    /// Name of device
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Address of buffer register
    /// </summary>
    ushort BufferRegisterAddress { get; }

    /// <summary>
    /// Address for control device
    /// </summary>
    ushort ControlRegisterAddress { get; }

    /// <summary>
    /// Address of interrupt vector
    /// </summary>
    ushort InterruptVectorAddress { get; }
    
    /// <summary>
    /// Indicates whether the device sent an interrupt signal
    /// </summary>
    bool HasInterrupt { get; }
    
    /// <summary>
    /// Value of buffer register
    /// </summary>
    ushort BufferRegisterValue { get; set; }
    
    /// <summary>
    /// Value of control register
    /// </summary>
    ushort ControlRegisterValue { get; set; }

    /// <summary>
    /// Device initialization and reset
    /// </summary>
    /// <returns>Result code</returns>
    int Init();

    /// <summary>
    /// Method for executor to accept interrupt;
    /// </summary>
    void AcceptInterrupt();
}