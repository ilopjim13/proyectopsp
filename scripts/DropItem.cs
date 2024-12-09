using Godot;
using System;

public partial class DropItem : Area2D
{
	private bool isInside = false;
	private AnimatedSprite2D animation;
	private MainCharacter character;
	private InventoryItem key;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		character = GetParent().GetNode<MainCharacter>("CharacterBody2D");
		key = ResourceLoader.Load<InventoryItem>("res://inventory/items/key.tres");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isInside) {
			if (Input.IsActionJustPressed("takeItem"))
			{
				animation.Play("whitoutItem");
				character.inventory.item[0] = key;
				GD.Print(character.inventory.item);
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
