using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using GUI.Extensions;
using GUI.MessageBoxes;
using GUI.Models.Executor;
using GUI.Views;
using ReactiveUI;
using Shared.Converters;

namespace GUI.ViewModels;

public class ExecutorViewModel : WindowViewModel<ExecutorWindow>, IExecutorWindowViewModel
{
    private readonly IMessageBoxManager _messageBoxManager;
    private bool _memoryAsWord = true;
    private Tab _currentTab = Tab.State;

    private CodeLine _selectedLine;
    private int _selectedMemoryCell;
    private ObservableCollection<IMemoryModel> _memory;

    /// <summary>
    /// Constructor for designer
    /// </summary>
    public ExecutorViewModel() : base(null)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="ExecutorViewModel"/>
    /// </summary>
    /// <param name="view">Executor window</param>
    /// <param name="messageBoxManager">Message box manager</param>
    public ExecutorViewModel(ExecutorWindow view, IMessageBoxManager messageBoxManager) : base(view)
    {
        _messageBoxManager = messageBoxManager;

        StartExecutionCommand = ReactiveCommand.CreateFromTask(RunAsync);
        PauseExecutionCommand = ReactiveCommand.CreateFromTask(PauseAsync);
        MakeStepCommand = ReactiveCommand.CreateFromTask(MakeStepAsync);
        ResetExecutorCommand = ReactiveCommand.CreateFromTask(ResetExecutorAsync);
        ChangeMemoryModeCommand = ReactiveCommand.Create(ChangeMemoryMode);
        FindAddressCommand = ReactiveCommand.CreateFromTask<string>(FindAddressAsync);

        Tabs = Enum.GetValues<Tab>().ToObservableCollection();
        Registers = Array.Empty<RegisterModel>().ToObservableCollection();
        ProcessorStateWord = Array.Empty<ProcessorStateWordModel>().ToObservableCollection();
        Memory = Array.Empty<IMemoryModel>().ToObservableCollection();
        Devices = Array.Empty<Device>().ToObservableCollection();
        CodeLines = Array.Empty<CodeLine>().ToObservableCollection();

        foreach (var line in CodeLines)
        {
            line.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CodeLine.Breakpoint))
                {
                    // update breakpoint in executor
                }
            };
        }

        InitContext();
    }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> StartExecutionCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> PauseExecutionCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> MakeStepCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> ResetExecutorCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> ChangeMemoryModeCommand { get; }

    /// <inheritdoc />
    public ReactiveCommand<string, Unit> FindAddressCommand { get; }

    // TODO
    /// <inheritdoc />
    public ObservableCollection<RegisterModel> Registers { get; }

    // TODO
    /// <inheritdoc />
    public ObservableCollection<ProcessorStateWordModel> ProcessorStateWord { get; }

    // TODO
    /// <inheritdoc />
    public ObservableCollection<IMemoryModel> Memory
    {
        get => _memory;
        set => this.RaiseAndSetIfChanged(ref _memory, value);
    }

    /// <inheritdoc />
    public int SelectedMemoryCell
    {
        get => _selectedMemoryCell;
        set => this.RaiseAndSetIfChanged(ref _selectedMemoryCell, value);
    }

    // TODO
    /// <inheritdoc />
    public ObservableCollection<Device> Devices { get; }

    // TODO
    /// <inheritdoc />
    public ObservableCollection<CodeLine> CodeLines { get; }

    /// <inheritdoc />
    public CodeLine SelectedLine
    {
        get => _selectedLine;
        set => this.RaiseAndSetIfChanged(ref _selectedLine, value);
    }

    /// <inheritdoc />
    public ObservableCollection<Tab> Tabs { get; }

    /// <inheritdoc />
    public string ChangeMemoryModeCommandHeader => _memoryAsWord ? "As Bytes" : "As Word";

    /// <inheritdoc />
    public Tab CurrentTab
    {
        get => _currentTab;
        set
        {
            _currentTab = value;
            this.RaisePropertyChanged(nameof(IsStateVisible));
            this.RaisePropertyChanged(nameof(IsMemoryVisible));
            this.RaisePropertyChanged(nameof(IsDevicesVisible));
        }
    }

    /// <inheritdoc />
    public bool IsStateVisible => CurrentTab == Tab.State;

    /// <inheritdoc />
    public bool IsMemoryVisible => CurrentTab == Tab.Memory;

    /// <inheritdoc />
    public bool IsDevicesVisible => CurrentTab == Tab.Devices;

    // TODO
    private Task MakeStepAsync() => Task.CompletedTask;

    // TODO
    private Task RunAsync() => Task.CompletedTask;

    // TODO
    private Task PauseAsync() => Task.CompletedTask;

    // TODO
    private Task ResetExecutorAsync() => Task.CompletedTask;

    private void ChangeMemoryMode()
    {
        _memoryAsWord = !_memoryAsWord;
        this.RaisePropertyChanged(nameof(ChangeMemoryModeCommandHeader));
        Memory = _memoryAsWord ? AsWords().ToObservableCollection() : AsBytes().ToObservableCollection();
    }

    // TODO
    private IEnumerable<IMemoryModel> AsWords() => Array.Empty<IMemoryModel>();

    // TODO
    private IEnumerable<IMemoryModel> AsBytes() => Array.Empty<IMemoryModel>();

    private async Task FindAddressAsync(string text)
    {
        var converter = new NumberStringConverter();
        var address = await converter.ConvertAsync(text);

        if (_memoryAsWord)
        {
            if (address % 2 == 1)
            {
                await _messageBoxManager.ShowErrorMessageBox("Word address must be even", View);
                return;
            }

            address /= 2;
        }

        SelectedMemoryCell = address;
    }
}