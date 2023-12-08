using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Executor.Exceptions;
using GUI.Extensions;
using GUI.MessageBoxes;
using GUI.Models.Executor;
using GUI.Views;
using MsBox.Avalonia.Enums;
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
        MakeStepCommand = ReactiveCommand.CreateFromTask(MakeStepAsync);
        ResetExecutorCommand = ReactiveCommand.CreateFromTask(ResetExecutorAsync);
        ChangeMemoryModeCommand = ReactiveCommand.Create(ChangeMemoryMode);
        FindAddressCommand = ReactiveCommand.CreateFromTask<string>(FindAddressAsync);

        Tabs = Enum.GetValues<Tab>().ToObservableCollection();
        Memory = AsWords().ToObservableCollection();
        Devices = Array.Empty<Device>().ToObservableCollection();
        CodeLines = InitCode().ToObservableCollection();

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

    private Task FastStep() => Task.Run(() => _executor.ExecuteNextInstruction());

    // TODO
    private async Task MakeStepAsync()
    {
        try
        {
            await FastStep();
            UpdateState();
        }
        catch (HaltException e) when (e.IsExpected)
        {
            await _messageBoxManager.ShowMessageBoxAsync("Execute", "HALT is executed", ButtonEnum.Ok, Icon.Info,
                View);
        }
        catch (Exception e)
        {
            await _messageBoxManager.ShowErrorMessageBox(e.Message, View);
        }
    }

    // TODO
    private async Task RunAsync()
    {
        _cancelRunToken = new CancellationTokenSource();

        while (!_cancelRunToken.IsCancellationRequested)
        {
            try
            {
                await FastStep();
            }
            catch (HaltException e) when (e.IsExpected)
            {
                await _messageBoxManager.ShowMessageBoxAsync("Execute", "HALT is executed", ButtonEnum.Ok, Icon.Info,
                    View);
                break;
            }
            catch (Exception e)
            {
                await _messageBoxManager.ShowErrorMessageBox(e.Message, View);
            }
        }

        _cancelRunToken.Dispose();
        _cancelRunToken = null;

        UpdateState();
    }

    // TODO
    private void PauseAsync() => _cancelRunToken?.Cancel();

    // TODO
    private async Task ResetExecutorAsync()
    {
        await _executor.Reload();
        UpdateState();
    }

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

    private IEnumerable<CodeModel> InitCode()
    {
        var start = _executor.StartProgramAddress;
        var count = _executor.LengthOfProgram;

        for (var i = start; i <= start + count; i += 2)
        {
            yield return new CodeModel(i, _executor.Memory.GetWord(i), string.Empty);
        }
    }

    private void UpdateState()
    {
        CodeLines = InitCode().ToObservableCollection();
        Memory = _memoryAsWord ? AsWords().ToObservableCollection() : AsBytes().ToObservableCollection();
        this.RaisePropertyChanged(nameof(Registers));
        this.RaisePropertyChanged(nameof(ProcessorStateWord));
        var line = CodeLines.FirstOrDefault(m =>
            Convert.ToUInt16(m.Address, 8) == _executor.Registers.ElementAt(7));
        if (line != null)
        {
            var index = CodeLines.IndexOf(line);

            View.CodeGrid.SelectedIndex = index;
        }
        else
        {
            View.CodeGrid.SelectedIndex = -1;
        }
    }
}