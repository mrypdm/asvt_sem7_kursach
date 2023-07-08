using System.Runtime.InteropServices;
using System.Text;

namespace TestCppSharedLibUsageCS;

internal static class CoolStringBuilderExtern
{
    private const string LibName = "libTestCppSharedLib";
    
    /// <summary>
    /// Creating builder instance
    /// </summary>
    /// <returns>Pointer to builder</returns>
    [DllImport(LibName, EntryPoint = "CoolStringBuilder_Create")]
    internal static extern IntPtr Create();

    /// <summary>
    /// Append string to builder
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <param name="str">String to append</param>
    /// <returns>Pointer to builder</returns>
    internal static void AppendSafe(IntPtr ptr, string str) => Append(ptr, Encoding.UTF8.GetBytes(str));

    /// <summary>
    /// Append string with new-line to builder
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <param name="str">String to append</param>
    /// <returns>Pointer to builder</returns>
    internal static void AppendLineSafe(IntPtr ptr, string str) => AppendLine(ptr, Encoding.UTF8.GetBytes(str));

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
            return Marshal.PtrToStringUTF8(cStr);
        }
        finally
        {
            Free(cStr);
        }
    }

    /// <summary>
    /// Clear builder instance
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <returns>Result code</returns>
    [DllImport(LibName, EntryPoint = "CoolStringBuilder_Dispose")]
    internal static extern ushort Dispose(IntPtr ptr);

    /// <summary>
    /// Append string to builder
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <param name="value">Bytes of string</param>
    /// <returns>Pointer to builder</returns>
    [DllImport(LibName, EntryPoint = "CoolStringBuilder_Append")]
    private static extern IntPtr Append(IntPtr ptr, byte[] value);

    /// <summary>
    /// Append string with new-line to builder
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <param name="value">Bytes of string</param>
    /// <returns>Pointer to builder</returns>
    [DllImport(LibName, EntryPoint = "CoolStringBuilder_AppendLine")]
    private static extern IntPtr AppendLine(IntPtr ptr, byte[] value);

    /// <summary>
    /// Build string
    /// </summary>
    /// <param name="ptr">Pointer to builder</param>
    /// <returns>Pointer to string</returns>
    [DllImport(LibName, EntryPoint = "CoolStringBuilder_ToString")]
    private static extern IntPtr GetString(IntPtr ptr);

    /// <summary>
    /// Free ptr
    /// </summary>
    [DllImport(LibName, EntryPoint = "FreePtr")]
    private static extern void Free(IntPtr ptr);
}