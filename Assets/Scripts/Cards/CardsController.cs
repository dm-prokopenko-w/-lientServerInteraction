using System;
using Core;
using System.Collections.Generic;
using Game.UI;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CardsSystem
{
	public class CardsController : IStartable
	{
		[Inject] private AssetLoader _assetLoader;
		[Inject] private UIController _uiController;

		public Action<int> OnUpdateCountCards;
		private Dictionary<string, CardView> _cardsType = new ();
		private List<CardView> _activeCards = new ();
		
		private ObjectPool<CardView> _pool;
		private Transform _parent;
		
		public async void Start()
		{
			var data = await _assetLoader.LoadConfig(Constants.CardsConfigPath) as CardsConfig;
			
			InitCards(data.Cards);
			InitColorsDropdownOptions(data.Cards);
		}

		private void InitCards(List<CardData> cards)
		{
			var _inactiveTr = _uiController.GetTransform(Constants.InactiveCards);
			_parent = _uiController.GetTransform(Constants.ActiveCards);
			_pool = new ObjectPool<CardView>();

			foreach (var item in cards)
			{
				_pool.InitPool(item.Prefab, _inactiveTr);
				_cardsType.Add(item.Id, item.Prefab);
			}
		}
		
		private void InitColorsDropdownOptions(List<CardData> cards)
		{
			var drops = _uiController.GetDropdown(Constants.DropdownColors);

			if (drops == null)
			{
				Debug.LogError("Check the dropdowns for colors.");
				return;
			}
			
			foreach (var drop in drops)
			{
				drop.options.Clear();
				foreach (var item in cards)
				{
					drop.options.Add(new TMP_Dropdown.OptionData(item.Id));
				}
				drop.value = 1;
			}
		}
		
		public void UpdateCards(List<CardItem> cards)
		{
			ActiveCards(false);
			foreach (var card in cards)
			{
				Spawn(card);
			}
		}
		
		public void Spawn(CardItem item)
		{
			if (_cardsType.TryGetValue(item.colorType, out CardView prefab))
			{
				var view = _pool.Spawn(prefab, _parent);
				view.Init(item.id, item.isAnimated);
				_activeCards.Add(view);
				OnUpdateCountCards?.Invoke(_activeCards.Count);
			}
			else
			{
				Debug.LogError("The data does not match, check the database and config.");
			}
		}

		public void Despawn(int num)
		{
			var view = _activeCards.Find(x => x.Id == num);
			if(view == null) return;
			_pool.Despawn(view);
			_activeCards.Remove(view);
			OnUpdateCountCards?.Invoke(_activeCards.Count);
		}
		
		public void ActiveCards(bool value)
		{
			foreach (var view in _activeCards)
			{
				view.ActiveEffect(value);
			}
		}
	}

	[Serializable]
	public struct CardItem
	{
		public bool isAnimated;
		public string colorType;
		public int id;
	}
}
