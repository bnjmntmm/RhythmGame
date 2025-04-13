using Godot;

public partial class InputController_Translated : Node
{
    [Signal] public delegate void PlayerAtDjEventHandler();
    [Signal] public delegate void PlayerThrownOutEventHandler();

    [Export] public float ShortPressDuration = 0.12f;
    [Export] public Godot.Collections.Array<string> InputStrings;
    [Export] public Godot.Collections.Dictionary<string, AudioStream> Sounds;

    [Export] public DirectionalLight3D WorldLight;

    private Node3D lightsNode;
    private Node3D player;
    private Node3D stages;

    private int currentStage;

    private readonly Godot.Collections.Dictionary<string, Color> colors = new()
    {
        { "Note_Q", Colors.Red },
        { "Note_W", Colors.Blue },
        { "Note_E", Colors.Green },
        { "Note_R", Colors.Yellow }
    };

    public override void _Ready()
    {
        lightsNode = GetNode<Node3D>("../World/Lights");
        player = GetNode<Node3D>("../World/Player");
        stages = GetNode<Node3D>("../World/Stages");

        currentStage = (int)player.Get("current_stage");

        foreach(string inputString in InputStrings)
        {
            var newNote = GD.Load<PackedScene>("res://scenes/Note/note.tscn").Instantiate();
            newNote.Set("inputName", inputString);
            newNote.Set("sound", Sounds[inputString]);

            newNote.Connect("pressed_note", new Callable(this, nameof(OnPressedNote)));
            newNote.Connect("released_note", new Callable(this, nameof(OnReleasedNote)));

            AddChild(newNote);
        }
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("move_forwards_DEBUG"))
            MoveToNextStage();

        if(Input.IsActionJustPressed("move_back_DEBUG"))
            MoveToPriorStage();
    }

    private void OnPressedNote(string noteName)
    {
        if(colors.ContainsKey(noteName))
        {
            lightsNode.Call("change_lights_to_color", colors[noteName]);
        }
    }

    private void OnReleasedNote(string noteName, float duration)
    {
        if(duration < ShortPressDuration)
            GD.Print($"{noteName} pressed only short");
        else
            GD.Print($"{noteName} pressed long with : {duration}");

        lightsNode.Call("reset_color");
    }

    private void MoveToNextStage()
    {
        GD.Print("called next stage");

        if(currentStage < 4)
        {
            currentStage++;
            player.Set("current_stage", currentStage);

            var stage = stages.GetChild<Node3D>(currentStage + 1);
            player.Set("target_position", stage.GlobalPosition);
            player.Set("should_move", true);

            if(currentStage == 4)
                GD.Print("YOU REACHED THE DJ");
        }
        else
        {
            GD.Print("YOU AT THE DJ");
            EmitSignal(SignalName.PlayerAtDj);
        }
    }

    private void MoveToPriorStage()
    {
        GD.Print("called prior stage");

        if(currentStage > -1)
        {
            currentStage--;
            player.Set("current_stage", currentStage);

            var stage = stages.GetChild<Node3D>(currentStage + 1);
            player.Set("target_position", stage.GlobalPosition);
            player.Set("should_move", true);

            if(currentStage == -1)
                GD.Print("YOU REACHED THE ENTRANCE");
        }
        else
        {
            GD.Print("YOU GOT KICKED OUT");
            EmitSignal(SignalName.PlayerThrownOut);
        }
    }

    private void ChangeWorldLight(Color toColor)
    {
        WorldLight.LightColor = toColor;
    }

    private void ResetWorldLight()
    {
        WorldLight.LightColor = Colors.Black;
    }
}
