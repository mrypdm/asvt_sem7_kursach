using System;
using System.Reflection;
using System.Runtime.Loader;

namespace ExternalDevices.Models;

/// <inheritdoc />
public class AssemblyContext : IAssemblyContext
{
    private readonly AssemblyLoadContext _context;

    public AssemblyContext(string name)
    {
        _context = new AssemblyLoadContext(name, true);
    }

    /// <inheritdoc />
    public Assembly Assembly { get; private set; }

    /// <inheritdoc />
    public Assembly Load(string assemblyPath)
    {
        if (Assembly != null)
        {
            throw new InvalidOperationException("Assembly has been already loaded");
        }
        
        Assembly = _context.LoadFromAssemblyPath(assemblyPath);
        return Assembly;
    }

    /// <inheritdoc />
    public void Unload()
    {
        _context.Unload();
        Assembly = null;
    }
}