extends Node

const NOTE = preload("res://scenes/Note/note.tscn")
@export var short_press_duration : float = 0.12
@export var input_strings : Array[String]
@export var sounds : Dictionary[String, AudioStream]

@export var world_light : DirectionalLight3D

@onready var lightsNode: Node3D = $"../World/Lights"

### PLAYER NODE
@onready var player: Node3D = $"../World/Player"
## STAGES TO GET THEIR POSITIONS
@onready var stages: Node3D = $"../World/Stages"



var colors : Dictionary = {
	"Note_Q" : Color.RED,
	"Note_W" : Color.BLUE,
	"Note_E" : Color.GREEN,
	"Note_R" : Color.YELLOW
}


var current_stage : int



func _ready() -> void:
	current_stage = player.current_stage
	for input_string in input_strings:
		var new_note = NOTE.instantiate()
		new_note.inputName = input_string
		new_note.sound = sounds[input_string]
		new_note.released_note.connect(released_note)
		new_note.pressed_note.connect(pressed_note)
		add_child(new_note)


func _process(delta: float) -> void:
	if Input.is_action_just_pressed("move_forwards_DEBUG"):
		move_to_next_stage()
	if Input.is_action_just_pressed("move_back_DEBUG"):
		move_to_prio_stage()

##when pressing, lights should change
func pressed_note(note_name : String):
	if colors.has(note_name):
		#change_world_light(colors[note_name])
		lightsNode.change_lights_to_color(colors[note_name]) ##changes the spotlights


func move_to_next_stage():
	print("called next stage")
	## current stage + 1 is the node that we are on ## I HOPE
	if current_stage < 4:
		## increase if not at fin
		current_stage += 1
		player.current_stage = current_stage
		
		## move to the position, current_stage + 1 would be the note via the index?
		var stage = stages.get_child(current_stage+1)
		player.target_position = stage.global_position
		player.should_move = true
		if current_stage == 4:
			print("YOU REACHED THE DJ")
	else:
		print("YOU AT THE DJ")
	
func move_to_prio_stage():
	print("called prior stage")
	if current_stage > -1:
		current_stage -= 1
		player.current_stage = current_stage
		
		
		## move to the position
		var stage = stages.get_child(current_stage +1)
		player.target_position = stage.global_position
		player.should_move = true
		if current_stage == -1:
			print("YOU REACHED THE ENTRANCE")
	else:
		print("YOU GOT KICKED OUT")
	pass

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
