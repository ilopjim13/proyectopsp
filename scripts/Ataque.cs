using Godot;
using System;

public partial class Ataque : Area2D
{
	public float Speed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnAreaEntered(Area2D area) {
		if (area is Enemigo enemigo)
		{
			enemigo.ReceiveDamage(20);
			GD.Print("DUELE");
		} 
	}
	
}

//public partial class Ataque : Area2D
//{
//	//[Export]
//	public float Speed;
//	
//	// Called when the node enters the scene tree for the first time.
//	public override void _Ready()
//	{
//		Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
//	}
//
//	// Called every frame. 'delta' is the elapsed time since the previous frame.
//	public override void _Process(double delta)
//	{
//		Position += new Vector2(Speed * (float) delta, 0.0f);
//	}
//	
//	public void OnAreaEntered(Area2D area) {
//		if (area is Enemigo enemigo)
//		{
//			enemigo.ReceiveDamage(20);
//			GD.Print("DUELE BRO");
//		}
//	}
//}
