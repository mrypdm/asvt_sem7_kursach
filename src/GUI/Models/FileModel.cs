using System.IO;

namespace GUI.Models;

/// <summary>
/// File model
/// </summary>
public record FileModel
{
    /// <summary>
    /// Default file name
    /// </summary>
    private const string DefaultFileName = "main.asm";

    /// <summary>
    /// Path to file
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// File name
    /// </summary>
    public string FileName => FilePath == null ? DefaultFileName : Path.GetFileName(FilePath);

    /// <summary>
    /// File content
    /// </summary>
    public string Text { get; set; } = string.Empty;
}