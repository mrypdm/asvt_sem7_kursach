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
        ControlAddress = controlAddress;
        ControlValue = controlValue;
        BufferAddress = bufferAddress;
        BufferValue = bufferValue;
        InterruptVectorAddress = interruptAddress;
    }
    
    /// <summary>
    /// Name of device
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Address of control register
    /// </summary>
    public ushort ControlAddress { get; }

    /// <summary>
    /// Value of control register
    /// </summary>
    public ushort ControlValue { get; }

    /// <summary>
    /// Address of buffer register
    /// </summary>
    public ushort BufferAddress { get; }

    /// <summary>
    /// Value of buffer register
    /// </summary>
    public ushort BufferValue { get; }

    /// <summary>
    /// Address of interrupt vector
    /// </summary>
    public ushort InterruptVectorAddress { get; }
}