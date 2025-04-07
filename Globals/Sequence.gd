extends Node
class_name Sequence


signal played_sound(sound)

@export_range(60,240,10) var bpm = 60
		
		
var sound_offset

@export var sounds : Array[Resource]
@onready var audio_player: AudioStreamPlayer = $AudioPlayer



func generate_random_sequence(sequence_count: int) -> Dictionary:
	sound_offset = 60.0 / bpm ## do we wanna hardcode this part??  ### this is for conversion of the sound offet to the next part
	randomize()
	var current_sequence : Dictionary = {}
	for i in range(sequence_count):
		var sound : Sound = sounds.pick_random()
		current_sequence[i] = {
			"sound": sound.audio_stream,
			"name": sound.audio_name,
			"offset": sound_offset ## this should be the offset according to bpm to the next entry?? 
		} 
	return current_sequence


func play_sequence(sequence : Dictionary):
	for entry in sequence.values():
		audio_player.stream = entry.sound
		audio_player.play()
		played_sound.emit(entry)
		await get_tree().create_timer(entry.offset).timeout
	pass
