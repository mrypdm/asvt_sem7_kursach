namespace Executor.Models;

/// <summary>
/// Model of device
/// </summary>
/// <param name="Name">Name of device</param>
/// <param name="ControlAddress">Address of control register</param>
/// <param name="ControlValue">Value of control register</param>
/// <param name="BufferAddress">Address of buffer register</param>
/// <param name="BufferValue">Value of buffer register</param>
/// <param name="InterruptVectorAddress">Address of interrupt vector</param>
public record Device(
    string Name,
    ushort ControlAddress, ushort ControlValue,
    ushort BufferAddress, ushort BufferValue,
    ushort InterruptVectorAddress);