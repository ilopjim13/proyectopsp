using Godot;
using System;

public partial class PuertaFinal : Area2D
{
	
	public AnimatedSprite2D animation;
	public Sprite2D press;
	public RichTextLabel text;
	public MainCharacter character;
	public bool isInside = false;
	public bool isItem = false;
	public bool isEnter = false;
	public bool isOpening = false;
	public double timeOpen = 2.4;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		character = GetParent().GetNode<MainCharacter>("CharacterBody2D");
		animation = GetNode<AnimatedSprite2D>("Puerta");
		press = GetNode<Sprite2D>("Press");
		text = GetNode<RichTextLabel>("PressE");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (Input.IsActionJustPressed("takeItem") && isInside) {
			if(isItem) {
				animation.Play("opening");
				isOpening = true;
			}
		}
		if (isOpening) {
			timeOpen-= delta;
			if (timeOpen <= 0){
				animation.Play("open");
				isEnter = true;
				press.Visible = false;
				text.Visible = false;
			}
		}


		if (Input.IsActionJustPressed("ui_up") && isEnter) {
			GetTree().ChangeSceneToFile("res://scenes/menu.tscn"); 
		}
	}
	
	public void OnBodyEntered(Node body) {
		isInside = true;
		for (int i = 0; i < character.inventory.item.Count; i++)
		{
			if (character.inventory.item[i] != null && character.inventory.item[i].name == "Key")
			{
				GD.Print("EH TRUE");
				isItem = true;
			}
		}
		if(isItem) {
			press.Visible = true;
			text.Visible = true;
		}
	}

	public void OnBodyExited(Node body) {
		isInside = false;
		if(isItem) {
			press.Visible = false;
			text.Visible = false;
		}
	}
}
