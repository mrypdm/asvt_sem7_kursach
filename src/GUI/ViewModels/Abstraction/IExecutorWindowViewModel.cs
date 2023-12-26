using System.Collections.ObjectModel;
using System.Reactive;
using Executor.Models;
using GUI.Models.Executor;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels.Abstraction;

/// <summary>
/// View model for <see cref="ExecutorWindow"/>
/// </summary>
public interface IExecutorWindowViewModel : IWindowViewModel<ExecutorWindow>
{
    /// <summary>
    /// Command for start execution
    /// </summary>
    ReactiveCommand<Unit, Unit> StartExecutionCommand { get; }

    /// <summary>
    /// Command for pause execution
    /// </summary>
    ReactiveCommand<Unit, Unit> PauseExecutionCommand { get; }

    /// <summary>
    /// Command for execute one command
    /// </summary>
    ReactiveCommand<Unit, Unit> MakeStepCommand { get; }

    /// <summary>
    /// Command for reset executor
    /// </summary>
    ReactiveCommand<Unit, Unit> ResetExecutorCommand { get; }

    /// <summary>
    /// Command for changing memory mode (word and byte)
    /// </summary>
    ReactiveCommand<Unit, Unit> ChangeMemoryModeCommand { get; }

    /// <summary>
    /// Command for find address in memory
    /// </summary>
    ReactiveCommand<string, Unit> FindAddressCommand { get; }

    /// <summary>
    /// Collection of registers
    /// </summary>
    ObservableCollection<RegisterModel> Registers { get; }

    /// <summary>
    /// Collection of PSW tokens (priority and flags)
    /// </summary>
    ObservableCollection<ProcessorStateWordModel> ProcessorStateWord { get; }

    /// <summary>
    /// Collection of memory cells
    /// </summary>
    ObservableCollection<IMemoryModel> Memory { get; }
    
    /// <summary>
    /// Index of selected cell of memory
    /// </summary>
    int SelectedMemoryCell { get; }

    /// <summary>
    /// Collection of devices
    /// </summary>
    ObservableCollection<Device> Devices { get; }
    
    /// <summary>
    /// Collection of code lines
    /// </summary>
    ObservableCollection<CodeLine> CodeLines { get; }
    
    /// <summary>
    /// Current line of code
    /// </summary>
    CodeLine SelectedLine { get; }
    
    /// <summary>
    /// Collection of tabs
    /// </summary>
    ObservableCollection<Tab> Tabs { get; }

    /// <summary>
    /// Header of <see cref="ChangeMemoryModeCommand"/>
    /// </summary>
    string ChangeMemoryModeCommandHeader { get; }

    /// <summary>
    /// Current selected tab
    /// </summary>
    Tab CurrentTab { get; set; }

    /// <summary>
    /// Is state tab visible (Registers and PSW)
    /// </summary>
    bool IsStateVisible { get; }

    /// <summary>
    /// Is memory tab visible
    /// </summary>
    bool IsMemoryVisible { get; }

    /// <summary>
    /// Is device tab visible
    /// </summary>
    bool IsDevicesVisible { get; }
}