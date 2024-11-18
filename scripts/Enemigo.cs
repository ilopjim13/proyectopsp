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
	
	private MainCharacter characterBody;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		areaAtaque = GetNode<AreaAtaque>("AreaAtaque"); 
		areaAtaque.Connect(nameof(AreaAtaque.EnemyAttackEventHandler), new Callable(this, nameof(OnEnemyAttack))); 
		
		animation = GetNode<EnemigoSprite>("EnemigoSprite");
		animation.Connect(nameof(EnemigoSprite.AnimationLoopedEventHandler), new Callable(this, nameof(OnAnimationLooped)));
		
		characterBody = GetNode<MainCharacter>("../CharacterBody2D");
		
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
	}

	
	public void OnBodyExited(Node2D body) {
		if (body is MainCharacter) {
			isInside = false;
			GD.Print("ADIOS");
		}
	}
	
	public void OnAnimationLooped() {
		if (!isInside && isAttack) {
			canAttack = false; 
			isAttack = false;
		} 
		if (isInside && isAttack) {
			canAttack = true; 
			isAttack = true;
			characterBody.ReceiveDamage(20); 
		}
	}
}
