using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using GUI.Views;
using ReactiveUI;

namespace GUI.ViewModels;

public class RegisterModel
{
    public string Register { get; set; }

    public string Value { get; set; }

    public RegisterModel(string name, string value)
    {
        Register = name;
        Value = value;
    }
}

public class PswModel
{
    private readonly int _psw;

    public PswModel(int psw)
    {
        _psw = psw;
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Value of processor sate word
    /// </summary>
    private string PSW => $"0b{Convert.ToString(_psw, 2).PadLeft(16, '0')}";

    /// <summary>
    /// Priority
    /// </summary>
    public int Priority => (_psw & 0xE0) >> 5;

    /// <summary>
    /// Trace flag
    /// </summary>
    public int T => (_psw & 16) >> 4;

    /// <summary>
    /// Negative flag
    /// </summary>
    public int N => (_psw & 8) >> 4;

    /// <summary>
    /// Zero flag
    /// </summary>
    public int Z => (_psw & 4) >> 2;

    /// <summary>
    /// Overflow flag
    /// </summary>
    public int V => (_psw & 2) >> 1;

    /// <summary>
    /// Carry flag
    /// </summary>
    public int C => (_psw & 1) >> 0;
}

public interface IMemoryModel
{
    public string Address { get; }

    public string Value { get; }
}

public class WordModel : IMemoryModel
{
    private readonly ushort _address;

    private readonly ushort _value;

    public WordModel(ushort address, ushort value)
    {
        _address = address;
        _value = value;
    }

    public string Address => $"0o{Convert.ToString(_address, 8).PadLeft(6, '0')}";

    public string Value => $"0o{Convert.ToString(_value, 8).PadLeft(6, '0')}";
}

public class ByteModel : IMemoryModel
{
    private readonly ushort _address;

    private readonly byte _value;

    public ByteModel(ushort address, byte value)
    {
        _address = address;
        _value = value;
    }

    public string Address => $"0o{Convert.ToString(_address, 8).PadLeft(5, '0')}";

    public string Value => $"0o{Convert.ToString(_value, 8).PadLeft(3, '0')}";
}

public class SourceCodeLine
{
    public string Address { get; set; }

    public string Code { get; set; }

    public string Text { get; set; }

    public SourceCodeLine(string text, string code, string address)
    {
        Text = text;
        Address = address;
        Code = code;
    }
}

public class ExecutorViewModel : WindowViewModel<ExecutorWindow>, IExecutorViewModel
{
    /// <summary>
    /// Constructor for designer
    /// </summary>
    public ExecutorViewModel() : base(null)
    {
    }

    public ExecutorViewModel(ExecutorWindow view) : base(view)
    {
        Registers = new ObservableCollection<RegisterModel>(new RegisterModel[]
        {
            new("R0", "0o177520"),
            new("R1", "0o177520"),
            new("R2", "0o177520"),
            new("R3", "0o177520"),
            new("R4", "0o177520"),
            new("R5", "0o177520"),
            new("SP", "0o177520"),
            new("PC", "0o177520"),
        });

        PswTokens = new ObservableCollection<PswModel>(new[] { new PswModel(7) });

        ChangeVisibilityCommand = ReactiveCommand.Create(() =>
        {
            _memoryAsWords = !_memoryAsWords;
            this.RaisePropertyChanged(nameof(MemoryVisibilityState));
            this.RaisePropertyChanged(nameof(Memory));
        });

        GotoAddressCommand = ReactiveCommand.Create<string>(text =>
        {
            if (!int.TryParse(text, out var address))
            {
                return;
            }

            if (_memoryAsWords)
            {
                if (address % 2 == 1)
                {
                    return;
                }

                address /= 2;
            }

            View.MemoryGrid.SelectedIndex = address;
            View.MemoryGrid.ScrollIntoView(View.MemoryGrid.SelectedItem, View.MemoryGrid.Columns.FirstOrDefault());
        });

        SourceCode = new ObservableCollection<SourceCodeLine>(new SourceCodeLine[]
        {
            new("MOV #2, R0", "0o012701", "0o000000"),
            new("", "0o000002", "0o000002"),
            new("MOV #4, R5", "0o012705", "0o000004"),
            new("", "0o000004", "0o000006"),
            new("MOV #2, R0", "0o012701", "0o000010"),
            new("", "0o000002", "0o000012"),
            new("MOV #4, R5", "0o012705", "0o000014"),
            new("", "0o000004", "0o000016"),
        });

        var rnd = new Random();
        _memory = new byte[ushort.MaxValue + 1];
        rnd.NextBytes(_memory);

        InitContext();
    }

    public ObservableCollection<RegisterModel> Registers { get; }

    public ObservableCollection<PswModel> PswTokens { get; }

    private bool _memoryAsWords = true;
    public string MemoryVisibilityState => _memoryAsWords ? "As Bytes" : "As Words";

    private readonly byte[] _memory;

    public ReactiveCommand<Unit, Unit> ChangeVisibilityCommand { get; }

    public ReactiveCommand<string, Unit> GotoAddressCommand { get; }

    public ObservableCollection<IMemoryModel> Memory => new(_memoryAsWords ? GetWordMemory() : GetByteMemory());

    public ObservableCollection<SourceCodeLine> SourceCode { get; }

    private IEnumerable<IMemoryModel> GetByteMemory() => _memory.Select((t, i) => new ByteModel((ushort)i, t));

    private IEnumerable<IMemoryModel> GetWordMemory()
    {
        for (var i = 0; i < _memory.Length; i += 2)
        {
            var low = _memory[i];
            var high = _memory[i + 1];
            var word = (high << 8) | low;
            yield return new WordModel((ushort)i, (ushort)word);
        }
    }
}