using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GUI.Models.Tutorial;
using GUI.ViewModels.Abstraction;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

/// <inheritdoc cref="ITutorialWindowViewModel" />
public class TutorialWindowViewModel : WindowViewModel<TutorialWindow>, ITutorialWindowViewModel
{
    private Section _selectedSection;

    public TutorialWindowViewModel() : base(null)
    {
    }

    public TutorialWindowViewModel(TutorialWindow view) : base(view)
    {
        var commands = new Section
        {
            Name = "Commands",
            Sections = new ObservableCollection<Section>(Directory.GetFiles("Assets/tutorial/Commands")
                .Where(m => Path.GetExtension(m) == ".md")
                .Select(file => new Section
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    Content = File.ReadAllText(file)
                }))
        };

        var addressing = new Section
        {
            Name = "Addressing",
            Content = File.ReadAllText("Assets/tutorial/Addressing.md")
        };

        Sections = new ObservableCollection<Section>(new[] { addressing, commands });

        InitContext();
    }

    /// <inheritdoc />
    public string MarkDownText => _selectedSection.Content;

    /// <inheritdoc cref="ITutorialWindowViewModel" />
    public ObservableCollection<Section> Sections { get; }

    /// <inheritdoc cref="ITutorialWindowViewModel" />
    public Section SelectedSection
    {
        get => _selectedSection;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedSection, value);
            this.RaisePropertyChanged(nameof(MarkDownText));
        }
    }
}