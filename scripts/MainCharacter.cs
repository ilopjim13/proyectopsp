using Godot;
using System;

public partial class MainCharacter : CharacterBody2D
{
	[Export]
	public float Speed;
	[Export]
	public float JumpVelocity;
	
	[Export]
	public Vector2 bulletOffSet;
	[Export]
	public float bulletSpeed = 200;
	
	private bool isAttacking = false;
	private bool isAttacking2 = false;

	public bool isCrawl = false;
	public bool isHitting = false;
	public bool isDeath = false;
	public bool isHit = false;
	
	private AnimatedSprite2D animation; 
	
	private PackedScene bullet;

	public int MaxHp = 100;
	public int Hp = 100;
	
	private double timerOfAttack = 0.6;
	private double actualTimerOfAttack = 0.6;

	public double bulletLifeSpan = 0.4;
	
	private double timerOfAttack2 = 0.6;
	private double actualTimerOfAttack2 = 0.6;

	private double timerOfHit = 0.4;
	private double actualTimerOfHit = 0.4;
	
	private double actualTimerOfDeath = 1.55;
	
	public float BulletSpeed;
	
	private Hud vidaHud;
	
	public override void _Ready() {
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		bullet = (PackedScene)ResourceLoader.Load("res://scenes/ataque.tscn");
		bulletOffSet = new Vector2(35, 83);
		AddToGroup("jugadores");
	}

	public override void _PhysicsProcess(double delta)
	{
		
		
		Vector2 velocity = Velocity;

		// Verificar si una acción específica ha sido presionada
		if (Input.IsActionJustPressed("attack")) 
		{
			if (!isHitting && !isAttacking) {
				float isFlipped = 1.1f;
				Ataque instBullet = (Ataque) bullet.Instantiate();
				if(animation.FlipH) isFlipped = -2f;
				instBullet.Speed = isFlipped * BulletSpeed;
				
				var bulletSpawnPoint = new Node2D(); 
				if (isCrawl) {
					bulletSpawnPoint.Position = new Vector2(bulletOffSet.X * isFlipped, bulletOffSet.Y + 12f); 
				} else {
					bulletSpawnPoint.Position = new Vector2(bulletOffSet.X * isFlipped, bulletOffSet.Y); 
				}
				
				
				AddChild(bulletSpawnPoint);
				
				bulletSpawnPoint.AddChild(instBullet);
				instBullet.GlobalPosition = bulletSpawnPoint.GlobalPosition;
				
				var timer = new Timer(); 
				timer.WaitTime = bulletLifeSpan; 
				timer.OneShot = true; 
				timer.Connect("timeout", new Callable(instBullet, "queue_free")); 
				instBullet.AddChild(timer); 
				timer.Start();
				
			}
			
		}


		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
			if (!isAttacking && !isAttacking2) {
				animation.Play("jump");
			
			} else {
				animation.Play("jumpAttack");
			}
		} 

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		
		if (Input.IsActionJustPressed("attack") && !isAttacking && !isCrawl)
		{
			if(!isHitting) {
				attack();
				animation.Play("attack");
			}
			
			
		}
		
		if (Input.IsActionJustPressed("attack2") && isAttacking == false)
		{
			attack2();
		}
		
		if (Input.IsActionJustPressed("crawl")) {
			if (IsOnFloor()) {
				animation.Position = new Vector2(0, 89f);
				isCrawl = true;
				animation.Play("crawl");
			}
		}
		
		
		if (Input.IsActionPressed("crawl") && Input.IsActionJustPressed("attack") && IsOnFloor()) {
			if (!isAttacking) {
				attack();
				animation.Play("crawlAttack");
				if (animation.FlipH) {
					animation.Position = new Vector2(-26.635f, 89f);
				} else {
					animation.Position = new Vector2(26.635f, 89f);
				}
			}
			
		}
		
		if (Input.IsActionJustReleased("crawl")) {
			isCrawl = false;
		}

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_left", "ui_left");

		if (!isDeath) {
			if (!isAttacking && !isAttacking2 && !isHitting && !isCrawl) 
			{
				animation.Position = new Vector2(0f, 89f);
				if (direction != Vector2.Zero)
				{
					velocity.X = direction.X * Speed;
					animation.FlipH = velocity.X < 0;
					if (IsOnFloor())
					{
						animation.Play("walk");
						Velocity = velocity;
						MoveAndSlide();
					} 
				}
				else
				{
					velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
					if (IsOnFloor())
					{
						animation.Play("idle");
						Velocity = velocity;
						MoveAndSlide();
					} 
				}
			} else {
				if (isAttacking) {
					attacking(delta);
				} else if(isAttacking2) {
					attacking2(delta);
				} else if (isHitting) {
					takeHit(delta);
					
				}
			}
			if (!IsOnFloor()) {
				Velocity = velocity;
				MoveAndSlide();
			}
		} else {
			animation.Play("death");
			actualTimerOfDeath-= delta;
			if (actualTimerOfDeath <= 0){
				GetTree().ChangeSceneToFile("res://scenes/gameOver.tscn");
			}
		}
	}
		
	
	public void attack() {
		isAttacking = true;
		if (animation.FlipH) {
			animation.Position = new Vector2(-25.76f, 89f);
		} else {
			animation.Position = new Vector2(25.76f, 89f);
		}
	}
	
	public void attack2() {
		isAttacking2 = true;
	}
	
	public void attacking(double delta) {
		if (isAttacking) {
			
			actualTimerOfAttack -= delta;
			if (actualTimerOfAttack <= 0){
				actualTimerOfAttack = timerOfAttack;
				isAttacking = false;
			}
		}
	}
	
	public void takeHit(double delta) {
		animation.Position = new Vector2(0, 89f);
		isCrawl = false; 
		animation.Play("takeHit");
		actualTimerOfHit-= delta;
		if (actualTimerOfHit <= 0){
			actualTimerOfHit = timerOfHit;
			isHitting = false;
		}
	}
	
	public void attacking2(double delta) {
		actualTimerOfAttack2 -= delta;
		if (actualTimerOfAttack2 <= 0){
			actualTimerOfAttack2 = timerOfAttack2;
			isAttacking2 = false;
		}
	}
	
	public void ReceiveDamage(int damage) {
		if (!isHitting) {
			Hp -= damage; 
			isHitting = true;
			if (Hp <= 0) 
				isDeath = true;
			vidaHud = GetNode<Hud>("../CanvasLayer/BarraVida"); 
			vidaHud.ActualizarBarraVida(Hp, MaxHp); 
		}
	}
}
