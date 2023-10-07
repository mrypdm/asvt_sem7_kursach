using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

namespace GUI.Managers;

public static class MessageBoxManager
{
    public static readonly ButtonDefinition CreateButton = new() { Name = "Create" };
    public static readonly ButtonDefinition OpenButton = new() { Name = "Open" };
    public static readonly ButtonDefinition ReopenButton = new() { Name = "Reopen" };
    public static readonly ButtonDefinition SkipButton = new() { Name = "Skip" };
    public static readonly ButtonDefinition CancelButton = new() { Name = "Cancel" };

    public static async Task<ButtonResult> ShowMessageBoxAsync(string title, string text, ButtonEnum buttons, Icon icon,
        Window owner)
    {
        return await MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentTitle = title,
            ContentMessage = text,
            ButtonDefinitions = buttons,
            Icon = icon,
            WindowIcon = owner.Icon,
            FontFamily = owner.FontFamily,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            MinWidth = 300
        }).ShowWindowDialogAsync(owner);
    }

    public static async Task<string> ShowCustomMessageBoxAsync(string title, string text, Icon icon, Window owner,
        params ButtonDefinition[] buttons)
    {
        return await MsBox.Avalonia.MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ContentTitle = title,
            ContentMessage = text,
            ButtonDefinitions = buttons,
            Icon = icon,
            WindowIcon = owner.Icon,
            FontFamily = owner.FontFamily,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            MinWidth = 300
        }).ShowWindowDialogAsync(owner);
    }

    public static async Task<(ButtonResult, string)> ShowInputMessageBoxAsync(string title, string text,
        ButtonEnum buttons,
        Icon icon,
        Window owner, string paramName)
    {
        var box = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentTitle = title,
            ContentMessage = text,
            ButtonDefinitions = buttons,
            Icon = icon,
            WindowIcon = owner.Icon,
            FontFamily = owner.FontFamily,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            InputParams = new InputParams { Label = paramName },
            MinWidth = 300
        });

        var res = await box.ShowWindowDialogAsync(owner);

        return (res, box.InputValue);
    }

    public static Task ShowErrorMessageBox(string message, Window owner) =>
        ShowMessageBoxAsync("Error", message, ButtonEnum.Ok, Icon.Error, owner);
}