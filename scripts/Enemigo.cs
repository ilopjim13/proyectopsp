using Godot;
using System;

public partial class Enemigo : Area2D
{

	private AreaAtaque areaAtaque;
	private AnimatedSprite2D animation;
	
	[Export]
	private double timeAttack = 1.2;
	
	private double timeWait = 1.5;
	private double actualTimeAttack;
	private double actualTimeWait;
	
	private bool isAttack = false;
	private bool canAttack = true;
	private bool isInside = false;
	Timer attackCooldown; 
	
	private MainCharacter characterBody;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		actualTimeAttack = timeAttack;
		actualTimeWait = timeWait;
		areaAtaque = GetNode<AreaAtaque>("AreaAtaque"); 
		if (areaAtaque != null) { 
			areaAtaque.Connect(nameof(AreaAtaque.EnemyAttackEventHandler), new Callable(this, nameof(OnEnemyAttack))); 
		}
		animation = GetNode<AnimatedSprite2D>("EnemigoSprite");
		characterBody = GetNode<MainCharacter>("../CharacterBody2D");
		
		animation.Connect("AnimationFinished", new Callable(this, nameof(Reset)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (!isAttack) {
			animation.Play("idle");
		} else {
			animation.Play("attack");
		}
		
	}
	
	public void OnBodyEntered(Node2D body) {
		if (body is MainCharacter characterBody) {
			GD.Print("besito");
		}
	}
	
	private void OnEnemyAttack(Node body) 
	{ 
		GD.Print("Atacando al jugador."); 
		isAttack = true;
		isInside = true;
		attackCooldown = new Timer();
		attackCooldown.WaitTime = timeAttack; 
		attackCooldown.OneShot = true; 
		attackCooldown.Connect("timeout", new Callable(this, nameof(ResetAttack))); 
		AddChild(attackCooldown); 
		attackCooldown.Start(); 
	}

	private void ResetAttack() 
	{ 
		characterBody.ReceiveDamage(20); 
		canAttack = true; 
		isAttack = true;
		attackCooldown.Start();
		GD.Print("SE PONE EN TRUE");
	}

	
	public void OnBodyExited(Node2D body) {
		if (body is MainCharacter) {
			actualTimeWait = timeWait;
			actualTimeAttack = timeAttack;
			attackCooldown.Stop();
			GD.Print("ADIOS");
		}
	}
	
	public void Reset() {
		if (!isInside) {
			canAttack = false; 
			isAttack = false;
			isInside = false;
			GD.Print("RESETEADO");
		}
	}
}
