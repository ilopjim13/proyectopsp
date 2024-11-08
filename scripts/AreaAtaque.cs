using Godot;
using System;

public partial class AreaAtaque : Area2D
{
	[Signal]
	public delegate void OnEnemyAttackEventHandler(Node body);

	public override void _Ready()
	{
		GD.Print("Area de ataque lista.");
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node body)
	{
		if (body is CharacterBody2D)
		{
			GD.Print("Jugador en el área de ataque.");
			// Disparar la señal de ataque
			EmitSignal(nameof(OnEnemyAttackEventHandler), body);
		}
	}
}
