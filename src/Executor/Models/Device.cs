namespace Executor.Models;

/// <summary>
/// Model of device
/// </summary>
public record Device(string Name, ushort ControlAddress, ushort ControlValue, ushort BufferAddress, ushort BufferValue,
    ushort InterruptVectorAddress, bool HasInterrup);