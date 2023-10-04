using Avalonia.Controls;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

namespace GUI.Managers;

public static class MessageBoxManager
{
    public static IMsBox<ButtonResult> GetMessageBox(string title, string text, ButtonEnum buttons, Icon icon,
        Window owner) =>
        MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentTitle = title,
            ContentMessage = text,
            ButtonDefinitions = buttons,
            Icon = icon,
            WindowIcon = owner.Icon,
            FontFamily = owner.FontFamily,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            MinWidth = 300
        });

    public static IMsBox<string> GetCustomMessageBox(string title, string text, Icon icon, Window owner,
        params ButtonDefinition[] buttons) =>
        MsBox.Avalonia.MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ContentTitle = title,
            ContentMessage = text,
            ButtonDefinitions = buttons,
            Icon = icon,
            WindowIcon = owner.Icon,
            FontFamily = owner.FontFamily,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            MinWidth = 300
        });

    public static IMsBox<ButtonResult> GetInputMessageBox(string title, string text, ButtonEnum buttons, Icon icon,
        Window owner, string paramName) =>
        MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
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
}