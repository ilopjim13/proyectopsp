using Godot;
using System;

public partial class Hud : TextureProgressBar
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		MinValue = 0; 
		MaxValue = 100; 
		Value = 100; 
		// Valor inicial de vida 
		GD.Print("HUD ready. Barra de vida inicializada.");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	
	public void ActualizarBarraVida(int vida, int maxVida) 
	{ 
		MaxValue = maxVida;
		Value = vida; 
	}
}
