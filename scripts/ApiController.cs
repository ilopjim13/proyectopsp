using Godot;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

public partial class ApiController : Node
{
	private HttpRequest _httpRequest;
	private string _requestType;
	private Game _game;

	public override void _Ready()
	{
		_httpRequest = GetNode<HttpRequest>("../ApiRequest");
		_httpRequest.Connect("request_completed", new Callable(this, nameof(OnRequestCompleted)));

		_game = GetParent() as Game;
	}

	// Obtener datos del personaje
	public void GetCharacterData(int characterId)
	{
		string url = $"https://pruebaapipsp.onrender.com/api/Players/{characterId}";
		string[] headers = { "Content-Type: application/json" };

		_requestType = "GET_CHARACTER";
		Error error = _httpRequest.Request(url, headers);

		if (error != Error.Ok){
			GD.PrintErr("Error en la solicitud HTTP: ", error);
		} else {
			GD.Print("Jugador obtenido: ", error);
		}
	}

	// Guardar datos del personaje
	public void SaveCharacterData(MainCharacter player)
	{
		string url = "https://pruebaapipsp.onrender.com/api/Players/save";
		string[] headers = { "Content-Type: application/json" };

		var inventoryData = new List<Dictionary<string, string>>();

		foreach (InventoryItem item in player.inventory.item)
		{
			if(item != null) {
				inventoryData.Add(new Dictionary<string, string>
				{
					{ "name", item.name },
					{ "texture", item.texture.ResourcePath } 
				});
			}
		}

		var characterData = new Dictionary<string, object>
		{
			{ "id", 1 },
			{ "name", player.name },
			{ "maxHp", player.MaxHp },
			{ "hp", player.Hp },
			{ "position", new float[] { player.Position.X, player.Position.Y } },
			{ "inventory", inventoryData }
		};

		string body = JsonConvert.SerializeObject(characterData);
		GD.Print(body);

		_requestType = "SAVE_CHARACTER";
		
		Error error = _httpRequest.Request(url, headers, Godot.HttpClient.Method.Post, body);
		if (error != Error.Ok)
			GD.PrintErr("Error al enviar datos: ", error);
	}
	
	// Guardar datos del personaje
	public void UpdateCharacter(MainCharacter player)
	{
		string url = "https://pruebaapipsp.onrender.com/api/Players/save/1";
		string[] headers = { "Content-Type: application/json" };

		var inventoryData = new List<Dictionary<string, string>>();

		foreach (InventoryItem item in player.inventory.item)
		{
			if(item != null) {
				inventoryData.Add(new Dictionary<string, string>
				{
					{ "name", item.name },
					{ "texture", item.texture.ResourcePath } 
				});
			}
		}

		var characterData = new Dictionary<string, object>
		{
			{ "id", 1 },
			{ "name", player.name },
			{ "maxHp", player.MaxHp },
			{ "hp", player.Hp },
			{ "position", new float[] { player.Position.X, player.Position.Y } },
			{ "inventory", inventoryData }
		};

		string body = JsonConvert.SerializeObject(characterData);
		GD.Print(body);

		_requestType = "Update_CHARACTER";
		
		Error error = _httpRequest.Request(url, headers, Godot.HttpClient.Method.Put, body);
		if (error != Error.Ok)
			GD.PrintErr("Error al enviar datos: ", error);
	}

	// Manejar la respuesta de la API
	private void OnRequestCompleted(int result, int responseCode, string[] headers, byte[] body)
	{
		if (responseCode == 200)
		{
			string jsonString = System.Text.Encoding.UTF8.GetString(body);
			var jsonData = JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, object>>(jsonString);

			if (jsonData != null)
			{
				if (_requestType == "GET_CHARACTER")
					_game.isExist= true;
					_game.ApplyCharacterData(jsonData);
			}
			else
			{
				GD.PrintErr("Error al analizar la respuesta JSON");
			}
		}
		else
		{
			GD.PrintErr("Error en la respuesta HTTP: ", responseCode);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
