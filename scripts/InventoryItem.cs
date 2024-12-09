using Godot;
using System;

[GlobalClass]
public partial class InventoryItem : Resource
{
	[Export]
	public String name = "";
	[Export] 
	public Texture2D texture;
}
