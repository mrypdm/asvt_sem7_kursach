using System.IO;

namespace Shared.Helpers;

/// <summary>
/// Helper for <see cref="Path"/>
/// </summary>
public static class PathHelper
{
    public enum PathType
    {
        File,
        Directory,
        UnExisting
    }

    /// <inheritdoc cref="Path.GetFullPath(string)"/>
    public static string GetFullPath(string path) => Path.GetFullPath(path);
    
    /// <inheritdoc cref="Path.Combine(string?, string?)"/>
    public static string Combine(string path1, string path2) => Path.GetFullPath(Path.Combine(path1, path2));

    /// <inheritdoc cref="Path.GetDirectoryName(string?)"/>
    public static string GetDirectoryName(string path) => Path.GetDirectoryName(Path.GetFullPath(path));

    /// <inheritdoc cref="Path.GetFileName(string?)"/>
    public static string GetFileName(string path) => Path.GetFileName(path);

    /// <inheritdoc cref="Path.GetRelativePath"/>
    public static string GetRelativePath(string relativeTo, string path) => Path.GetRelativePath(relativeTo, path);

    /// <summary>
    /// Gives the type of the object pointed to by the path
    /// </summary>
    /// <param name="path">Path to object</param>
    /// <returns>Type of path</returns>
    public static PathType GetPathType(string path)
    {
        if (File.Exists(path))
        {
            return PathType.File;
        }

        if (Directory.Exists(path))
        {
            return PathType.Directory;
        }

        return PathType.UnExisting;
    }
}