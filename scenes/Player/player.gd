extends Node3D

## has idle animation and a walk animation
## idle could be played when dj is doing stuff
## walk when going to next / previous stage?
@onready var animation_player: AnimationPlayer = $AnimationPlayer

## where the player currently is
## there are [-1, 0, 1, 2, 3, 4] and 4 is goal
var current_stage = 0

## for movement
var should_move : bool = false
var target_position : Vector3
var move_speed : float = 5.0

func _physics_process(delta: float) -> void:
	if should_move:
		move_towards_position(target_position, move_speed, delta)

func move_towards_position(to_pos : Vector3, speed: float, delta : float):
	var current_pos  := global_position
	var direction : Vector3 = to_pos - current_pos
	## calc remaining distance to target
	var distance_to_target = direction.length()
	
	## if close enough that the next move would overshoot, snap to target pos
	if distance_to_target <= speed * delta:
		global_position.x = to_pos.x
		should_move = false
	else:
		var move_vector : Vector3 = direction.normalized() * speed * delta
		var new_pos : Vector3 = current_pos + move_vector
		global_position.x = new_pos.x
