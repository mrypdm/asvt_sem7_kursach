namespace Executor.States;

public interface IState
{
    ushort ProcessorStateWord { get; set; }
    
    bool C { get; set; }
    
    bool V { get; set; }
    
    bool Z { get; set; }

    bool N { get; set; }
    
    bool T { get; }
    
    int Priority { get; set; }

    ushort[] Registers { get; }
}