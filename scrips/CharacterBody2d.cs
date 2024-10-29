using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
	[Export]
	public float Speed;
	[Export]
	public float JumpVelocity;
	
	private AnimatedSprite2D animation; 
	
	public override void _Ready() {
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			animation.FlipH = velocity.X < 0;
			animation.Play("walk");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			animation.Play("idle");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
