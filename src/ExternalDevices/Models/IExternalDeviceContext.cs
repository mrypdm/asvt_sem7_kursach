using System;
using System.Collections.Generic;
using ExternalDeviceSdk;

namespace ExternalDevices.Models;

/// <summary>
/// Context of external device
/// </summary>
public interface IExternalDeviceContext : IDisposable
{
    /// <summary>
    /// Path to assembly file
    /// </summary>
    string AssemblyPath { get; }

    /// <summary>
    /// External device object
    /// </summary>
    IReadOnlyCollection<IExternalDevice> ExternalDevices { get; }
}