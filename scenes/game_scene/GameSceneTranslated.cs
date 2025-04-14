using Godot;
using System;
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
    private Note[] _notes;
    private DjRootTranslated _djNode;
    private PlayerTranslated _playerNode;
    private AudioStreamPlayer _playerSounds;
    private int _noteToHitIndex = 1;
    private int _correctNoteHits = 0;
    private bool _playedWrongNote = true;


    public double BeatsPerMinute => _beatsPerMinute;
    public AudioStream[] Sounds => _sounds;

    [Signal] public delegate void GameOverEventHandler();
    [Signal] public delegate void PlayerAtDJEventHandler();


    [ExportCategory("PlayerStageControl")]
    private Node3D _stages;
    private int stageCount;
    private int currentStage = 0;

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
        _stages = GetNode<Node3D>("World/Stages");
        stageCount = _stages.GetChildCount();
        currentStage = _playerNode.currentStage;
        var noteKeys = InputMap.GetActions().Where((a) => a.ToString().StartsWith("Note")).ToArray();
        _notes = new Note[_sounds.Length];

        for(int i = 0; i < _sounds.Length; i++)
        {
            AudioStream sound = _sounds[i];
            _notes[i] = new(noteKeys[i], _noteColors[i], sound, i);
        }

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
                    _correctNoteHits++;
                }
                else
                {
                    GD.Print("Perfect: Held Correct Note");
                    _correctNoteHits++;
                }
            }
            _noteToHitIndex++;
        }
    }

    private void _djNode_OnStoppedPlaying()
    {
        //dj just stopped playing (but tact is not quite over), this is just to allow the user to begin playing right after the last note from the dj
        _noteToHitIndex = 1;
        _correctNoteHits = 0;
    }

    //new tact
    private void _djNode_OnPlayingTurnChanged(bool isDjPlaying)
    {
        //player did not play 4 notes 
        if(isDjPlaying && _correctNoteHits != 4 && _playedWrongNote == false)
        {
            MoveToPreviousStage();           
        }
        _playedWrongNote = false;
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
            _correctNoteHits++;
            if(_correctNoteHits == 4){
                MoveToNextStage();
            }
            //GD.Print("Played Correct Note");
            // here invoke the signal to play the note
        }
        else if(_djNode.noteIndexes[_noteToHitIndex - 1] == -1)
        {
            GD.Print("Note was supposed to be held");
            MoveToPreviousStage();
            _playedWrongNote = true;
        }
        else
        {
            GD.Print("Played Wrong Note");
            MoveToPreviousStage();
            _playedWrongNote = true;
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

    public void MoveToNextStage(){
        if (currentStage < stageCount - 1){
            currentStage++;
            _playerNode.currentStage = currentStage;

            // move to postion, currentStage +1 will current_stage + 1 would be the note via the index?
            var stage = _stages.GetChild(currentStage) as Node3D;
            GD.Print(stage.Name);
            var targetPosition = stage.GlobalPosition;
            _playerNode.targetPosition = targetPosition;
            _playerNode.shouldMove = true;
            if (currentStage == stageCount - 1){
                GD.Print("REACHED THE DJ");
                EmitSignal(SignalName.PlayerAtDJ);
                //GameOver();
            }
        }else {
            GD.Print("No more stages to move to. YOU AT THE DJ");
            EmitSignal(SignalName.PlayerAtDJ);
        }
    
    }
    public void MoveToPreviousStage(){
        if (currentStage > 0){
            currentStage--;
            _playerNode.currentStage = currentStage;

            // move to postion, currentStage +1 will current_stage + 1 would be the note via the index?
            var stage = _stages.GetChild(currentStage) as Node3D;
            GD.Print("moving back to stage: " +stage.Name);
            var targetPosition = stage.GlobalPosition;
            _playerNode.targetPosition = targetPosition;
            _playerNode.shouldMove = true;
            if (currentStage == 0){
                GD.Print("REACHED THE Entrance,  LAST CHANCE");
                //GameOver();
            }
        }else {
            GD.Print("No more stages to move to. YOU AT THE ENTRANCE");
            _djNode.BeatTimer.Stop();
            EmitSignal(SignalName.GameOver);
            
        }
    
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
