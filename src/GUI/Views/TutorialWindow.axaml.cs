using Avalonia;
using Avalonia.Controls;

namespace GUI.Views;

public partial class TutorialWindow : Window
{
    public TutorialWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }
}