using Godot;

[Tool]
public partial class InventoryManager : Node
{
	public override void _Ready()
	{
		Inventory inventory = new Inventory();
		ResourceSaver.Save(inventory, "res://inventory/inventory.tres");
	}
}
