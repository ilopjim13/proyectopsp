using Godot;
using System;

public partial class Enemigo : Area2D
{
	[Export]
	public float speed;
	
	private AreaAtaque areaAtaque;
	private AreaVision areaVision;
	
	private CollisionShape2D attackCollision;
	private AnimatedSprite2D animation;
	
	private bool isAttack = false;
	private bool canAttack = true;
	private bool isInside = false;
	private bool isVisible = false;
	
	private MainCharacter characterBody;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		areaAtaque = GetNode<AreaAtaque>("AreaAtaque"); 
		areaAtaque.Connect(nameof(AreaAtaque.EnemyAttackEventHandler), new Callable(this, nameof(OnEnemyAttack))); 
		
		areaVision = GetNode<AreaVision>("AreaVision");
		areaVision.Connect(nameof(AreaVision.EnemyVisionEventHandler), new Callable(this, nameof(OnAreaVision)));
		
		attackCollision = areaAtaque.GetNode<CollisionShape2D>("CollisionAreaAtaque");
		
		animation = GetNode<EnemigoSprite>("EnemigoSprite");
		animation.Connect(nameof(EnemigoSprite.AnimationLoopedEventHandler), new Callable(this, nameof(OnAnimationLooped)));
		
		characterBody = GetNode<MainCharacter>("../CharacterBody2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!isAttack) 
		{
			animation.Play("walk");
		} else {
			animation.Play("attack");
		}
		
		if (isVisible) {
			FollowPlayer(delta);
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
	
	public void OnAreaVision() {
		GD.Print("Jugador en el campo de vision");
		isVisible = true;
	}
	
	public void OnAreaVisionBodyExited() {
		isVisible = false;
	}
	
	private void FollowPlayer(double delta) 
	{ 
		var playerPosition = characterBody.GlobalPosition; 
		var enemyPosition = GlobalPosition; 
		var direction = new Vector2((playerPosition.X - enemyPosition.X), 0).Normalized();

		GlobalPosition = GlobalPosition.Lerp(GlobalPosition + direction, speed * (float)delta);
		animation.FlipH = playerPosition.X < enemyPosition.X;
		
		
		if (animation.FlipH) 
		{ 
			attackCollision.Position = new Vector2(Math.Abs(attackCollision.Position.X) * -1, attackCollision.Position.Y); 
		} 
		else 
		{ 
			attackCollision.Position = new Vector2(Math.Abs(attackCollision.Position.X), attackCollision.Position.Y); 
		}
		
		GD.Print("Siguiendo al jugador"); 
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
			characterBody.ReceiveDamage(10); 
		}
	}
}
