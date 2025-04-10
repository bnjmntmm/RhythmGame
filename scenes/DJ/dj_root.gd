extends Node3D

const NOTE = preload("res://scenes/Note/note.tscn")

@onready var lights_node: Node3D = $"../Lights"

@export var sounds : Dictionary[String, AudioStream]
@export var colors : Dictionary = {
	"Note_Q" : Color.RED,
	"Note_W" : Color.BLUE,
	"Note_E" : Color.GREEN,
	"Note_R" : Color.YELLOW
}



func _ready() -> void:
	for input_string in colors:
		var new_note = NOTE.instantiate()
		new_note.inputName = input_string
		new_note.name = input_string
		new_note.sound = sounds[input_string]
		new_note.is_DJ = true
		add_child(new_note)

func async_play_song_sequence(sequence: Array) -> Array:
	for note_data in sequence:
		var note_name : String = note_data.note
		var duration : float = note_data.duration
		
		## check fi we have the sound for the note
		if sounds.has(note_name):
			var note_node = get_node(note_name)
			print("Playing, ", note_name, " for : ", duration, " seconds")
			lights_node.change_lights_to_color(colors[note_name])
			await note_node.dj_played_sound(duration) ## timeout is in the noteInput.gd
	lights_node.reset_color()
	return sequence
