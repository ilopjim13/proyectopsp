using Godot;
using System;

public partial class InvUiSlot : Panel
{
	public Sprite2D itemVisual;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemVisual = GetNode<Sprite2D>("CenterContainer/Panel/item_display");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void update(InventoryItem item) {
		if(item == null) {
			itemVisual.Visible = false;
		} else {
			GD.Print("HO");
			itemVisual.Visible = true;
			itemVisual.Texture = item.texture;
		}
		
	}
}
