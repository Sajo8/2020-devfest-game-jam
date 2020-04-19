extends Area2D

var bullet = preload("res://combat/Bullet.tscn")

func _ready() -> void:
	randomize()
	var random = randi() % 3
	var weapon = "rifle"
	match random:
		0:
			weapon = "rifle"
		1:
			weapon = "handgun"
		2:
			weapon = "shotgun"
	var anim_string = "shoot_" + weapon
	$AnimationPlayer.play(anim_string)

	var timers = get_tree().get_nodes_in_group("level_timer")
	if timers:
		timers[0].connect("timer_timeout", self, "game_over")

func _on_Timer_timeout() -> void:
	var b = bullet.instance()
	add_child(b)
	$AudioStreamPlayer.play()

func game_over():
	get_tree().paused = true
	get_node("/root/CombatScreen/LoseDialogue").show()
