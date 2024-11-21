using Godot;
using System;

public partial class Puerta : Area2D
{
	private bool isOpen = false;
	private bool isClose = false;
	private AnimatedSprite2D animation;
	
	
	private double actualTimerOfOpen = 0.7;
	private double actualTimerOfClose = 0.7;
	private double timerOfOpen = 0.7;
	private double timerOfClose = 0.7;
	
	private double del = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("PUERTA INICIANDO");
		animation = GetNode<AnimatedSprite2D>("PuertaSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		del = delta;
		
		if(!isOpen && !isClose) {
			animation.Play("lock");
		}
		
	}
	
	public void OnBodyEntered(Node2D body) {
		isOpen = true;
		animation.Play("open");
	}
	
	public void OnBodyExited(Node2D body) {
		GD.Print("ADIOS");
		var timer = new Timer(); 
		timer.WaitTime = timerOfOpen; 
		timer.OneShot = true; 
		timer.Connect("timeout", this, nameof(Exited)); 
		timer.Start();
	}
	
	public void Exited() {
		isOpen = false;
		isClose = true;
		animation.Play("locking");
		GD.Print("HOLAAA");
		var timer = new Timer(); 
		timer.WaitTime = timerOfClose; 
		timer.OneShot = true; 
		timer.Connect("timeout", this, nameof(Exited)); 
		timer.Start();
	}
	
	public void Closing() {
		isClose = false;
	}
	
}
