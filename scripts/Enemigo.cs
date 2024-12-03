using Godot;
using System;

public partial class Enemigo : CharacterBody2D
{
	[Export]
	public float speed;

	[Export]
	public int Hp = 50;

	public int MaxHp = 50;
	
	private Vector2 velocity = Vector2.Zero; 
	
	private float gravity = 980f;
	
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
	
	private PackedScene character;
	private Node characterInstance;
	
	private BarraVidaEnemy vidaHud;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		areaAtaque = GetNode<AreaAtaque>("AreaAtaque"); 
		
		attackCollision = areaAtaque.GetNode<CollisionShape2D>("CollisionAreaAtaque");
		
		animation = GetNode<EnemigoSprite>("EnemigoSprite");
		
		characterBody = GetNode<MainCharacter>("../CharacterBody2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		//character = (PackedScene)ResourceLoader.Load("res://scenes/player2.tscn");
		//characterBody = GetNode<MainCharacter>("CharacterBody2D");
		if (!isDeath) {
			
			if (!IsOnFloor()) // Si no está tocando el suelo
			{
				velocity += GetGravity() * (float)delta;  // La gravedad se acumula
			}
			else
			{
				velocity.Y = 0;  // Si está tocando el suelo, no cae
			}
			
			// Utilizamos MoveAndCollide en lugar de MoveAndSlide 
			KinematicCollision2D collision = MoveAndCollide(velocity * (float)delta); 
			// Manejar la colisión si ocurre 
			if (collision != null) { 
			// Ejemplo: detiene el movimiento vertical al colisionar con el suelo 
				if (collision.GetNormal().Y > 0) { 
					velocity.Y = 0; 
				}
			}
			
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
		if (body is MainCharacter characterBody) {
			GD.Print("Atacando al jugador."); 
			isAttack = true;
			isInside = true;
		}
			
	}
	
	public void OnVisionBodyEntered(Node2D body) {
		if (body is MainCharacter characterBody) {
			GD.Print("Jugador en el campo de vision");
			isVisible = true;
		}
		
	}
	
	public void OnAreaVisionBodyExited(Node2D body) {
		if (body is MainCharacter characterBody){
			isVisible = false;
		}
		
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
		MoveAndSlide();
	}

	
	public void OnBodyExited(Node2D body) {
		if (body is MainCharacter) {
			isInside = false;
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
		
		vidaHud = GetNode<BarraVidaEnemy>("EnemigoSprite/BarraVidaEnemy"); 
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
			GD.Print($"El nodo characterBody es de tipo: {characterBody.GetType()}");
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
