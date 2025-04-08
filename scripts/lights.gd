extends Node3D

@export_color_no_alpha var base_color = Color.WHITE

var lights : Array

func _ready() -> void:
	lights = get_children()
	
	

func change_lights_to_color(to_color : Color):
	for light : SpotLight3D in lights:
		light.light_color = to_color

func reset_color():
	for light in lights:
		light.light_color = base_color
