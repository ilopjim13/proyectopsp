using Godot;
using System;

[Tool]
public partial class Inventory : Resource
{
	[Export]
	public Godot.Collections.Array<Inventory_item> item = new Godot.Collections.Array<Inventory_item>();
	
}
