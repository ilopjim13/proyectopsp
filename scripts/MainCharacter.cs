using Godot;
using System;

public partial class MainCharacter : CharacterBody2D
{
	[Export]
	public float Speed;
	[Export]
	public float JumpVelocity;
	[Export]
	public float bulletSpeed = 200;
	[Export]
	public float pushForce = -350;
	[Export]
	public int pushDamage = 5;
	[Export]
	public int MaxHp = 100;
	public int Hp = 100;
	public string name = "";
	
	private bool isAttacking = false;
	private bool isAttacking2 = false;
	public bool isCrawl = false;
	public bool isHitting = false;
	public bool isDeath = false;
	public bool isHit = false;
	public bool isPushed = false;
	public bool doubleJump = false;
	public bool isSave = false;
	
	private AnimatedSprite2D animation; 
	public Vector2 bulletOffSet;
	private PackedScene bullet;
	private AudioStreamPlayer2D attackSound;
	private AudioStreamPlayer2D hurtSound;
	private CollisionShape2D collision;
	private ApiController _apiHandler;
	public float BulletSpeed;
	private Game _game;
	private Hud vidaHud;
	[Export]
	public Inventory inventory;
	
	private double timerOfAttack = 0.6;
	private double actualTimerOfAttack = 0.6;
	public double bulletLifeSpan = 0.4;
	private double timerOfAttack2 = 0.6;
	private double actualTimerOfAttack2 = 0.6;
	private double timerOfPush = 0;
	private double actualTimerOfPush = 0;
	private double timerOfHit = 0.4;
	private double actualTimerOfHit = 0.4;
	private double actualTimerOfDeath = 1.35;
	
	public override void _Ready() {
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		bullet = (PackedScene)ResourceLoader.Load("res://scenes/ataque.tscn");
		bulletOffSet = new Vector2(35, 83);
		attackSound = GetNode<AudioStreamPlayer2D>("AttackSound");
		hurtSound = GetNode<AudioStreamPlayer2D>("HurtSound");
		collision = GetNode<CollisionShape2D>("CollisionShape2D");
		_apiHandler = GetNode<ApiController>("../ApiController");
		_game = GetParent() as Game;
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
		
		if (IsOnFloor()) {
			doubleJump = false;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		
		// Handle Jump.
		if (Input.IsActionJustPressed("save"))
		{
			isSave = true;
			SavePlayerData();
		}
		
		if (Input.IsActionJustPressed("jump") && !IsOnFloor())
		{
			if (!doubleJump) {
				velocity.Y = JumpVelocity;
				doubleJump = true;
				GD.Print(doubleJump);
			}
			
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
				collision.Position = new Vector2(0, 105.53f);
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
			collision.Position = new Vector2(0, 95f);
		}

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_left", "ui_left");

		if (isPushed) {
			GD.Print("Velocidad = " + Velocity);
			ReceiveDamage(pushDamage);
			actualTimerOfPush -= delta;
			if (actualTimerOfPush <= 0){
				actualTimerOfPush = timerOfPush;
				isPushed = false;
			}
		}



		if (!isDeath && !isPushed) 
		{
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
		} else if (isDeath) {
			animation.Play("death");
			actualTimerOfDeath-= delta;
			if (actualTimerOfDeath <= 0){
				GetTree().ChangeSceneToFile("res://scenes/gameOver.tscn");
			}
		}
	}
		
	
	public void attack() {
		isAttacking = true;
		attackSound.Play();
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
		hurtSound.Play();
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
	
	public void CollisionEnemy(Vector2 normal) {
		if (!isPushed) {
			Velocity = Vector2.Zero;
			Velocity = normal * pushForce;
			GD.Print(normal);
			//animation.Play("idle");
			GD.Print(Velocity);
			isPushed = true;
		}
	}
	
	public void SavePlayerData()
	{
		if(isSave){
			if(_game.isExist){
				_apiHandler.UpdateCharacter(this);
				isSave = false;
			} else {
				_apiHandler.SaveCharacterData(this);
				isSave = false;
			}
		}
	}
	
	public void UpdateVida() {
		vidaHud = GetNode<Hud>("../CanvasLayer/Hud/BarraVida"); 
		vidaHud.ActualizarBarraVida(Hp, MaxHp); 
	}
	
	
	public void ReceiveDamage(int damage) {
		if (!isHitting) {
			Hp -= damage; 
			isHitting = true;
			if (Hp <= 0) 
				isDeath = true;
			UpdateVida();
		}
	}
}
