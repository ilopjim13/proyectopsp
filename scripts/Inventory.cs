using Godot;
using System;

[GlobalClass]
public partial class Inventory : Resource
{
	[Export]
	public Godot.Collections.Array<InventoryItem> item = new Godot.Collections.Array<InventoryItem>();
	
}
