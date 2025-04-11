extends Control

@export var menu_scene : String


func _on_menu_button_pressed() -> void:
	%FadeOverlay.fade_out()


func _on_quit_button_pressed() -> void:
	get_tree().quit()


func _on_fade_overlay_on_complete_fade_out() -> void:
	get_tree().change_scene_to_file(menu_scene)
