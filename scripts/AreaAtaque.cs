using Godot;
using System;

public partial class AreaAtaque : Area2D
{
	[Signal]
	public delegate void EnemyAttackEventHandler(Node body);

	public override void _Ready()
	{
		GD.Print("Area de ataque lista.");
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
			GD.Print("Jugador en el área de ataque.");
			EmitSignal(nameof(EnemyAttackEventHandler), body);
		}
	}
}
