﻿using System.Collections.Generic;
using System.Runtime.Loader;
using ExternalDeviceSdk;

namespace ExternalDevices;

/// <summary>
/// Context of external device
/// </summary>
public class ExternalDeviceModel
{
    /// <summary>
    /// Path to assembly file
    /// </summary>
    public string AssemblyPath { get; set; }
    
    /// <summary>
    /// Context of assembly
    /// </summary>
    public AssemblyLoadContext AssemblyContext { get; set; }
    
    /// <summary>
    /// External device object
    /// </summary>
    public List<IExternalDevice> ExternalDevices { get; set; }
}