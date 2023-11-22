namespace Executor;

public class TwoOperandsArgW : IArgument
{
    private ushort _Mode;
    private ushort _Register;
    private State _state;
    private Memory _memory;
    public TwoOperandsArgW(ushort Mode, ushort Register, State state, Memory memory)
    {
        this._memory = memory;
        this._state = state;
        this._Register = Register;
        this._Mode = Mode;
    }

    public ushort GetValue()
    {
        switch (_Mode)
        {
            case 0:
                return _state.R[_Register];
            case 1:
                return _memory.GetByte(_state.R[_Register]);
            case 2:
                ushort word = _memory.GetByte(_state.R[_Register]);
                _state.R[_Register] += 1;
                return word;
            case 3:
                ushort addr = _memory.GetByte(_state.R[_Register]);
                ushort word = _memory.GetByte(addr);
                _state.R[_Register] += 1;
                return word;
            case 4:
                _state.R[_Register] -= 1;
                return _memory.GetByte(_state.R[_Register]);
            case 5:
                _state.R[_Register] -= 1;
                ushort addr = _memory.GetByte(_state.R[_Register]);
                return _memory.GetByte(_state[addr]);
            case 6:
                _state.R[7] += 1;
                ushort offset = _memory.GetByte(_state.R[7]);
                return _memory.GetByte(_state.R[_Register] + offset);
            case 7:
                _state.R[7] += 1;
                ushort offset = _memory.GetByte(_state.R[7]);
                ushort addr = _memory.GetByte(_state.R[_Register] + offset);
                return _memory.GetByte(_state.R[addr]);

        }
    }
    public void SetValue(ushort word)
    {
        switch (_Mode)
        {
            case 0:
                _state.R[_Register] = word;
            case 1:
                _memory.SetByte(_state.R[_Register], word);
            case 2:
                _memory.SetByte(_state.R[_Register], word);
                _state.R[_Register] += 1;
            case 3:
                ushort addr = _memory.GetByte(_state.R[_Register]);
                _memory.SetByte(addr, word);
                _state.R[_Register] += 1;
            case 4:
                _state.R[_Register] -= 1;
                _memory.SetByte(_state.R[_Register], word);
            case 5:
                _state.R[_Register] -= 1;
                ushort addr = _memory.GetByte(_state.R[_Register]);
                _memory.SetByte(_state[addr], word);
            case 6:
                _state.R[7] += 1;
                ushort offset = _memory.GetByte(_state.R[7]);
                _memory.SetByte(_state.R[_Register] + offset, word);
            case 7:
                _state.R[7] += 1;
                ushort offset = _memory.GetByte(_state.R[7]);
                ushort addr = _memory.GetByte(_state.R[_Register] + offset);
                _memory.SetByte(_state.R[addr], word);

        }
    }
}