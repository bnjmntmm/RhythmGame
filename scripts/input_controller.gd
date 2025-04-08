extends Node
@export var input_strings : Array[String]
@export var short_press_duration : float = 0.12

func _ready() -> void:
	for input_string in input_strings:
		var new_note = NoteInput.new()
		new_note.inputName = input_string
		new_note.released_note.connect(released_note)
		add_child(new_note)


func released_note(note_name, duration):
	if duration < short_press_duration:
		print(note_name," pressed only short")
	else:
		print(note_name, " pressed long with : ", duration)
	
