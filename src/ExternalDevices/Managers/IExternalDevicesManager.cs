using System;
using System.Collections.Generic;
using ExternalDeviceSdk;

namespace ExternalDevices.Managers;

/// <summary>
/// Manager for <see cref="IExternalDevice"/>
/// </summary>
public interface IExternalDevicesManager : IDisposable
{
    /// <summary>
    /// Collection of connected external devices
    /// </summary>
    IReadOnlyCollection<IExternalDevice> ExternalDevices { get; }

    /// <summary>
    /// Adds new device
    /// </summary>
    /// <param name="devicePath">Path to device</param>
    void AddDevice(string devicePath);

    /// <summary>
    /// Removes device
    /// </summary>
    /// <param name="devicePath">Path to external device</param>
    void RemoveDevice(string devicePath);

    /// <summary>
    /// Validates device
    /// </summary>
    /// <param name="devicePath">Path to external device</param>
    /// <returns>True if device is valid</returns>
    bool ValidateDevice(string devicePath);
}