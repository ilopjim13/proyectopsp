using Godot;
using System;

public partial class Enemigo : Area2D
{

	private AreaAtaque areaAtaque;
	private AnimatedSprite2D animation;
	
	[Export]
	private double timeAttack = 1.5;
	
	private double timeWait = 1.5;
	private double actualTimeAttack;
	private double actualTimeWait;
	
	private bool isAttack = false;
	private bool canAttack = true;
	private bool isInside = false;


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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (!isAttack) {
			animation.Play("idle");
		} else {
			animation.Play("attack");
			
			if(isInside) {
				Timer attackCooldown = new Timer(); 
				attackCooldown.WaitTime = timeAttack; 
				attackCooldown.OneShot = true; 
				attackCooldown.Connect("timeout", new Callable(this, nameof(ResetAttack))); 
				AddChild(attackCooldown); 
				attackCooldown.Start(); 
			}
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
		if (body is MainCharacter characterBody) {
			isAttack = true;
			if (isAttack && animation.Animation == "idle") { 
				GD.Print("is attack"); 
				var bodies = areaAtaque.GetOverlappingBodies(); 
				foreach (var b in bodies) 
				{ 
					GD.Print("FOR"); 
					characterBody.ReceiveDamage(20); 
					
					} 
				} 
		}
	}

	private void ResetAttack() 
	{ 
		canAttack = true; 
		isAttack = true;
		GD.Print("SE PONE EN TRUE");
	}

	
	public void OnBodyExited(Node2D body) {
		if (body is MainCharacter) {
			actualTimeWait = timeWait;
			actualTimeAttack = timeAttack;
			canAttack = false; 
			isAttack = false;
			isInside = false;
			GD.Print("ADIOS");
		}
	}
}
