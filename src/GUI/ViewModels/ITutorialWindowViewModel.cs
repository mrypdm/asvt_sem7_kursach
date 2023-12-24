using System.Collections.ObjectModel;
using GUI.Models.Tutorial;
using GUI.Views;

namespace GUI.ViewModels;

/// <summary>
/// View model for <see cref="TutorialWindow"/>
/// </summary>
public interface ITutorialWindowViewModel : IWindowViewModel<TutorialWindow>
{
    /// <summary>
    /// Current text
    /// </summary>
    string MarkDownText { get; }
    
    /// <summary>
    /// Sections
    /// </summary>
    ObservableCollection<Section> Sections { get; }
    
    /// <summary>
    /// Current section
    /// </summary>
    Section SelectedSection { get; set; }
}