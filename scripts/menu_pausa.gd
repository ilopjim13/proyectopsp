extends Control

@onready var hud = get_node("../Hud")

func _input(event:InputEvent) -> void:
	if Input.is_action_just_pressed("ui_cancel"):
		get_tree().paused = not get_tree().paused
		if visible:
			visible = false
		else:
			visible = true
			
		check_hud()


func _on_salir_pressed() -> void:
	get_tree().quit()


func _on_continuar_pressed() -> void:
	get_tree().paused = not get_tree().paused
	visible = not visible
	
func check_hud():
	if visible:
		hud.visible = false
	else:
		hud.visible = true
