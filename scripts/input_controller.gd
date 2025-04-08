extends Node

const NOTE = preload("res://scenes/Note/note.tscn")
@export var short_press_duration : float = 0.12
@export var input_strings : Array[String]
@export var sounds : Dictionary[String, AudioStream]

@export var world_light : DirectionalLight3D

var colors : Dictionary = {
	"Note_Q" : Color.RED,
	"Note_W" : Color.BLUE,
	"Note_E" : Color.GREEN,
	"Note_R" : Color.YELLOW
}


@onready var lightsNode: Node3D = $"../World/Lights"


func _ready() -> void:
	for input_string in input_strings:
		var new_note = NOTE.instantiate()
		new_note.inputName = input_string
		new_note.sound = sounds[input_string]
		new_note.released_note.connect(released_note)
		new_note.pressed_note.connect(pressed_note)
		add_child(new_note)


##when pressing, lights should change
func pressed_note(note_name : String):
	if colors.has(note_name):
		#change_world_light(colors[note_name])
		lightsNode.change_lights_to_color(colors[note_name]) ##changes the spotlights

## light should turn off again?
func released_note(note_name, duration):
	if duration < short_press_duration:
		print(note_name," pressed only short")
	else:
		print(note_name, " pressed long with : ", duration)
	lightsNode.reset_color()
	#reset_world_light()
	
	
func change_world_light(to_color : Color):
	world_light.light_color = to_color
	pass

func reset_world_light():
	world_light.light_color = Color.BLACK
