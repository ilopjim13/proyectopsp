using Godot;
using System;

public partial class InvUiSlot : Panel
{
	public Sprite2D itemVisual;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemVisual = GetNode<Sprite2D>("CenterContainer/item_display");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void update(Inventory_item item) {
		if(item == null) {
			itemVisual.Visible = false;
		} else {
			itemVisual.Visible = true;
			itemVisual.Texture = item.texture;
		}
		
	}
}
