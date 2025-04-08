extends Node
class_name NoteInput

signal released_note(note_name, duration)


@export var inputName = ""
var _time_when_pushed_down = 0
var duration_pressed

func _physics_process(delta: float) -> void:
	if Input.is_action_just_pressed(inputName):
		_time_when_pushed_down = Time.get_ticks_msec()
		print("Pressed ", inputName)
	if Input.is_action_just_released(inputName):
		var duration = (Time.get_ticks_msec()- _time_when_pushed_down) / 1000.0
		released_note.emit(inputName, duration)
