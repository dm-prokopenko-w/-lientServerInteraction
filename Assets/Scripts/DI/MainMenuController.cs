using System.Collections.Generic;
using static Core.Constants;
using VContainer;
using VContainer.Unity;
using NetworkSystem;
using CardsSystem;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace Game.UI
{
	public class MainMenuController : IStartable, IDisposable
	{
		[Inject] private UIController _uiController;
		[Inject] private ClientController _clientController;
		[Inject] private CardsController _cardsController;
		[Inject] private PopupController _popupController;

		private bool _isClikedDelete = false;

		public void Start()
		{
			_uiController.SetAction(UIType.Create.ToString(), OnClickCreate);
			_uiController.SetAction(UIType.Delete.ToString(), OnClickDelete);
			_uiController.SetAction(UIType.Update.ToString(), OnClickUpdate);
			_uiController.SetAction(UIType.Refresh.ToString(), OnClickRefresh);

			_cardsController.OnUpdateCountCards += (count) =>
			{
				_isClikedDelete = (count > 0);
				_uiController.SetInteractableBtn(UIType.Delete.ToString(), _isClikedDelete);
			};
			_popupController.UpdateViewPopup += (bool value) => UpdateInteractable(!value);
			UpdateInteractable(true);
		}

		private void OnClickCreate()
		{
			var toggle = _uiController.GetToggle(IsAnimatedCard + UIType.Create);
			var drop = _uiController.GetDropdown(DropdownColors + UIType.Create);

			UnityAction<string, bool> create = new(async (colorType, isAnimated) =>
			{
				_popupController.ActivePopup(UIType.Create, false);

				string json = await _clientController.WebRequestPost(drop.captionText.text, toggle.isOn);
				if (json != null)
				{
					var card = JsonUtility.FromJson<CardItem>(json);
					_cardsController.Spawn(new CardItem() { id = card.id, colorType = colorType, isAnimated = isAnimated });
				}
			});

			_uiController.SetAction(BtnOkPopup + UIType.Create, () => create(drop.captionText.text, toggle.isOn));
			_popupController.ActivePopup(UIType.Create, true);
		}

		private async void OnClickDelete()
		{
			_popupController.ActivePopup(UIType.Delete, true);
			List<CardItem> cards = await _clientController.WebRequestGetAll();
			if (cards == null)
			{
				_uiController.SetText(ErrorText + UIType.Delete, Error);
				return;
			}

			WriteNumsCards(cards, UIType.Delete);
			var input = _uiController.GetInputField(InputCountCard + UIType.Delete);
			UnityAction<string> delete = new(async (num) =>
			{
				bool value = await _clientController.WebRequestDetele(num);
				if (value)
				{
					_cardsController.Despawn(int.Parse(num));
					_popupController.ActivePopup(UIType.Delete, false);
				}
				else
				{
					_uiController.SetText(ErrorText + UIType.Delete, Error);
				}
			});
			_uiController.SetAction(BtnOkPopup + UIType.Delete, () => delete(input.text));
		}

		private async void OnClickUpdate()
		{
			_popupController.ActivePopup(UIType.Update, true);
			var input = _uiController.GetInputField(InputCountCard + UIType.Update);
			var toggle = _uiController.GetToggle(IsAnimatedCard + UIType.Update);
			var drop = _uiController.GetDropdown(DropdownColors + UIType.Update);

			List<CardItem> cards = await _clientController.WebRequestGetAll();

			if (cards == null)
			{
				_uiController.SetText(ErrorText + UIType.Update, Error);
				return;
			}

			WriteNumsCards(cards, UIType.Update);

			UnityAction<string, bool, string> update = new(async (colorType, isAnimated, num) =>
			{
				bool value = await _clientController.WebRequestPut(colorType, isAnimated, num);

				if (value)
				{
					CardItem item = new CardItem()
					{
						colorType = colorType,
						isAnimated = isAnimated,
						id = int.Parse(num)
					};

					_cardsController.UpdateCard(item);
					_popupController.ActivePopup(UIType.Update, false);
				}
				else
				{
					_uiController.SetText(ErrorText + UIType.Update, Error);
				}
			});
			_uiController.SetAction(BtnOkPopup + UIType.Update, () => update(drop.captionText.text, toggle.isOn, input.text));
		}

		public async void OnClickRefresh()
		{
			_popupController.ActivePopup(UIType.Refresh, true);
			var input = _uiController.GetInputField(InputCountCard + UIType.Refresh);
			var toggle = _uiController.GetToggle(IsRefreshAllCards + UIType.Refresh);

			List<CardItem> cards = await _clientController.WebRequestGetAll();
			if (cards == null)
			{
				_uiController.SetText(ErrorText + UIType.Refresh, Error);
				return;
			}

			WriteNumsCards(cards, UIType.Refresh);

			UnityAction<bool, string> refresh = new(async (isRefreshAll, num) =>
			{
				if (isRefreshAll)
				{
					List<CardItem> cards = await _clientController.WebRequestGetAll();
					if (cards == null)
					{
						_uiController.SetText(ErrorText + UIType.Refresh, Error);
						return;
					}

					_cardsController.UpdateCards(cards);
					_popupController.ActivePopup(UIType.Refresh, false);
				}
				else
				{
					var card = await _clientController.WebRequestGet(int.Parse(num));
					if (card == null)
					{
						_uiController.SetText(ErrorText + UIType.Refresh, Error);
					}
					else
					{
						bool isUpdated = _cardsController.UpdateCard(card);
						if (isUpdated)
						{
							_popupController.ActivePopup(UIType.Refresh, false);
						}
						else
						{
							_uiController.SetText(ErrorText + UIType.Refresh, Error);
						}
					}
				}

			});
			_uiController.SetAction(BtnOkPopup + UIType.Refresh, () => refresh(toggle.isOn, input.text));
		}

		public void UpdateInteractable(bool value)
		{
			_uiController.SetInteractableBtn(UIType.Create.ToString(), value);

			if (_isClikedDelete)
			{
				_uiController.SetInteractableBtn(UIType.Delete.ToString(), value);
			}
			else
			{
				_uiController.SetInteractableBtn(UIType.Delete.ToString(), false);
			}

			_uiController.SetInteractableBtn(UIType.Update.ToString(), value);
			_uiController.SetInteractableBtn(UIType.Refresh.ToString(), value);
		}

		public void Dispose()
		{
			_cardsController.OnUpdateCountCards -= (count) =>
			{
				_isClikedDelete = (count > 0);
				_uiController.SetInteractableBtn(UIType.Delete.ToString(), _isClikedDelete);
			};
			_popupController.UpdateViewPopup -= (bool value) => UpdateInteractable(!value);
		}

		private void WriteNumsCards(List<CardItem> cards, UIType type)
		{
			string nums = "";
			if (cards.Count > 0)
			{
				nums = "\n" + cards[0].id.ToString();
				for (int i = 1; i < cards.Count; i++)
				{
					nums += " - " + cards[i].id;
				}
			}

			_uiController.SetText(TextCountCards + type, NumsCards + nums);
		}
	}
}
