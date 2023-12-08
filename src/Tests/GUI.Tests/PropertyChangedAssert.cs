using System.ComponentModel;

namespace GUI.Tests;

/// <summary>
/// Assert for <see cref="INotifyPropertyChanged.PropertyChanged"/> event
/// </summary>
public class PropertyChangedAssert
{
    private string _property;
    
    public PropertyChangedAssert(INotifyPropertyChanged source)
    {
        source.PropertyChanged += (_, args) => { _property = args.PropertyName; };
    }

    /// <summary>
    /// Check changed property
    /// </summary>
    /// <param name="expected">Expected property</param>
    public void Assert(string expected)
    {
        NUnit.Framework.Assert.That(_property, Is.EqualTo(expected));
    }
}