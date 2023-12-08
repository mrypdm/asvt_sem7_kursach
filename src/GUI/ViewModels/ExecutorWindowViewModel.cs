using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
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
    private readonly Executor.Executor _executor;
    private bool _memoryAsWord = true;
    private Tab _currentTab = Tab.State;

    private ObservableCollection<IMemoryModel> _memory;
    private ObservableCollection<CodeModel> _code;

    private CancellationTokenSource _cancelRunToken;

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
    /// <param name="executor">Executor</param>
    public ExecutorViewModel(ExecutorWindow view, IMessageBoxManager messageBoxManager, Executor.Executor executor) :
        base(view)
    {
        _messageBoxManager = messageBoxManager;
        _executor = executor;

        StartExecutionCommand = ReactiveCommand.CreateFromTask(RunAsync);
        PauseExecutionCommand = ReactiveCommand.Create(PauseAsync);
        MakeStepCommand = ReactiveCommand.CreateFromTask(async () => { await MakeStepAsync(); });
        ResetExecutorCommand = ReactiveCommand.CreateFromTask(ResetExecutorAsync);
        ChangeMemoryModeCommand = ReactiveCommand.Create(ChangeMemoryMode);
        FindAddressCommand = ReactiveCommand.CreateFromTask<string>(FindAddressAsync);

        Tabs = Enum.GetValues<Tab>().ToObservableCollection();
        Memory = AsWords().ToObservableCollection();
        Devices = Array.Empty<Device>().ToObservableCollection();
        CodeLines = Array.Empty<CodeModel>().ToObservableCollection();

        InitCode();

        InitContext();

        View.CodeGrid.SelectedIndex = 0;
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
    public ObservableCollection<RegisterModel> Registers =>
        _executor.Registers.Select((m, i) => new RegisterModel($"R{i}", m)).ToObservableCollection();

    // TODO
    /// <inheritdoc />
    public ObservableCollection<ProcessorStateWordModel> ProcessorStateWord =>
        new[] { new ProcessorStateWordModel(_executor.PSW) }.ToObservableCollection();

    // TODO
    /// <inheritdoc />
    public ObservableCollection<IMemoryModel> Memory
    {
        get => _memory;
        set => this.RaiseAndSetIfChanged(ref _memory, value);
    }

    // TODO
    /// <inheritdoc />
    public ObservableCollection<Device> Devices { get; }

    // TODO
    /// <inheritdoc />
    public ObservableCollection<CodeModel> CodeLines
    {
        get => _code;
        set => this.RaiseAndSetIfChanged(ref _code, value);
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
    private async Task<bool> MakeStepAsync()
    {
        try
        {
            await Task.Run(() => _executor.ExecuteNextInstruction());
            this.RaisePropertyChanged(nameof(Memory));
            this.RaisePropertyChanged(nameof(CodeLines));
            this.RaisePropertyChanged(nameof(Registers));
            this.RaisePropertyChanged(nameof(ProcessorStateWord));

            var line = CodeLines.First(m => m.Address == Convert.ToString(_executor.Registers.ElementAt(7), 8));
            var index = CodeLines.IndexOf(line);

            View.CodeGrid.SelectedIndex = index;
        }
        catch (Exception e)
        {
            await _messageBoxManager.ShowErrorMessageBox(e.Message, View);
            return false;
        }

        return true;
    }

    // TODO
    private async Task RunAsync()
    {
        _cancelRunToken = new CancellationTokenSource();

        while (!_cancelRunToken.IsCancellationRequested && await MakeStepAsync())
        {
        }

        this.RaisePropertyChanged(nameof(Memory));
        this.RaisePropertyChanged(nameof(CodeLines));
        this.RaisePropertyChanged(nameof(Registers));
        this.RaisePropertyChanged(nameof(ProcessorStateWord));

        _cancelRunToken.Dispose();
        _cancelRunToken = null;
    }

    // TODO
    private void PauseAsync() => _cancelRunToken?.Cancel();

    // TODO
    private Task ResetExecutorAsync() => Task.CompletedTask;

    private void ChangeMemoryMode()
    {
        _memoryAsWord = !_memoryAsWord;
        this.RaisePropertyChanged(nameof(ChangeMemoryModeCommandHeader));
        Memory = _memoryAsWord ? AsWords().ToObservableCollection() : AsBytes().ToObservableCollection();
    }

    // TODO
    private IEnumerable<IMemoryModel> AsWords()
    {
        var count = _executor.Memory.GetMemory().Count;
        for (ushort i = 0; i < count; i += 2)
        {
            yield return new WordModel(i, _executor.Memory.GetWord(i));
        }
    }

    // TODO
    private IEnumerable<IMemoryModel> AsBytes()
        => _executor.Memory.GetMemory().Select((m, i) => new ByteModel((ushort)i, m));

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

    private void InitCode()
    {
        CodeLines = new ObservableCollection<CodeModel>();
        var start = _executor.StartProgramAddress;
        var count = _executor.LengthOfProgram;

        for (var i = start; i <= start + count; i += 2)
        {
            CodeLines.Add(new CodeModel(i, _executor.Memory.GetWord(i), string.Empty));
        }
    }
}