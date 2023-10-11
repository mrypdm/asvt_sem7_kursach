namespace ExternalDeviceSdk;

/// <summary>
/// Interface for external device
/// </summary>
public interface IExternalDevice
{
    /// <summary>
    /// Address of buffer register
    /// </summary>
    ushort BufferRegisterAddress { get; }

    /// <summary>
    /// Address for control external device
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
    ushort BufferRegister { get; set; }
    
    /// <summary>
    /// Value of control register
    /// </summary>
    ushort ControlRegister { get; set; }

    /// <summary>
    /// External device initialization and reset
    /// </summary>
    /// <returns>Result code</returns>
    public int Init();
}