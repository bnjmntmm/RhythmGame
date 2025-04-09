extends Node
class_name NoteInput

signal released_note(note_name, duration)
signal pressed_note(note_name)

@export var inputName = ""
var _time_when_pushed_down = 0
var duration_pressed
var is_pressed : bool = false
var sound : AudioStream

var is_DJ : bool = false

func _ready() -> void:
	%AudioStreamPlayer.stream = sound


func _process(delta: float) -> void:
	if not is_DJ:
		if Input.is_action_just_pressed(inputName):
			%AudioStreamPlayer.play()
			is_pressed = true
			_time_when_pushed_down = Time.get_ticks_msec()
			
			print("Pressed ", inputName)
			pressed_note.emit(inputName)
		if Input.is_action_just_released(inputName):
			is_pressed = false
			%AudioStreamPlayer.stop()
			var duration = (Time.get_ticks_msec()- _time_when_pushed_down) / 1000.0
			released_note.emit(inputName, duration)


## DJ INPUT
func dj_played_sound(duration):
	%AudioStreamPlayer.play()
	await get_tree().create_timer(duration).timeout
	%AudioStreamPlayer.stop()
