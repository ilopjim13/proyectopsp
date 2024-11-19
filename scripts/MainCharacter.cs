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

	public bool isHitting = false;
	public bool isDeath = false;
	
	private AnimatedSprite2D animation; 
	
	private PackedScene bullet;

	public int MaxHp = 100;
	public int Hp = 100;
	
	private double timerOfAttack = 0.4;
	private double actualTimerOfAttack = 0.4;

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
		bulletOffSet = new Vector2(30, 90);
	}

	public override void _PhysicsProcess(double delta)
	{
		
		
		Vector2 velocity = Velocity;

		// Verificar si una acción específica ha sido presionada
		if (Input.IsActionJustPressed("attack")) 
		{
			float isFlipped = -1.0f;
			//GD.Print("Fire");
			Ataque instBullet = (Ataque) bullet.Instantiate();
			if(!animation.FlipH) isFlipped = 1.0f;
			instBullet.Speed = isFlipped * BulletSpeed;
			
			instBullet.GlobalPosition = GlobalPosition + new Vector2(bulletOffSet.X * isFlipped, bulletOffSet.Y);
			GetTree().Root.AddChild(instBullet);

			var timer = new Timer(); 
			timer.WaitTime = bulletLifeSpan; 
			timer.OneShot = true; 
			timer.Connect("timeout", new Callable(instBullet, "queue_free")); 
			instBullet.AddChild(timer); 
			timer.Start();

		}


		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
			if (!isAttacking && !isAttacking2) {
				animation.Play("jump");
			}
		} 

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		
		if (Input.IsActionJustPressed("attack") && isAttacking == false)
		{
			attack();
			animation.Play("attack");
		}
		
		if (Input.IsActionJustPressed("attack2") && isAttacking == false)
		{
			attack2();
			animation.Play("attack2");
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_left", "ui_left");

		if (!isDeath) {
			if (!isAttacking && !isAttacking2 && !isHitting) 
			{
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
				} else {
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
				GetTree().ReloadCurrentScene();
			}
		}
	}
		
	
	public void attack() {
		isAttacking = true;
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
		Hp -= damage; 
		isHitting = true;
		if (Hp <= 0) 
			isDeath = true;
			
			
		vidaHud = GetNode<Hud>("../CharacterBody2D/BarraVida"); 
		vidaHud.ActualizarBarraVida(Hp, MaxHp); 
	}
}
