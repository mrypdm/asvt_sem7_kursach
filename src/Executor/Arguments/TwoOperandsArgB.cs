namespace Executor;

public class TwoOperandsArgB : IArgument
{
    private ushort _Mode;
    private ushort _Register;
    private State _state;
    private Memory _memory;
    public TwoOperandsArgB(ushort Mode, ushort Register, State state, Memory memory)
    {
        this._memory = memory;
        this._state = state;
        this._Register = Register;
        this._Mode = Mode;
    }

    public ushort GetValue()
    {
        ushort Byte;
        ushort addr;
        ushort offset;
        switch (_Mode)
        {
            case 0:
                return (byte)_state.R[_Register];
            case 1:
                return _memory.GetByte(_state.R[_Register]);
            case 2:
                Byte = _memory.GetByte(_state.R[_Register]);
                _state.R[_Register] += 1;
                return (byte)Byte;
            case 3:
                addr = _memory.GetByte(_state.R[_Register]);
                _state.R[_Register] += 1;
                return _memory.GetByte(addr);
            case 4:
                _state.R[_Register] -= 1;
                return _memory.GetByte(_state.R[_Register]);
            case 5:
                _state.R[_Register] -= 1;
                addr = _memory.GetByte(_state.R[_Register]);
                return _memory.GetByte(_state.R[addr]);
            case 6:
                _state.R[7] += 1;
                offset = _memory.GetByte(_state.R[7]);
                return _memory.GetByte((ushort)(_state.R[_Register] + offset));
            case 7:
                _state.R[7] += 1;
                offset = _memory.GetByte(_state.R[7]);
                addr = _memory.GetByte((ushort)(_state.R[_Register] + offset));
                return _memory.GetByte(_state.R[addr]);

        }
        throw new InvalidOperationException("Invalid Mode!");
    }
    public void SetValue(ushort word)
    {
        byte Byte = (byte)word;    
        ushort addr;
        ushort offset;
        switch (_Mode)
        {
            case 0:
                _state.R[_Register] = Byte;
                return;
            case 1:
                _memory.SetByte(_state.R[_Register], Byte);
                return;
            case 2:
                _memory.SetByte(_state.R[_Register], Byte);
                _state.R[_Register] += 1;
                return;
            case 3:
                addr = _memory.GetByte(_state.R[_Register]);
                _memory.SetByte(addr, Byte);
                _state.R[_Register] += 1;
                return;
            case 4:
                _state.R[_Register] -= 1;
                _memory.SetByte(_state.R[_Register], Byte);
                return;
            case 5:
                _state.R[_Register] -= 1;
                addr = _memory.GetByte(_state.R[_Register]);
                _memory.SetByte(_state.R[addr], Byte);
                return;
            case 6:
                _state.R[7] += 1;
                offset = _memory.GetByte(_state.R[7]);
                _memory.SetByte((ushort)(_state.R[_Register] + offset), Byte);
                return;
            case 7:
                _state.R[7] += 1;
                offset = _memory.GetByte(_state.R[7]);
                addr = _memory.GetByte((ushort)(_state.R[_Register] + offset));
                _memory.SetByte(_state.R[addr], Byte);
                return;

        }
        throw new InvalidOperationException("Invalid Mode!");
    }
}