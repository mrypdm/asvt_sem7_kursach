using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using GUI.Extensions;
using GUI.MessageBoxes;
using GUI.Models.Executor;
using GUI.Views;
using ReactiveUI;
using Shared.Converters;

namespace GUI.ViewModels;

public static class ExecutorTabs
{
    public const string State = nameof(State);

    public const string Memory = nameof(Memory);

    public const string Devices = nameof(Devices);
}

public class ExecutorViewModel : WindowViewModel<ExecutorWindow>, IExecutorWindowViewModel
{
    private readonly IMessageBoxManager _messageBoxManager;
    private bool _memoryAsWord = true;
    private Tab _currentTab = Tab.State;

    private ObservableCollection<IMemoryModel> _memory;

    private byte[] _testMemory = new byte[ushort.MaxValue + 1];

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
        var random = new Random();
        random.NextBytes(_testMemory);

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
        //Memory = Array.Empty<IMemoryModel>().ToObservableCollection();
        Memory = _testMemory.Select((m, i) => new ByteModel((ushort)i, m) as IMemoryModel).ToObservableCollection();
        Devices = Array.Empty<Device>().ToObservableCollection();
        CodeLines = Array.Empty<CodeModel>().ToObservableCollection();

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

    /// <inheritdoc />
    public ObservableCollection<RegisterModel> Registers { get; }

    /// <inheritdoc />
    public ObservableCollection<ProcessorStateWordModel> ProcessorStateWord { get; }

    /// <inheritdoc />
    public ObservableCollection<IMemoryModel> Memory
    {
        get => _memory;
        set => this.RaiseAndSetIfChanged(ref _memory, value);
    }

    /// <inheritdoc />
    public ObservableCollection<Device> Devices { get; }

    /// <inheritdoc />
    public ObservableCollection<CodeModel> CodeLines { get; }

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

    private async Task MakeStepAsync()
    {
    }

    private async Task RunAsync()
    {
    }

    private async Task PauseAsync()
    {
    }

    private async Task ResetExecutorAsync()
    {
    }

    private void ChangeMemoryMode()
    {
        _memoryAsWord = !_memoryAsWord;
        this.RaisePropertyChanged(nameof(ChangeMemoryModeCommandHeader));

        Memory = _memoryAsWord
            ? AsWords().ToObservableCollection()
            : _testMemory.Select((m, i) => new ByteModel((ushort)i, m) as IMemoryModel).ToObservableCollection();
    }

    private IEnumerable<IMemoryModel> AsWords()
    {
        for (var i = 0; i < _testMemory.Length; i += 2)
        {
            var low = _testMemory[i];
            var high = _testMemory[i + 1];
            var word = (high << 8) | low;
            yield return new WordModel((ushort)i, (ushort)word);
        }
    }

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

        View.MemoryGrid.SelectedIndex = address;
        View.MemoryGrid.ScrollIntoView(View.MemoryGrid.SelectedItem, View.MemoryGrid.Columns.FirstOrDefault());
    }
}