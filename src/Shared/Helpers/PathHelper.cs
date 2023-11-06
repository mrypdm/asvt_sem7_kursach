using System;
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
    public static string GetFullPath(string path) => string.IsNullOrWhiteSpace(path)
        ? string.Empty
        : Path.GetFullPath(path);

    /// <inheritdoc cref="Path.Combine(string?, string?)"/>
    public static string Combine(string path1, string path2) => Path.GetFullPath(Path.Combine(path1, path2));

    /// <inheritdoc cref="Path.GetDirectoryName(string?)"/>
    public static string GetDirectoryName(string path) => string.IsNullOrWhiteSpace(path)
        ? string.Empty
        : Path.GetDirectoryName(Path.GetFullPath(path));

    /// <inheritdoc cref="Path.GetFileName(string?)"/>
    public static string GetFileName(string path) => Path.GetFileName(path);

    /// <summary>
    /// Return relative path from <paramref name="path"/> to <paramref name="relativeTo"/>
    /// If <paramref name="relativeTo"/> is empty, then returns full path to <paramref name="path"/>
    /// If <paramref name="path"/> is empty, then returns <see cref="string.Empty"/> string
    /// </summary>
    public static string GetRelativePath(string relativeTo, string path)
    {
        if (string.IsNullOrWhiteSpace(relativeTo))
        {
            return GetFullPath(path);
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        return Path.GetRelativePath(relativeTo, path);
    }

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