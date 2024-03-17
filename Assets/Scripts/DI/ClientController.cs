using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardsSystem;
using Game.UI;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using VContainer;
using VContainer.Unity;
using static Core.Constants;
using static UnityEditor.Progress;

namespace NetworkSystem
{
	public class ClientController : IStartable
	{
		[Inject] private UIController _uiController;
		[Inject] private CardsController _cardsController;
		[Inject] private PopupController _popupController;

		public void Start()
		{
			_uiController.SetAction(UIType.Create.ToString(), OnClickCreate);
			_uiController.SetAction(UIType.Delete.ToString(), OnClickDelete);
			_uiController.SetAction(UIType.Update.ToString(), OnClickUpdate);
			_uiController.SetAction(UIType.Refresh.ToString(), OnClickRefresh);
		}

		/// <summary>
		/// Refresh
		/// </summary>

		public async void OnClickRefresh()
		{
			_popupController.ActivePopup(UIType.Refresh, true);
			var input = _uiController.GetInputField(InputCountCard + UIType.Refresh);
			var toggle = _uiController.GetToggle(IsRefreshAllCards + UIType.Refresh);

			List<CardItem> cards = await WebRequestGetAll();
			WriteNumsCards(cards, UIType.Refresh);

			UnityAction<bool, string> refresh = RefreshCard;
			_uiController.SetAction(BtnOkPopup + UIType.Refresh, () => refresh(toggle.isOn, input.text));
		}

		private async void RefreshCard(bool isRefreshAll, string num)
		{
			List<CardItem> cards = await WebRequestGetAll();
			if (isRefreshAll)
			{
				_cardsController.UpdateCards(cards);
			}
			else
			{
				var card = cards.Find(x => x.id == int.Parse(num));
				if (card.IsUnityNull())
				{
					_uiController.SetText(ErrorText + UIType.Refresh,
						"The card with this number does not exist, please enter a valid number from the list.");
				}
				else
				{
					bool value = _cardsController.UpdateCard(card);
					if (value)
					{
						_popupController.ActivePopup(UIType.Refresh, false);
					}
					else
					{
						_uiController.SetText(ErrorText + UIType.Delete,
							"The card with this number does not exist, please enter a valid number from the list.");
					}
				}
			}
		}
		/// <summary>
		/// Update
		/// </summary>
		private async void OnClickUpdate()
		{
			_popupController.ActivePopup(UIType.Update, true);
			var input = _uiController.GetInputField(InputCountCard + UIType.Update);
			var toggle = _uiController.GetToggle(IsAnimatedCard + UIType.Update);
			var drop = _uiController.GetDropdown(DropdownColors + UIType.Update);

			List<CardItem> cards = await WebRequestGetAll();
			WriteNumsCards(cards, UIType.Update);

			UnityAction<string, bool, string> update = UpdateCard;
			_uiController.SetAction(BtnOkPopup + UIType.Update, () => update(drop.captionText.text, toggle.isOn, input.text));
		}

		private async void UpdateCard(string colorType, bool isAnimated, string num)
		{
			bool value = await WebRequestPut(colorType, isAnimated, num);

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
				_uiController.SetText(ErrorText + UIType.Update,
					"The card with this number does not exist, please enter a valid number from the list.");
			}
		}

		private async Task<bool> WebRequestPut(string colorType, bool isAnimated, string num)
		{
			CardItem item = new CardItem()
			{
				colorType = colorType,
				isAnimated = isAnimated
			};
			string json = JsonUtility.ToJson(item);

			UnityWebRequest request = UnityWebRequest.Put(URL + "/" + num, json);
			request.SetRequestHeader("Content-Type", "application/json");
			await request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Create
		/// </summary>
		private void OnClickCreate()
		{
			var toggle = _uiController.GetToggle(IsAnimatedCard + UIType.Create);
			var drop = _uiController.GetDropdown(DropdownColors + UIType.Create);

			_uiController.SetAction(BtnOkPopup + UIType.Create, () => WebRequestPost(drop.captionText.text, toggle.isOn));
			_popupController.ActivePopup(UIType.Create, true);
		}

		private async void WebRequestPost(string colorType, bool isAnimated)
		{
			WWWForm form = new WWWForm();
			CardItem item = new CardItem()
			{
				colorType = colorType,
				isAnimated = isAnimated
			};

			string json = JsonUtility.ToJson(item);
			byte[] post = Encoding.UTF8.GetBytes(json);
			UnityWebRequest request = UnityWebRequest.Post(URL, form);
			UploadHandler uploadHandler = new UploadHandlerRaw(post);
			request.uploadHandler = uploadHandler;
			request.SetRequestHeader("Content-Type", "application/json");
			_popupController.ActivePopup(UIType.Create, false);

			await request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
			}
			else
			{
				var card = JsonUtility.FromJson<CardItem>(request.downloadHandler.text);
				_cardsController.Spawn(new CardItem() { id = card.id, colorType = colorType, isAnimated = isAnimated });
			}
		}

		/// <summary>
		/// Delete
		/// </summary>
		private async void OnClickDelete()
		{
			_popupController.ActivePopup(UIType.Delete, true);
			List<CardItem> cards = await WebRequestGetAll();
			WriteNumsCards(cards, UIType.Delete);

			UnityAction<string> delete = DeleteCard;
			_uiController.SetAction(BtnOkPopup + UIType.Delete, delete);
		}

		private async void DeleteCard(string num)
		{
			bool value = await WebRequestDetele(num);

			if (value)
			{
				_cardsController.Despawn(int.Parse(num));
				_popupController.ActivePopup(UIType.Delete, false);
			}
			else
			{
				_uiController.SetText(ErrorText + UIType.Delete,
					"The card with this number does not exist, please enter a valid number from the list.");
			}
		}

		private async Task<bool> WebRequestDetele(string num)
		{
			UnityWebRequest request = UnityWebRequest.Delete(URL + "/" + num);
			await request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError) return false;
			return true;
		}

		/// <summary>
		/// Other
		/// </summary>
		/// <param name="cards"></param>
		/// <param name="type"></param>
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

		private async Task<List<CardItem>> WebRequestGetAll()
		{
			UnityWebRequest request = UnityWebRequest.Get(URL);
			await request.SendWebRequest();
			return JsonConvert.DeserializeObject<IEnumerable<CardItem>>(request.downloadHandler.text).ToList();
		}

		private async void WebRequestGet(int num)
		{
			UnityWebRequest request = UnityWebRequest.Get(URL);
			await request.SendWebRequest();
			var req = JsonConvert.DeserializeObject<IEnumerable<CardItem>>(request.downloadHandler.text).ToList();
			var item = req[num];
			// _cardsController.Spawn(new CardItem() { colorType = item.colorType, isAnimated = item.isAnimated });
		}
	}
}