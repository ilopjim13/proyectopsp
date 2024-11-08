using Godot;
using System;

public partial class area2d : Area2D
{

	private AreaAtaque areaAtaque;
	private AnimatedSprite2D animation;
	
	private double timeAttack = 1.5;
	private double timeWait = 1.5;
	private double actualTimeAttack = 1.5;
	private double actualTimeWait = 1.5;
	
	private bool isAttack = false;
	private bool canAttack = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		areaAtaque = GetNode<AreaAtaque>("AreaAtaque"); 
		if (areaAtaque != null) { 
			areaAtaque.Connect(nameof(AreaAtaque.OnEnemyAttack), new Callable(this, nameof(OnEnemyAttack))); 
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
			actualTimeAttack -= delta;
			if (actualTimeAttack <= 0){
				actualTimeAttack = timeAttack;
				isAttack = false;
			}
			
			actualTimeWait -= delta;
			if (actualTimeWait <= 0){
				actualTimeWait = timeWait;
				canAttack = true;
			}
		}
		
	}
	
	public void OnBodyEntered(Node2D body) {
		//GetTree().ReloadCurrentScene();
		if (body is MainCharacter characterBody) {
			//characterBody.ReceiveDamage(20);
			GD.Print(characterBody.Hp);
		}
	}
	
	private void OnEnemyAttack(Node body) 
	{ 
		GD.Print("Atacando al jugador."); 
	// Lógica de ataque aquí, por ejemplo: 
		if (body is MainCharacter characterBody) {
			isAttack = true;
			
			if (canAttack) {
				characterBody.ReceiveDamage(20);
				GD.Print(characterBody.Hp);
				canAttack = false;
			}
		}
	}
}
