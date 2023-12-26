using DeviceSdk;
using Executor.Models;

namespace Executor.Extensions;

/// <summary>
/// Extensions for <see cref="IDevice"/>
/// </summary>
public static class DeviceExtensions
{
    /// <summary>
    /// Transforms device to DTO <see cref="Device"/>
    /// </summary>
    public static Device ToDto(this IDevice device) => new(
        device.Name,
        device.ControlRegisterAddress,
        device.ControlRegisterValue,
        device.BufferRegisterAddress,
        device.BufferRegisterValue,
        device.InterruptVectorAddress);
}