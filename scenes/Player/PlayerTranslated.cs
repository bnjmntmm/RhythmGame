using Godot;
using Godot.Collections;
using System;

public partial class PlayerTranslated : Node3D
{
    private const string NOTE_Q = "Note_Q";
    private const string NOTE_W = "Note_W";
    private const string NOTE_E = "Note_E";
    private const string NOTE_R = "Note_R";
    
    private AnimationPlayer _animationPlayer;

    public event Action<string> KeyPressed;
    public event Action<string, double> KeyReleased;

    private Dictionary<string, double> _keyHoldTimers = [];
    
    private Sprite3D _mainSprite;

    [Export] public Texture2D defaultTexture;
    [Export] public Texture2D happyTexture;
    [Export] public Texture2D angryTexture;

    public int currentStage = 1;
    public AnimationPlayer AnimationPlayer => _animationPlayer;
    public Sprite3D MainSprite => _mainSprite;

    public bool shouldMove = false;
    public Vector3 targetPosition;
    public float speed = 5f;
    

    public override void _Ready()
    {
        _mainSprite = GetNode<Sprite3D>("Sprite3D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.Play("Default");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (shouldMove){
            MoveTowardsPosition(targetPosition, delta, speed);
        }
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
    public void MoveTowardsPosition(Vector3 toPosition, double delta, float speed)
    {

        var currentPos = GlobalPosition;
        var direction = toPosition - currentPos;
        var distanceToTarget = direction.Length();

        if (distanceToTarget <= speed * delta){
            GlobalPosition = toPosition;
            shouldMove = false;

        } else {
            var moveVector = direction.Normalized() * (float)(speed * delta);
            var newPosition = currentPos + moveVector;
            GlobalPosition = newPosition;
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
