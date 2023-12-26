namespace GUI.Models.Executor;

/// <summary>
/// Model of PSW register
/// </summary>
public class ProcessorStateWordModel
{
    public ProcessorStateWordModel(ushort value)
    {
        Priority = (value & 0xE0) >> 5;
        T = (value & 16) >> 4;
        N = (value & 8) >> 3;
        Z = (value & 4) >> 2;
        V = (value & 2) >> 1;
        C = (value & 1) >> 0;
    }

    /// <summary>
    /// Priority
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Trace flag
    /// </summary>
    public int T { get; }

    /// <summary>
    /// Negative flag
    /// </summary>
    public int N { get; }

    /// <summary>
    /// Zero flag
    /// </summary>
    public int Z { get; }

    /// <summary>
    /// Overflow flag
    /// </summary>
    public int V { get; }

    /// <summary>
    /// Carry flag
    /// </summary>
    public int C { get; }
}