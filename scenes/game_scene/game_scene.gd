extends Node
@onready var sequenceGenerator: Sequence = $Sequence
@onready var notes: Node2D = $UILAYER/UI/Notes
@onready var spawn_location: Marker2D = $UILAYER/UI/SpawnLocation

const NOTE_OBJECT = preload("res://scenes/NoteObject/note_object.tscn")


func _ready() -> void:
	sequenceGenerator.played_sound.connect(print_current_sound)
	var sequence = sequenceGenerator.generate_random_sequence(4)
	sequenceGenerator.play_sequence(sequence)
	

func print_current_sound(sound) -> void:
	var new_note = NOTE_OBJECT.instantiate()
	notes.add_child(new_note)
	new_note.global_position = spawn_location.global_position
	new_note.label.text = sound.name
	new_note.allowed_to_move = true
	
