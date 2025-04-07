extends Node
@onready var sequenceGenerator: Sequence = $Sequence


func _ready() -> void:
	var sequence = sequenceGenerator.generate_random_sequence(4)
	sequenceGenerator.play_sequence(sequence)
	
