using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
	[Export]
	public float Speed;
	[Export]
	public float JumpVelocity;
	
	//[Export]
	//public Vector2 bulletOffSet = Vector2();
	//[Export]
	//public float bulletSpeed = 200;
	
	private bool isAttacking = false;
	private bool isAttacking2 = false;
	
	private AnimatedSprite2D animation; 
	
	private PackedScene bullet;
	
	private double timerOfAttack = 0.4;
	private double actualTimerOfAttack = 0.4;
	
	private double timerOfAttack2 = 0.6;
	private double actualTimerOfAttack2 = 0.6;
	
	public override void _Ready() {
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		//bullet = GD.Link
	}

	public override void _PhysicsProcess(double delta)
	{
		
		
		Vector2 velocity = Velocity;

		//if (Input.IsActionJustPressed("")) {
		//	Bullet initBullet = (ataque) bullet.Inter;
		// if (!animation.FlipH) {
		// 		initBullet.Speed *= -1;
		// 		bulletOffSet.X *= -1;
		// }
		//	initBullet.Position = GlobalPosition + bulletOffSet;
	
		//	GetTree.Root.(initBullet);
		//}


		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
			if (!isAttacking) {
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
		
		if (!isAttacking && !isAttacking2) {
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
				actualTimerOfAttack -= delta;
				if (actualTimerOfAttack <= 0){
					actualTimerOfAttack = timerOfAttack;
					isAttacking = false;
				}
			} else {
				actualTimerOfAttack2 -= delta;
				if (actualTimerOfAttack2 <= 0){
					actualTimerOfAttack2 = timerOfAttack2;
					isAttacking2 = false;
				}
			}
			
		}
		if (!IsOnFloor()) {
			Velocity = velocity;
			MoveAndSlide();
		}
		
	}
	
	public void attack() {
		isAttacking = true;
	}
	public void attack2() {
		isAttacking2 = true;
	}
}
