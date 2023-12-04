using Shared.Helpers;

namespace GUI.Models;

/// <summary>
/// File model
/// </summary>
public record FileModel
{
    /// <summary>
    /// Path to file
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// File name
    /// </summary>
    public string FileName => PathHelper.GetFileName(FilePath);

    /// <summary>
    /// File content
    /// </summary>
    public string Text { get; set; } = string.Empty;
    
    /// <summary>
    /// Is file in changed state and need to be saved
    /// </summary>
    public bool IsNeedSave { get; set; }
}