extends Node2D

@export var game_scene : PackedScene
@export var settings_scene : PackedScene

@onready var overlay: FadeOverlay = %FadeOverlay

@onready var new_game_button: Button = $UI/PlayGameButton
@onready var settings_button: Button = $UI/SettingsButton
@onready var exit_button: Button = $UI/LeaveGameButton

var next_scene = game_scene
var new_game = true

var new_game_button_2

func _ready() -> void:
	overlay.visible = true
	#new_game_button.disabled = game_scene == null
	settings_button.disabled = settings_scene == null
	
	new_game_button.pressed.connect(_on_play_button_pressed)
	settings_button.pressed.connect(_on_settings_button_pressed)
	exit_button.pressed.connect(_on_exit_button_pressed)
	overlay.on_complete_fade_out.connect(_on_fade_overlay_on_complete_fade_out)

func _on_settings_button_pressed() -> void:
	new_game = false
	next_scene = settings_scene
	overlay.fade_out()
	

func _on_play_button_pressed() -> void:
	next_scene = game_scene
	overlay.fade_out()

func _on_exit_button_pressed() -> void:
	get_tree().quit()

func _on_fade_overlay_on_complete_fade_out() -> void:
	## this only if we wanna have a save file
	#if new_game and SaveGame.has_save():
		#SaveGame.delete_save()
	get_tree().change_scene_to_packed(next_scene)
