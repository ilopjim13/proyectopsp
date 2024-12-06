using Godot;
using System;

public partial class DropItem : Area2D
{
	private bool isInside = false;
	private AnimatedSprite2D animation;
	private MainCharacter character;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GD.Print(GetParent().GetNode<MainCharacter>("CharacterBody2D"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isInside) {
			if (Input.IsActionJustPressed("takeItem"))
			{
				animation.Play("whitoutItem");
			}
		}
	}
	
	public void OnBodyEntered(Node body) {
		if (body is MainCharacter) {
			isInside = true;
			GD.Print("HOlaa");
		}
		
	}
	
	public void OnBodyExited(Node body) {
		if (body is MainCharacter) {
			isInside = false;
			GD.Print("ADIOS");
		}
	}
}