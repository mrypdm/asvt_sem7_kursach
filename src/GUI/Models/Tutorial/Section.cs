using System.Collections.ObjectModel;

namespace GUI.Models.Tutorial;

/// <summary>
/// Section of tutorial
/// </summary>
public class Section
{
    /// <summary>
    /// Name of section
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Content of section
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Child sections
    /// </summary>
    public ObservableCollection<Section> Sections { get; init; }
}