using Avalonia.Controls;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace GUI.Managers;

public static class MessageBoxManager
{
    public static IMsBox<ButtonResult> GetMessageBox(string title, string text, ButtonEnum buttons, Icon icon,
        WindowIcon windowIcon) => MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
    {
        ContentTitle = title,
        ContentMessage = text,
        ButtonDefinitions = buttons,
        Icon = icon,
        WindowIcon = windowIcon,
        WindowStartupLocation = WindowStartupLocation.CenterOwner,
    });
}