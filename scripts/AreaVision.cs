using Godot;
using System;

public partial class AreaVision : Area2D
{
	[Signal]
	public delegate void EnemyVisionEventHandler(Node body);
	
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnBodyEntered(Node body) 
	{
		if (body is CharacterBody2D) 
		{
			GD.Print("te veo");
			EmitSignal(nameof(EnemyVisionEventHandler), body);
		}
	}
	

}
