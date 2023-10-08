using System.IO;

namespace Shared.Helpers;

/// <summary>
/// Helper for <see cref="Path"/>
/// </summary>
public static class PathHelper
{
    /// <inheritdoc cref="Path.Combine(string?, string?)"/>
    public static string Combine(string path1, string path2) => Path.GetFullPath(Path.Combine(path1, path2));

    /// <inheritdoc cref="Path.GetDirectoryName(string?)"/>
    public static string GetDirectoryName(string path) => Path.GetDirectoryName(Path.GetFullPath(path));

    /// <inheritdoc cref="Path.GetFileName(string?)"/>
    public static string GetFileName(string path) => Path.GetFileName(path);

    /// <inheritdoc cref="Path.GetRelativePath"/>
    public static string GetRelativePath(string relativeTo, string path) => Path.GetRelativePath(relativeTo, path);
}