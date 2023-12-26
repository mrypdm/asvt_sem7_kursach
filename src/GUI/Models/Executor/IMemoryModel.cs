namespace GUI.Models.Executor;

/// <summary>
/// Model of memory cell
/// </summary>
public interface IMemoryModel
{
    /// <summary>
    /// Address of cell
    /// </summary>
    public string Address { get; }
    
    /// <summary>
    /// Value of cell
    /// </summary>
    public string Value { get; }
}