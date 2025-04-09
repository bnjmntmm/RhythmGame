extends Node3D

## has idle animation and a walk animation
## idle could be played when dj is doing stuff
## walk when going to next / previous stage?
@onready var animation_player: AnimationPlayer = $AnimationPlayer

## where the player currently is
## there are [-1, 0, 1, 2, 3, 4] and 4 is goal
var current_stage = 0
