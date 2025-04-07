extends Node2D
class_name NoteObject

@onready var label: Label = $Label
var allowed_to_move : bool = false
var speed = 200

func _physics_process(delta: float) -> void:
	if allowed_to_move:
		global_position.x -= speed * delta
