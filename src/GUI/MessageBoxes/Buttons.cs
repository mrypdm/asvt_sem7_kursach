using MsBox.Avalonia.Models;

namespace GUI.MessageBoxes;

/// <summary>
/// Buttons for <see cref="MessageBoxManager"/>
/// </summary>
public static class Buttons
{
    public static readonly ButtonDefinition CreateButton = new() { Name = "Create" };
    public static readonly ButtonDefinition OpenButton = new() { Name = "Open" };
    public static readonly ButtonDefinition ReopenButton = new() { Name = "Reopen" };
    public static readonly ButtonDefinition SkipButton = new() { Name = "Skip" };
    public static readonly ButtonDefinition CancelButton = new() { Name = "Cancel" };
}