using Godot;
using System;

public partial class Pichos : Area2D
{
	
	public bool isInside = false;
	public double ticksPerDamage = 0f;
	public MainCharacter characterBody;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isInside) {
			ticksPerDamage+= delta;
			if (ticksPerDamage == 0 || ticksPerDamage % 1 <= 0.05 ){
				characterBody.ReceiveDamage(5);
			}
		}
	}

	
	public void OnBodyEntered(Node2D body) {
		GD.Print("hola");
		if (body is MainCharacter character) {
			isInside = true;
			characterBody = character;
		}
	}
	
	public void OnBodyExited(Node2D body) {
		isInside = false;
		ticksPerDamage = 0f;
	}
	
}
