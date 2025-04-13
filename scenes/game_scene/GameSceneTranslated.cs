using Godot;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public partial class GameSceneTranslated : Node
{
    //[Export] private Label UserShouldStartLabel;
    //[Export] private Control GameOverScreen;
    [Export, Range(1, 240)]
    private int _beatsPerMinute = 60;
    [Export]
    private AudioStream[] _sounds;
    [Export]
    private Color[] _noteColors = [];


    [Export] private double _perfectTimingThreshold = 0.05;
    [Export] private double _greatTimingThreshold = 0.15;
    [Export] private double _goodTimingThreshold = 0.25;

    //private Control fadeOverlay;
    //private Control pauseOverlay;
    private Note[] _notes;
    private DjRootTranslated _djNode;
    private PlayerTranslated _playerNode;
    private AudioStreamPlayer _playerSounds;
    private int _noteToHitIndex = 1;
    //private Songs songNode;
    //private Node inputController;

    //private Array songKeys;
    //private Array playedSongByDJ;

    public double BeatsPerMinute => _beatsPerMinute;
    public AudioStream[] Sounds => _sounds;

    public override void _Ready()
    {
        //fadeOverlay = GetNode<Control>("%FadeOverlay");
        //pauseOverlay = GetNode<Control>("%PauseOverlay");
        _djNode = GetNode<Node3D>("DJ_C#") as DjRootTranslated;
        _djNode.PlayingTurnChanged += _djNode_OnPlayingTurnChanged;
        _djNode.StoppedPlaying += _djNode_OnStoppedPlaying; ;
        _playerNode = GetNode<Node3D>("Player_C#") as PlayerTranslated;
        _playerNode.KeyPressed += _playerNode_OnKeyPressed;
        _playerNode.KeyReleased += _playerNode_OnKeyReleased; ;
        _playerSounds = GetNode<AudioStreamPlayer>("PlayerSounds") as AudioStreamPlayer;
        var noteKeys = InputMap.GetActions().Where((a) => a.ToString().StartsWith("Note")).ToArray();
        _notes = new Note[_sounds.Length];

        for(int i = 0; i < _sounds.Length; i++)
        {
            AudioStream sound = _sounds[i];
            _notes[i] = new(noteKeys[i], _noteColors[i], sound, i);
        }

        //songNode = GetNode<Songs>("Songs");
        //inputController = GetNode<Node>("InputController");

        //UserShouldStartLabel = GetNode<Label>("UILAYER/UserShouldStartLabel");
        //GameOverScreen = GetNode<Control>("UILAYER/GameOverScreen");

        // Connect custom signal
        //inputController.Connect("player_thrown_out", new Callable(this, nameof(GameOver)));

        // Only for transition and pause
        //fadeOverlay.Visible = true;

        //songKeys = songNode.songs.Keys;

        // Wenn du die DJ-Sequenz abspielen willst:
        // playedSongByDJ = await djNode.AsyncPlaySongSequence(songNode.Songs[songKeys[0]]);
        // UserShouldStartLabel.Show();
    }

    private void _playerNode_OnKeyReleased(string keyActionName, double keyHoldTime)
    {
        if(_djNode.IsPlaying || _noteToHitIndex > 4 || _djNode.noteIndexes.Count == 0)
        {
            return;
        }

        // check if current note is played long
        if(_djNode.noteIndexes[_noteToHitIndex - 1] == -1)
        {
            if(_djNode.noteIndexes[_noteToHitIndex - 2] == _notes.First((n) => n.actionName == keyActionName).index)
            {
                if(keyHoldTime < 30.0 / _beatsPerMinute)
                {
                    GD.Print("Did not hold key");
                }
                else if(keyHoldTime < 90.0 / _beatsPerMinute)
                {
                    GD.Print("Held Correct Note but for too short");
                }
                else
                {
                    GD.Print("Perfect: Held Correct Note");
                }
            }
            _noteToHitIndex++;
        }
    }

    private void _djNode_OnStoppedPlaying()
    {
        //dj just stopped playing (but tact is not quite over), this is just to allow the user to begin playing right after the last note from the dj
        _noteToHitIndex = 1;
    }

    private void _djNode_OnPlayingTurnChanged(bool isDjPlaying)
    {
        //new tact
    }

    private void _playerNode_OnKeyPressed(string keyActionName)
    {
        // no input when dj is playing (except after last note)
        if(_djNode.IsPlaying && _djNode.NextTactBeat != 1)
        {
            GD.Print("not your time to shine yet champ");
            return;
        }

        _playerSounds.Stream = _notes.First((n) => n.actionName == keyActionName).sound;
        _playerSounds.Play();

        if(_noteToHitIndex > 4)
        {
            GD.Print("Already played 4 note this tact");
            return;
        }

        if(_djNode.noteIndexes.Count == 0) return;

        if(_djNode.noteIndexes[_noteToHitIndex - 1] == _notes.First((n) => n.actionName == keyActionName).index)
        {
            //GD.Print("Played Correct Note");
        }
        else if(_djNode.noteIndexes[_noteToHitIndex - 1] == -1)
        {
            GD.Print("Note was supposed to be held");
        }
        else
        {
            GD.Print("Played Wrong Note");
            return;
        }

        // key was pressed too early
        if(_djNode.NextTactBeat == _noteToHitIndex)
        {
            if(_djNode.BeatTimer.TimeLeft <= _perfectTimingThreshold)
            {
                GD.Print("perfect: " + _djNode.BeatTimer.TimeLeft);
            }
            else if(_djNode.BeatTimer.TimeLeft <= _greatTimingThreshold)
            {
                GD.Print("great but slightly too early: " + _djNode.BeatTimer.TimeLeft);
            }
            else if(_djNode.BeatTimer.TimeLeft <= _goodTimingThreshold)
            {
                GD.Print("good but too early: " + _djNode.BeatTimer.TimeLeft);
            }
        }
        // key was pressed too late
        else if(_djNode.NextTactBeat == _noteToHitIndex + 1 || _djNode.NextTactBeat == 1 && _djNode.IsPlaying == false)
        {
            if(_djNode.BeatTimer.WaitTime - _djNode.BeatTimer.TimeLeft <= _perfectTimingThreshold)
            {
                GD.Print("perfect: " + (_djNode.BeatTimer.WaitTime - _djNode.BeatTimer.TimeLeft));
            }
            else if(_djNode.BeatTimer.WaitTime - _djNode.BeatTimer.TimeLeft <= _greatTimingThreshold)
            {
                GD.Print("great but slightly too late: " + (_djNode.BeatTimer.WaitTime - _djNode.BeatTimer.TimeLeft));
            }
            else if(_djNode.BeatTimer.WaitTime - _djNode.BeatTimer.TimeLeft <= _goodTimingThreshold)
            {
                GD.Print("good but too late: " + (_djNode.BeatTimer.WaitTime - _djNode.BeatTimer.TimeLeft));
            }
            else
            {
                GD.Print("too late: " + (_djNode.BeatTimer.WaitTime - _djNode.BeatTimer.TimeLeft));
            }
        }
        // key was pressed waaay too early
        else if(_djNode.NextTactBeat < _noteToHitIndex)
        {
            GD.Print("waaay too early: " + ((_djNode.BeatTimer.WaitTime * (_noteToHitIndex - _djNode.NextTactBeat)) + _djNode.BeatTimer.TimeLeft));
        }
        // key was pressed waaay too late
        else
        {
            GD.Print("waaay too late: " + ((_djNode.BeatTimer.WaitTime * (_djNode.NextTactBeat - 1 - _noteToHitIndex)) + _djNode.BeatTimer.TimeLeft));
        }

        _noteToHitIndex++;
    }

    public override void _Process(double delta)
    {
    }

    //public override void _Input(InputEvent @event)
    //{
    //    if(@event.IsActionPressed("pause") && !pauseOverlay.Visible)
    //    {
    //        GetViewport().SetInputAsHandled();
    //        GetTree().Paused = true;
    //        pauseOverlay.GrabFocus();
    //        pauseOverlay.Visible = true;
    //    }
    //}

    //    private void GameOver()
    //    {
    //        // Trigger FadeOut auf dem Overlay (falls es eine Animation hat)
    //        //var fadeOverlayScript = fadeOverlay as FadeOverlay;
    //        //fadeOverlayScript?.FadeOut();
    //    }

    //    private void OnFadeOverlayOnCompleteFadeOut()
    //    {
    //        GameOverScreen.Show();
    //    }
}

//public partial class Songs : Node
//{
//    public static Dictionary<string, string> songs = null;
//}
