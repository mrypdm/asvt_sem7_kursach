﻿using System;
using GUI.ViewModels;

namespace GUI.Exceptions;

/// <summary>
/// The exception that is thrown when the tab already exists
/// </summary>
public class TabExistsException : InvalidOperationException
{
    public TabExistsException(string message = "Tab already exists", Exception innerException = null) :
        base(message, innerException)
    {
    }
    
    /// <summary>
    /// Reference to an existing tab
    /// </summary>
    public FileTabViewModel Tab { get; init; }
}