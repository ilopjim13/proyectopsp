using Godot;
using System;

public partial class AreaVision : Area2D
{
	[Signal]
	public delegate void EnemyVisionEventHandler(Node body);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("te veo");
		Connect("body_entered", new Callable(this, nameof(OnAreaVisionBodyEntered)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnAreaVisionBodyEntered(Node body) 
	{
		if (body is CharacterBody2D) 
		{
			EmitSignal(nameof(EnemyVisionEventHandler), body);
		}
	}
}
