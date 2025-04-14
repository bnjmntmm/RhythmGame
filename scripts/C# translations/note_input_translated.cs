using Godot;

public partial class NoteInput_Translated : Node
{
    [Signal]
    public delegate void ReleasedNoteEventHandler(string noteName, float duration);

    [Signal]
    public delegate void PressedNoteEventHandler(string noteName);

    [Export]
    public string InputName = "";

    private ulong _timeWhenPushedDown;
    private bool _isPressed = false;

    public AudioStream Sound;
    public bool IsDJ = false;

    private AudioStreamPlayer _player;

    public override void _Ready()
    {
        _player = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _player.Stream = Sound;
    }

    private void OnTimeout()
    {
        _player.Stop();
        _player.Play();
        GD.Print("Play");
    }

    public override void _Process(double delta)
    {
        if(IsDJ)
        {
            if(Input.IsActionJustPressed(InputName))
            {
                _player.Play();
                _isPressed = true;
                _timeWhenPushedDown = Time.GetTicksMsec();

                GD.Print("Pressed ", InputName);
                EmitSignal(SignalName.PressedNote, InputName);
            }

            if(Input.IsActionJustReleased(InputName))
            {
                _isPressed = false;
                _player.Stop();
                float duration = (Time.GetTicksMsec() - _timeWhenPushedDown) / 1000.0f;
                EmitSignal(SignalName.ReleasedNote, InputName, duration);
            }
        }
    }

    // DJ INPUT
    public async void DjPlayedSound(float duration)
    {
        _player.Play();
        await ToSignal(GetTree().CreateTimer(duration), SceneTreeTimer.SignalName.Timeout);
        _player.Stop();
    }
}
