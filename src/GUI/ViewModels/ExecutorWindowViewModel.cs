using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Executor.Exceptions;
using Executor.Models;
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
    private readonly Executor.Executor _executor;
    private readonly IMessageBoxManager _messageBoxManager;
    private bool _memoryAsWord = true;
    private Tab _currentTab = Tab.State;

    private CodeLine _selectedLine;
    private int _selectedMemoryCell;
    private ObservableCollection<IMemoryModel> _memory;
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
    /// <param name="executor">Executor</param>
    /// <param name="messageBoxManager">Message box manager</param>
    public ExecutorViewModel(ExecutorWindow view, Executor.Executor executor, IMessageBoxManager messageBoxManager) :
        base(view)
    {
        _executor = executor;
        _executor.LoadProgram().Wait();
        
        _messageBoxManager = messageBoxManager;

        StartExecutionCommand = ReactiveCommand.CreateFromTask(RunAsync);
        PauseExecutionCommand = ReactiveCommand.Create(PauseAsync);
        MakeStepCommand = ReactiveCommand.CreateFromTask(MakeStepAsync);
        ResetExecutorCommand = ReactiveCommand.CreateFromTask(ResetExecutorAsync);
        ChangeMemoryModeCommand = ReactiveCommand.Create(ChangeMemoryMode);
        FindAddressCommand = ReactiveCommand.CreateFromTask<string>(FindAddressAsync);

        Tabs = Enum.GetValues<Tab>().ToObservableCollection();
        Memory = AsWords().ToObservableCollection();

        CodeLines = _executor.Commands.Select(m =>
        {
            var codeLine = CodeLine.FromDto(m);
            codeLine.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != nameof(CodeLine.Breakpoint))
                {
                    return;
                }

                var line = s as CodeLine;
                if (line!.Breakpoint)
                {
                    _executor.AddBreakpoint(line.Address);
                }
                else
                {
                    _executor.RemoveBreakpoint(line.Address);
                }
            };
            return codeLine;
        }).ToObservableCollection();
        SelectedLine = CodeLines.FirstOrDefault();

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
    public ObservableCollection<RegisterModel> Registers =>
        _executor.Registers.Select((m, i) => new RegisterModel(i, m)).ToObservableCollection();

    /// <inheritdoc />
    public ObservableCollection<ProcessorStateWordModel> ProcessorStateWord =>
        new[] { new ProcessorStateWordModel(_executor.ProcessorStateWord) }.ToObservableCollection();

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

    /// <inheritdoc />
    public ObservableCollection<Device> Devices => _executor.Devices.ToObservableCollection();

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

    private async Task Runner(Func<Task<bool>> runFunction)
    {
        try
        {
            var res = await runFunction();

            if (!res)
            {
                await _messageBoxManager.ShowMessageBoxAsync("Executor", "End of program is reached", ButtonEnum.Ok,
                    Icon.Info, View);
            }
        }
        catch (HaltException e)
        {
            await _messageBoxManager.ShowMessageBoxAsync("Executor", $"Program halted with error:\n{e.Message}",
                ButtonEnum.Ok, Icon.Info, View);
        }
        catch (Exception e)
        {
            await _messageBoxManager.ShowErrorMessageBox(e.Message, View);
        }
    }

    private async Task MakeStepAsync()
    {
        await Runner(() => _executor.ExecuteNextInstructionAsync());
        UpdateState();
    }

    private async Task RunAsync()
    {
        _cancelRunToken = new CancellationTokenSource();

        await Runner(() => _executor.ExecuteAsync(_cancelRunToken.Token));

        _cancelRunToken.Dispose();
        _cancelRunToken = null;

        UpdateState();
    }

    private void PauseAsync() => _cancelRunToken?.Cancel();

    private async Task ResetExecutorAsync()
    {
        await _executor.LoadProgram();
        UpdateState();
    }

    private void ChangeMemoryMode()
    {
        _memoryAsWord = !_memoryAsWord;
        this.RaisePropertyChanged(nameof(ChangeMemoryModeCommandHeader));
        Memory = _memoryAsWord ? AsWords().ToObservableCollection() : AsBytes().ToObservableCollection();
    }

    private IEnumerable<IMemoryModel> AsWords()
    {
        var count = _executor.Memory.Data.Count;
        for (ushort i = 0; i < count; i += 2)
        {
            yield return new WordModel(i, _executor.Memory.GetWord(i));
        }
    }

    private IEnumerable<IMemoryModel> AsBytes() => _executor.Memory.Data.Select((m, i) => new ByteModel((ushort)i, m));

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

    private void UpdateLines()
    {
        foreach (var codeLine in CodeLines)
        {
            codeLine.Code = _executor.Memory.GetWord(codeLine.Address);
        }
    }

    private void UpdateState()
    {
        Memory = (_memoryAsWord ? AsWords() : AsBytes()).ToObservableCollection();
        UpdateLines();
        this.RaisePropertyChanged(nameof(Registers));
        this.RaisePropertyChanged(nameof(ProcessorStateWord));
        this.RaisePropertyChanged(nameof(Devices));
        SelectedLine = CodeLines.SingleOrDefault(m => m.Address == _executor.Registers.ElementAt(7));
    }
}