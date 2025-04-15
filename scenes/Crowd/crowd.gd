extends Node3D

@export var crowdBackTexture : Texture2D
@export var crowdFrontTexture : Texture2D

# Maximum displacement (in world units) from the original position.
@export var wobbleAmplitude : float = 10.0
# Controls how quickly the wobble evolves over time.
@export var wobbleSpeed : float = 1.0

@onready var crowd_back: Sprite3D = $CrowdBack
@onready var crowd_front: Sprite3D = $CrowdFront

# To store the original positions of the sprites.
var original_back_pos: Vector3
var original_front_pos: Vector3

# Noise generator for smooth random offsets.
var noise: FastNoiseLite
var original_global_pos: Vector3

func _ready() -> void:
	# Capture the parent's starting global position.
	original_global_pos = global_position
	# Set the textures for the sprites.
	crowd_back.texture = crowdBackTexture
	crowd_front.texture = crowdFrontTexture

	###ANIMATIONPLAYER
	#$AnimationPlayer.play("DANCE")

	## Save the original positions to use as the central point for the wobble effect.
	#original_back_pos = crowd_back.position
	#original_front_pos = crowd_front.position
#
	## Initialize FastNoiseLite.
	#noise = FastNoiseLite.new()
	#noise.seed = randi()                          # Randomize each run.
	#noise.noise_type = FastNoiseLite.TYPE_SIMPLEX  # Choose a smooth noise type.
	#noise.fractal_octaves = 2                          # Number of layers of noise.
	#noise.frequency = 0.25                        # Frequency set to 0.25 (period ≈4 units).
	#noise.fractal_gain = 0.8          
#
#func _process(delta: float) -> void:
	## Scale time by the wobble speed.
	#var time =Time.get_ticks_msec() / 1000.0 * wobbleSpeed
#
	## Generate a 2D noise offset. Different time shifts can be used for x and y.
	#var offset = Vector2(
		#noise.get_noise_1d(time),
		#noise.get_noise_1d(time + 50.0)
	#) * wobbleAmplitude
#
	## Apply the noise offset to the parent's original global position.
	#global_position = original_global_pos + Vector3(offset.x, offset.y, 0)
