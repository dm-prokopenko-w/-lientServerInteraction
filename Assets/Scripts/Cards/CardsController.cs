using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardsSystem
{
	public class CardsController : IStartable
	{
		[Inject] private AssetLoader _assetLoader;

		private Dictionary<string, GameObject> _cards = new Dictionary<string, GameObject>();

		public async void Start()
		{
			var data = await _assetLoader.LoadConfig(Constants.CardsConfigPath) as CardsConfig;

			foreach (var item in data.Cards)
			{
				_cards.Add(item.Id, item.Prefab);
			}
		}

		public void Spawn(string type)
		{
			if (_cards.TryGetValue(type, out GameObject prefab))
			{

			}
			else
			{
				Debug.LogError("The data does not match, check the database and config.");
			}
		}

	}
}
