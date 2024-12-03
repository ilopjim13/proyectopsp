extends Node

func _input(event:InputEvent) -> void:
	if Input.is_action_just_pressed("ui_cancel"):
		get_tree().paused = not get_tree().paused
		$Control.visible = not $Control.visible


func _on_salir_pressed() -> void:
	get_tree().quit()
	pass # Replace with function body.


func _on_continuar_pressed() -> void:
	get_tree().paused = not get_tree().paused
	$Control.visible = not $Control.visible
	pass # Replace with function body.
