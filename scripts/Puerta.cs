using Godot;
using System;

public partial class Puerta : Area2D
{
	private bool isOpen = false;
	private bool isClose = false;
	private AnimatedSprite2D animation;
	
	private double timerOfOpen = 2.7;
	private double timerOfClose = 5.7;
	
	private Timer timer1;
	private Timer timer2;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("PuertaSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if(!isOpen && !isClose) {
			animation.Play("lock");
		}
		
	}
	
	public void OnBodyEntered(Node2D body) {
		if(!isOpen) {
			isOpen = true;
			animation.Play("open");
		}
	}

	
}
