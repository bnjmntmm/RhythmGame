extends Node

@onready var fade_overlay = %FadeOverlay
@onready var pause_overlay = %PauseOverlay
@onready var dj_node: Node3D = $World/DJ
@onready var songNode: Songs = $Songs
@onready var user_should_start_label: Label = $UILAYER/UserShouldStartLabel
@onready var input_controller: Node = $InputController
@onready var game_over_screen: Control = $UILAYER/GameOverScreen

var songKeys
var playedSongByDJ

func _ready() -> void:
	input_controller.player_thrown_out.connect(game_over)

	
	
	## only for transition and pause
	fade_overlay.visible = true
	#pause_overlay.game_exited.connect(_save_game) ## if we wanna have save stats
	songKeys = songNode.songs.keys()
	#playedSongByDJ = await dj_node.async_play_song_sequence(songNode.songs[songKeys[0]])
	#user_should_start_label.show()

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
