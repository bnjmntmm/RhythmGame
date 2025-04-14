extends Node

@onready var fade_overlay = %FadeOverlay
@onready var pause_overlay = %PauseOverlay
@onready var user_should_start_label: Label = $UILAYER/UserShouldStartLabel
@onready var game_over_screen: Control = $UILAYER/GameOverScreen
@onready var main_game_logic: Node3D = $"Game_Scene_C#"

var songKeys
var playedSongByDJ

func _ready() -> void:
	## only for transition and pause
	fade_overlay.visible = true
	%Controls.show()
	main_game_logic.connect("GameOver",game_over)
	#pause_overlay.game_exited.connect(_save_game) ## if we wanna have save stats

func _input(event) -> void:
	if event.is_action_pressed("pause") and not pause_overlay.visible:
		get_viewport().set_input_as_handled()
		get_tree().paused = true
		pause_overlay.grab_button_focus()
		pause_overlay.visible = true

func game_over():
	%FadeOverlay.fade_out()
	
func _on_fade_overlay_on_complete_fade_out() -> void:
	game_over_screen.show()
	%Controls.hide()
