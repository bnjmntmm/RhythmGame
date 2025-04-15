extends Control

@export var menu_scene : String

func _on_back_to_menu_pressed() -> void:
	get_tree().change_scene_to_file(menu_scene)
