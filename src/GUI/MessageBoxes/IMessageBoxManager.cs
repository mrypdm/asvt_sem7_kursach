using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

namespace GUI.MessageBoxes;

/// <summary>
/// Message box manager
/// </summary>
public interface IMessageBoxManager
{
    /// <summary>
    /// Show standard box without input
    /// </summary>
    /// <param name="title">Title of box</param>
    /// <param name="text">Text in box</param>
    /// <param name="buttons">Buttons</param>
    /// <param name="icon">Icon</param>
    /// <param name="owner">Owner of box</param>
    /// <returns>Clicked button</returns>
    Task<ButtonResult> ShowMessageBoxAsync(string title, string text, ButtonEnum buttons, Icon icon, Window owner);

    /// <summary>
    /// Show custom box without input
    /// </summary>
    /// <param name="title">Title of box</param>
    /// <param name="text">Text in box</param>
    /// <param name="icon">Icon</param>
    /// <param name="owner">Owner of box</param>
    /// <param name="buttons">Buttons</param>
    /// <returns>Name of clicked button</returns>
    Task<string> ShowCustomMessageBoxAsync(string title, string text, Icon icon, Window owner,
        params ButtonDefinition[] buttons);

    /// <summary>
    /// Show standard box with input
    /// </summary>
    /// <param name="title">Title of box</param>
    /// <param name="text">Text in box</param>
    /// <param name="buttons">Buttons</param>
    /// <param name="icon">Icon</param>
    /// <param name="owner">Owner of box</param>
    /// <param name="paramName">Name of input param</param>
    /// <returns>Clicked button and value of param</returns>
    Task<(ButtonResult, string)> ShowInputMessageBoxAsync(string title, string text, ButtonEnum buttons, Icon icon,
        Window owner, string paramName);

    /// <summary>
    /// Show error box
    /// </summary>
    /// <param name="message">Text in box</param>
    /// <param name="owner">Owner of box</param>
    Task ShowErrorMessageBox(string message, Window owner);
}