using Godot;
using Godot.Collections;
using System;

public partial class PlayerTranslated : Node3D
{
    private const string NOTE_Q = "Note_Q";
    private const string NOTE_W = "Note_W";
    private const string NOTE_E = "Note_E";
    private const string NOTE_R = "Note_R";

    public event Action<string> KeyPressed;
    public event Action<string, double> KeyReleased;

    private Dictionary<string, double> _keyHoldTimers = [];

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed(NOTE_Q))
        {
            KeyPressed?.Invoke(NOTE_Q);
        }
        else if(Input.IsActionJustPressed(NOTE_W))
        {
            KeyPressed?.Invoke(NOTE_W);
        }
        else if(Input.IsActionJustPressed(NOTE_E))
        {
            KeyPressed?.Invoke(NOTE_E);
        }
        else if(Input.IsActionJustPressed(NOTE_R))
        {
            KeyPressed?.Invoke(NOTE_R);
        }

        if(Input.IsActionPressed(NOTE_Q))
        {
            if(_keyHoldTimers.ContainsKey(NOTE_Q))
            {
                _keyHoldTimers[NOTE_Q] += delta;
            }
            else
            {
                _keyHoldTimers.Add(NOTE_Q, 0);
            }
        }
        else if(Input.IsActionPressed(NOTE_W))
        {
            if(_keyHoldTimers.ContainsKey(NOTE_W))
            {
                _keyHoldTimers[NOTE_W] += delta;
            }
            else
            {
                _keyHoldTimers.Add(NOTE_W, 0);
            }
        }
        else if(Input.IsActionPressed(NOTE_E))
        {
            if(_keyHoldTimers.ContainsKey(NOTE_E))
            {
                _keyHoldTimers[NOTE_E] += delta;
            }
            else
            {
                _keyHoldTimers.Add(NOTE_E, 0);
            }
        }
        else if(Input.IsActionPressed(NOTE_R))
        {
            if(_keyHoldTimers.ContainsKey(NOTE_R))
            {
                _keyHoldTimers[NOTE_R] += delta;
            }
            else
            {
                _keyHoldTimers.Add(NOTE_R, 0);
            }
        }

        if(Input.IsActionJustReleased(NOTE_Q))
        {
            if(_keyHoldTimers.ContainsKey(NOTE_Q))
            {
                KeyReleased?.Invoke(NOTE_Q, _keyHoldTimers[NOTE_Q]);
                _keyHoldTimers[NOTE_Q] = 0;
            }
        }
        else if(Input.IsActionJustReleased(NOTE_W))
        {
            if(_keyHoldTimers.ContainsKey(NOTE_W))
            {
                KeyReleased?.Invoke(NOTE_W, _keyHoldTimers[NOTE_W]);
                _keyHoldTimers[NOTE_W] = 0;
            }
        }
        else if(Input.IsActionJustReleased(NOTE_E))
        {
            if(_keyHoldTimers.ContainsKey(NOTE_E))
            {
                KeyReleased?.Invoke(NOTE_E, _keyHoldTimers[NOTE_E]);
                _keyHoldTimers[NOTE_E] = 0;
            }
        }
        else if(Input.IsActionJustReleased(NOTE_R))
        {
            if(_keyHoldTimers.ContainsKey(NOTE_R))
            {
                KeyReleased?.Invoke(NOTE_R, _keyHoldTimers[NOTE_R]);
                _keyHoldTimers[NOTE_R] = 0;
            }
        }
    }
}

public partial class Note(string actionName, Color color, AudioStream sound, int index)
{
    public string actionName = actionName;
    public Color color = color;
    public AudioStream sound = sound;
    public int index = index;
}
