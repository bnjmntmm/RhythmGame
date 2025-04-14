using Godot;
using System;
using System.Collections.Generic;


//using System;
//using System.Collections.Generic;

public partial class DjRootTranslated : Node3D
{


    //[Export]
    //public Dictionary<string, Color> colors = new()
    //{
    //    { "Note_Q", Colors.Red },
    //    { "Note_W", Colors.Blue },
    //    { "Note_E", Colors.Green },
    //    { "Note_R", Colors.Yellow }
    //};

    public List<int> noteIndexes = [];
    public Timer BeatTimer => _timer;
    public int CurrentTactBeat => ((_totalBeats - 1) % 4) + 1;
    public int NextTactBeat => (CurrentTactBeat % 4) + 1;
    public bool IsPlaying => _isPlaying;
    public bool IsLongNotePlaying => _longNotePlaying;

    public event Action<bool> PlayingTurnChanged;
    public event Action StoppedPlaying;

    private GameSceneTranslated _gameScene;
    private Node3D _lightsNode;
    private PackedScene _noteScene;
    private AudioStreamPlayer _melodyAudioPlayer;
    private AudioStreamPlayer _beatAudioPlayer;
    private Timer _timer;
    private int _melodyCooldown = 4;
    private int _totalBeats = 0;
    private bool _isPlaying = false;
    private bool _longNotePlaying = false;

    public override void _Ready()
    {
        //_noteScene = GD.Load<PackedScene>("res://scenes/Note/note.tscn");

        //foreach(var inputString in colors.Keys)
        //{
        //    Node noteInstance = _noteScene.Instantiate();
        //    var note = noteInstance;
        //    if(note is not null)
        //    {
        //        var noteInput = note as NoteInput_Translated;
        //        if(noteInput != null)
        //        {
        //            noteInput.InputName = inputString;
        //            note.Name = inputString;
        //            noteInput.Sound = sounds[inputString];
        //            noteInput.IsDJ = true;
        //        }
        //        AddChild(note);
        //    }
        //}

        _timer = GetNode<Timer>("Beat");
        _timer.Timeout += OnTimeout;
        _timer.Start();

        _gameScene = GetParent<GameSceneTranslated>();
        _beatAudioPlayer = GetNode<AudioStreamPlayer>("BeatSound");
        _melodyAudioPlayer = GetNode<AudioStreamPlayer>("MelodySound");
    }

    private void OnTimeout()
    {
        if(_melodyCooldown == -4)
        {
            PlayingTurnChanged?.Invoke(_isPlaying);
            _melodyCooldown = 4;
        }

        _beatAudioPlayer.Stop();
        _beatAudioPlayer.Play();
        _totalBeats++;

        if(_melodyCooldown <= 0)
        {
            if(_isPlaying == false)
            {
                _isPlaying = true;
                PlayingTurnChanged?.Invoke(_isPlaying);
                noteIndexes.Clear();
            }

            if(_longNotePlaying == false)
            {
                int index = GD.RandRange(0, _gameScene.Notes.Length - 1);
                _melodyAudioPlayer.Stream = _gameScene.Notes[index].sound;
                _melodyAudioPlayer.Play();
                _gameScene.Lights.ChangeLightToColor(_gameScene.Notes[index].color);
                noteIndexes.Add(index);
                GD.Print(index + 1);

                if(GD.RandRange(1, 4) == 4 && noteIndexes.Count != 4)
                {
                    _longNotePlaying = true;
                }
            }
            else
            {
                // -1 indicates that the note is played long
                noteIndexes.Add(-1);
                GD.Print(-1);
                _longNotePlaying = false;
            }

        }

        _melodyCooldown -= 1;

        if(_melodyCooldown == -4)
        {
            _isPlaying = false;
            StoppedPlaying?.Invoke();
        }
    }

    //public async Task<Array> AsyncPlaySongSequence(Array sequence)
    //{
    //    foreach(Variant noteDataVar in sequence)
    //    {
    //        // Unpack Dictionary for current note
    //        Dictionary noteData = noteDataVar.As<Dictionary>();
    //        string noteName = noteData["note"].AsString();
    //        float duration = (float)noteData["duration"].AsDouble();

    //        if(sounds.ContainsKey(noteName))
    //        {
    //            var noteNode = GetNode<NoteInput_Translated>(noteName);
    //            GD.Print($"Playing {noteName} for {duration} seconds");

    //            if(colors.ContainsKey(noteName))
    //                _lightsNode.Call("change_lights_to_color", colors[noteName]);

    //            noteNode.DjPlayedSound(duration);
    //        }
    //    }

    //    _lightsNode.Call("reset_color");
    //    return sequence;
    //}

    public override void _Process(double delta)
    {
        if(_timer.WaitTime != 60.0 / _gameScene.BeatsPerMinute)
        {
            _timer.WaitTime = 60.0 / _gameScene.BeatsPerMinute;
            GD.Print(_timer.WaitTime + ", " + _gameScene.BeatsPerMinute);
        }
    }
}
