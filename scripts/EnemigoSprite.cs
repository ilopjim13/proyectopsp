using Godot;
using System;

public partial class EnemigoSprite : AnimatedSprite2D
{
	public delegate void AnimationFinished(); 
	
	public override void _Ready() 
	{ 
		// Conectar la señal animation_finished del AnimatedSprite2D a un método local 
		Connect("animation_finished", new Callable(this, nameof(AnimationFinished))); 
	} 
		
		private void OnAnimationFinished() 
		{ 
			// Emitir la señal personalizada 
			EmitSignal(nameof(AnimationFinished)); 
		}
}
