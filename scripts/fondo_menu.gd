extends ParallaxBackground

var DIR = Vector2(1,0)
var speed = 80

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	scroll_base_offset += DIR * speed * delta
