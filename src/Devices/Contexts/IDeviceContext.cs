using System;
using System.Collections.Generic;
using DeviceSdk;

namespace Devices.Contexts;

/// <summary>
/// Context of device
/// </summary>
public interface IDeviceContext : IDisposable
{
    /// <summary>
    /// Path to assembly file
    /// </summary>
    string AssemblyPath { get; }

    /// <summary>
    /// Devices in current context
    /// </summary>
    IReadOnlyCollection<IDevice> Devices { get; }
}