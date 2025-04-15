extends Node

@onready var fade_overlay = %FadeOverlay
@onready var pause_overlay = %PauseOverlay
@onready var user_should_start_label: Label = $UILAYER/UserShouldStartLabel
@onready var game_over_screen: Control = $UILAYER/GameOverScreen
@onready var main_game_logic: Node3D = $"Game_Scene_C#"

var songKeys
var playedSongByDJ

var is_win_screen:bool = false

func _ready() -> void:
	## only for transition and pause
	fade_overlay.visible = true
	%Controls.show()
	main_game_logic.connect("GameOver",game_over)
	main_game_logic.connect("PlayerAtDJ",win_screen)
	#pause_overlay.game_exited.connect(_save_game) ## if we wanna have save stats

func _input(event) -> void:
	if event.is_action_pressed("pause") and not pause_overlay.visible and not is_win_screen:
		get_viewport().set_input_as_handled()
		get_tree().paused = true
		pause_overlay.grab_button_focus()
		pause_overlay.visible = true

func win_screen():
	%Controls.hide()
	%FadeOverlay.hide()
	%ForegroundUI.hide()
	%WinScreen.show()
	is_win_screen= true
func game_over():
	%FadeOverlay.fade_out()
	
func _on_fade_overlay_on_complete_fade_out() -> void:
	game_over_screen.show()
	%Controls.hide()
	%ForegroundUI.hide()
	%FadeOverlay.hide()
	
