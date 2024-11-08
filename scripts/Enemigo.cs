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
	private bool canAttack = false;


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
		animation.Connect("animation_finished", new Callable(this, nameof(OnAnimationFinished)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (!isAttack) {
			animation.Play("idle");
		} else {
			animation.Play("attack");
			actualTimeAttack -= delta;
			if (actualTimeAttack <= 0){
				actualTimeAttack = timeAttack;
				isAttack = false;
				canAttack = true;
			}
		}
		
	}
	
	public void OnBodyEntered(Node2D body) {
		if (body is MainCharacter characterBody) {
			GD.Print(characterBody.Hp);
		}
	}
	
	private void OnEnemyAttack(Node body) 
	{ 
		GD.Print("Atacando al jugador."); 
		if (body is MainCharacter characterBody) {
			isAttack = true;
			
			if (canAttack) {
				characterBody.ReceiveDamage(20);
				GD.Print(characterBody.Hp);
			}
		}
	}


	private void OnAnimationFinished() { 
		
	// Si está en estado de ataque y la animación de ataque ha terminado 
	if (isAttack && animation.Animation == "attack") { 
		// Encontrar todos los cuerpos dentro del área de ataque 
		var bodies = areaAtaque.GetOverlappingBodies(); 
		foreach (var body in bodies) 
		{ 
			if (body is MainCharacter characterBody) { 
				// Aplicar daño al jugador 
				if (canAttack) { 
					characterBody.ReceiveDamage(20); 
					GD.Print(characterBody.Hp); 
					canAttack = false; 
					// Esperar un poco antes del siguiente ataque 
					Timer attackCooldown = new Timer(); 
					attackCooldown.WaitTime = timeAttack; 
					attackCooldown.OneShot = true; 
					attackCooldown.Connect("timeout", new Callable(this, nameof(ResetAttack))); 
					AddChild(attackCooldown); 
					attackCooldown.Start(); 
					} 
				} 
			} 
		} 
	} 
	private void ResetAttack() 
	{ 
		canAttack = true; 
	}

	
	public void OnBodyExited(Node2D body) {
		if (body is MainCharacter) {
			actualTimeWait = timeWait;
			actualTimeAttack = timeAttack;
		}
	}
}
