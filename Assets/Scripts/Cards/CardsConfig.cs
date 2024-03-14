using Game.Configs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardsSystem
{
	[CreateAssetMenu(fileName = "CardsConfig", menuName = "Configs/CardsConfig", order = 0)]
	public class CardsConfig : Config
	{
		public List<CardData> Cards;
	}

	[Serializable]
	public class CardData
	{
		public string Id;
		public GameObject Prefab;
	}
}
