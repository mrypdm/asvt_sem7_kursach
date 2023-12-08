using System;
using System.Collections.Generic;
using DeviceSdk;

namespace Devices.Managers;

/// <summary>
/// Manager for <see cref="IDevice"/>
/// </summary>
public interface IDevicesManager : IDisposable
{
    /// <summary>
    /// Collection of connected devices
    /// </summary>
    IReadOnlyCollection<IDevice> Devices { get; }

    /// <summary>
    /// Adds new device
    /// </summary>
    /// <param name="devicePath">Path to device</param>
    void Add(string devicePath);

    /// <summary>
    /// Removes device
    /// </summary>
    /// <param name="devicePath">Path to device</param>
    void Remove(string devicePath);

    /// <summary>
    /// Removes all devices
    /// </summary>
    void Clear();
}