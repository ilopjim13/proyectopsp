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
	
	private AnimatedSprite2D animation; 
	
	private PackedScene bullet;
	
	private double timerOfAttack = 1.4;
	private double actualTimerOfAttack = 1.4;
	
	
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
			animation.Play("jump");
		} 

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		
		if (Input.IsActionJustPressed("attack") && isAttacking == false)
		{
			attack();
			
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_left", "ui_left");
		
		if (!isAttacking) {
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
				animation.FlipH = velocity.X < 0;
				if (IsOnFloor())
				{
					animation.Play("walk");
				} 
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				if (IsOnFloor())
				{
					animation.Play("idle");
				} 
			}
		} else {
			actualTimerOfAttack -= delta;
			if (actualTimerOfAttack <= 0){
				actualTimerOfAttack = timerOfAttack;
				isAttacking = false;
			}
			animation.Play("attack");
		}
		Velocity = velocity;
		MoveAndSlide();
	}
	
	public void attack() {
		isAttacking = true;
	}
}
