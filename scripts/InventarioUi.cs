using Godot;
using System;

public partial class InventarioUi : Control
{
	public bool is_open = false;

	public Inventory inv; 
	public Godot.Collections.Array<InvUiSlot> slots;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Close();
		slots = new Godot.Collections.Array<InvUiSlot>();
		UpdateSlots();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("i")) {
			if (is_open) {
				Close();
			} else {
				Open();
			}
		}
	}

	public void UpdateSlots() {
		var gridContainer = GetNode<GridContainer>("NinePatchRect/GridContainer");
		for (int i = 0; i < Math.Min(inv.item.Count, slots.Count); i++)
		{
			slots[i].update(inv.item[i]);
		}
	}

	public void Open() {
		Visible = true;
		is_open = true;
	}

	public void Close() {
		Visible = false;
		is_open = false;
	}



}
