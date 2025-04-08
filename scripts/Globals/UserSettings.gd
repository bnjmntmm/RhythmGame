extends Node

signal on_value_change(key, value)
const SECTION ="user"
const SETTINGS_FILE = "user://settings.cfg"

const MASTERVOLUME_ENABLED = "mastervolume_enabled"
const BEATSVOLUME_ENABLED = "beatsvolume_enabled"
const BGMUSICVOLUME_ENABLED = "bgvolumevolume_enabled"
const MASTERVOLUME = "mastervolume"
const BEATSVOLUME = "beatsvolume"
const BGMUSICVOLUME = "bgmusicvolume"

const AUDIO_BUS_MASTER = "Master"
const AUDIO_BUS_BEATS = "Beats"
const AUDIO_BUS_BGMUSIC = "BGMusic"


var USER_SETTING_DEFAULTS = {
	MASTERVOLUME:100,
	BGMUSICVOLUME:70,
	BEATSVOLUME:100,
}

var config:ConfigFile

func _ready() -> void:
	config = ConfigFile.new()
	config.load(SETTINGS_FILE)
	_configure_audio()
	
func set_value(key, value):
	config.set_value(SECTION, key, value)
	config.save(SETTINGS_FILE)
	if key == MASTERVOLUME:
		_update_volume(MASTERVOLUME, AUDIO_BUS_MASTER)
	if key == BEATSVOLUME:
		_update_volume(BEATSVOLUME, AUDIO_BUS_BEATS)
	if key == BGMUSICVOLUME:
		_update_volume(BGMUSICVOLUME, AUDIO_BUS_BGMUSIC)
	emit_signal("on_value_change", key, value)
func get_value(key):
	return config.get_value(SECTION, key, _get_default(key))
	
func get_value_with_default(key, default):
	return config.get_value(SECTION, key, default)

func _get_default(key):
	if USER_SETTING_DEFAULTS.has(key):
		return USER_SETTING_DEFAULTS[key]
	return null


func _configure_audio():
	_update_volume(MASTERVOLUME, AUDIO_BUS_MASTER)
	_update_volume(BEATSVOLUME, AUDIO_BUS_BEATS)
	_update_volume(BGMUSICVOLUME, AUDIO_BUS_BGMUSIC)
	
func _update_volume(property, bus):
	var current = (get_value_with_default(property, USER_SETTING_DEFAULTS[property]) -100) / 2
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index(bus), current)
