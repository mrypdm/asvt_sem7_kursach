using System.Runtime.InteropServices;

namespace TestCppSharedLibUsageCS;

internal static class CoolStringBuilderExtern
{
    private const string LibPath = "libTestCppSharedLib.dll";
    
    /// <summary>
    /// Creating builder instance
    /// </summary>
    /// <returns>Pointer to builder</returns>
    [DllImport(LibPath, EntryPoint = "CreateCoolStringBuilder")]
    internal static extern IntPtr Create();

    /// <summary>
    /// Clear builder instance
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    [DllImport(LibPath, EntryPoint = "DisposeCoolStringBuilder")]
    internal static extern void Dispose(IntPtr ptr);

    /// <summary>
    /// Append string to builder
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <param name="value">String to append</param>
    /// <returns>Pointer to builder</returns>
    [DllImport(LibPath, EntryPoint = "CoolStringBuilder_Append", CharSet = CharSet.Ansi)]
    internal static extern IntPtr Append(IntPtr ptr, string value);

    /// <summary>
    /// Append string with new-line to builder
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <param name="value">String to append</param>
    /// <returns>Pointer to builder</returns>
    [DllImport(LibPath, EntryPoint = "CoolStringBuilder_AppendLine", CharSet = CharSet.Ansi)]
    internal static extern IntPtr AppendLine(IntPtr ptr, string value);

    /// <summary>
    /// Build string
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <returns>Pointer to string</returns>
    [DllImport(LibPath, EntryPoint = "CoolStringBuilder_ToString")]
    private static extern IntPtr GetString(IntPtr ptr);

    /// <summary>
    /// Get string from builder 
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <returns>String from builder</returns>
    internal static string? GetStringSafe(IntPtr ptr)
    {
        var cStr = IntPtr.Zero;
        try
        {
            cStr = GetString(ptr);
            return Marshal.PtrToStringAnsi(cStr);
        }
        finally
        {
            Free(cStr);
        }
    }
    
    /// <summary>
    /// Free ptr
    /// </summary>
    [DllImport(LibPath, EntryPoint = "FreePtr")]
    private static extern void Free(IntPtr ptr);
}