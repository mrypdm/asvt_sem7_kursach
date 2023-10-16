using System;
using System.Reflection;

namespace ExternalDevices.Models;

/// <summary>
/// Assembly context
/// </summary>
public interface IAssemblyContext : IDisposable
{
    /// <summary>
    /// Loaded assembly
    /// </summary>
    Assembly Assembly { get; }
    
    /// <summary>
    /// Loads assembly
    /// </summary>
    /// <param name="assemblyPath">Path to assembly file</param>
    /// <returns>Loaded assembly</returns>
    Assembly Load(string assemblyPath);
    
    /// <summary>
    /// Unloads assembly
    /// </summary>
    void Unload();
}