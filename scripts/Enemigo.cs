using Godot;
using System;

public partial class Enemigo : Area2D
{
	[Export]
	public float speed;

	[Export]
	public int Hp = 50;

	public int MaxHp = 50;
	
	private AreaAtaque areaAtaque;
	private AreaVision areaVision;
	
	private double actualTimerOfHit = 0.7;
	private double actualTimerOfDeath = 0.7;
	private double timerOfHit = 0.7;
	
	private CollisionShape2D attackCollision;
	private AnimatedSprite2D animation;
	
	private bool isAttack = false;
	private bool canAttack = true;
	private bool isInside = false;
	private bool isVisible = false;
	private bool isDeath = false;
	private bool isTakeHit = false;
	
	private MainCharacter characterBody;
	
	private BarraVidaEnemy vidaHud;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		areaAtaque = GetNode<AreaAtaque>("AreaAtaque"); 
		areaAtaque.Connect(nameof(AreaAtaque.EnemyAttackEventHandler), new Callable(this, nameof(OnEnemyAttack))); 
		
		//areaVision = GetNode<AreaVision>("AreaVision");
		//areaVision.Connect(nameof(AreaVision.EnemyVisionEventHandler), new Callable(this, nameof(OnBodyEnteredVision)));
		
		attackCollision = areaAtaque.GetNode<CollisionShape2D>("CollisionAreaAtaque");
		
		animation = GetNode<EnemigoSprite>("EnemigoSprite");
		animation.Connect(nameof(EnemigoSprite.AnimationLoopedEventHandler), new Callable(this, nameof(OnAnimationLooped)));
		
		characterBody = GetNode<MainCharacter>("../CharacterBody2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		
		if (!isDeath) {
			if (!isAttack && !isTakeHit) 
			{
				if (isVisible) {
					animation.Play("walk");
				} else {
					animation.Play("idle");
				}
				
			} else {
				if (isAttack && !isTakeHit) {
					animation.Play("attack");
				} else {
					TakeHit(delta);
				}
				
			}
			
			if (isVisible) {
				FollowPlayer(delta);
			}
		} else {
			animation.Play("death");
			Eliminated(delta);
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
	
	public void OnVisionBodyEntered(Node2D body) {
		GD.Print("Jugador en el campo de vision");
		isVisible = true;
	}
	
	public void OnAreaVisionBodyExited(Node2D body) {
		isVisible = false;
		GD.Print("ADIOOO");
	}
	
	private void FollowPlayer(double delta) 
	{ 
		var playerPosition = characterBody.GlobalPosition; 
		var enemyPosition = GlobalPosition; 
		var direction = new Vector2((playerPosition.X - enemyPosition.X), 0).Normalized();

		if(!isAttack) {
			GlobalPosition = GlobalPosition.Lerp(GlobalPosition + direction, speed * (float)delta);
			animation.FlipH = playerPosition.X < enemyPosition.X;
		}
		
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

	public void ReceiveDamage(int damage) {
		Hp-= damage;
		if (Hp <= 0) {
			isDeath = true;
		} else {
			GD.Print(Hp);
			isTakeHit = true;
		}
		
		vidaHud = GetNode<BarraVidaEnemy>("../Enemigo/EnemigoSprite/BarraVidaEnemy"); 
		if(vidaHud == null) {
		}
		vidaHud.ActualizarBarraVida(Hp, MaxHp); 
		
	}
	
	public void TakeHit(double delta) {
		animation.Play("takeHit");
		actualTimerOfHit-= delta;
		if (actualTimerOfHit <= 0){
			actualTimerOfHit = timerOfHit;
			isTakeHit = false;
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
			characterBody.ReceiveDamage(5); 
		}
	}


	public void Eliminated(double delta) {
		actualTimerOfDeath-= delta;
			if (actualTimerOfDeath <= 0){
				QueueFree();
			}
	}
}
