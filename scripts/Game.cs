using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Game : Node2D
{
	private ApiController _apiHandler;
	private MainCharacter _player;
	private PackedScene enemigoPack;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_apiHandler = GetNode<ApiController>("ApiController");
		_player = GetNode<MainCharacter>("CharacterBody2D");

		_apiHandler.GetCharacterData(1);
	}
	
	public void ApplyCharacterData(System.Collections.Generic.Dictionary<string, object> data)
	{
		_player.name = data["name"].ToString();
		_player.MaxHp = Convert.ToInt32(data["maxHp"]);
		_player.Hp = Convert.ToInt32(data["hp"]);

		var positionArray = (Newtonsoft.Json.Linq.JArray)data["position"];
		_player.Position = new Vector2((float)positionArray[0], (float)positionArray[1]);

		// Limpiar inventario antes de asignar nuevos objetos
		_player.inventory.item.Clear();

		// Procesar inventario
		var inventoryArray = (Newtonsoft.Json.Linq.JArray)data["inventory"];
		foreach (var itemData in inventoryArray)
		{
			var itemDict = itemData.ToObject<System.Collections.Generic.Dictionary<string, string>>();
			var item = new InventoryItem
			{
				name = itemDict["name"],
				texture = GD.Load<Texture2D>(itemDict["texture"]) // Cargar la textura desde la ruta
			};

			_player.inventory.item.Add(item);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
