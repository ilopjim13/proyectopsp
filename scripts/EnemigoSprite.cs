using Godot;
using System;

public partial class EnemigoSprite : AnimatedSprite2D
{
	[Signal]
	public delegate void AnimationLoopedEventHandler(); 
	
	public override void _Ready() 
	{ 
		Connect("animation_looped", new Callable(this, nameof(OnAnimationLooped))); 
	} 
	
	
	public override void _EnterTree()
	{
		AddUserSignal("AnimationLoopedEventHandler");
	}
	
	
	private void OnAnimationLooped() 
	{ 
		EmitSignal(nameof(AnimationLoopedEventHandler)); 
	}
	
	
}
