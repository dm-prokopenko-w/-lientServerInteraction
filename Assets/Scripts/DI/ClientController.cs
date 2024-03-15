using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardsSystem;
using Game.UI;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using VContainer;
using VContainer.Unity;
using static Core.Constants;

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
        
        public async void OnClickRefresh()
        {
            List<CardItem> cards = await WebRequestGetAll();
            _cardsController.UpdateCards(cards);
        }

        /// <summary>
        /// Refresh
        /// </summary>


        /// <summary>
        /// Update
        /// </summary>
        private async void OnClickUpdate()
        {
            _uiController.SetAction(PanelView + UIType.Update, WebRequestPut, true);
            _popupController.ActivePopup(UIType.Update, true);

            /*
            _popupController.ActivePopup(UIType.Update, true);
            List<CardItem> cards = await WebRequestGetAll();
            WriteNumsCards(cards, UIType.Update);

            UnityAction<string> delete = UpdateCard;
            _uiController.SetAction(PanelView + UIType.Update, delete);
            */
        }
        
        private async void UpdateCard(string num)
        {
            //bool value = await WebRequestPut(num);

            // if (value)
            {
                _cardsController.Despawn(int.Parse(num));
                _popupController.ActivePopup(UIType.Delete, false);
            }
            //  else
            {
                _uiController.SetText(ErrorText + UIType.Delete,
                    "The card with this number does not exist, please enter a valid number from the list.");
            }
        }

        private async void WebRequestPut(string colorType, bool isAnimated, string num)
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
            }
            else
            {
                // _cardsController.Spawn(new CardItem() { colorType = colorType, isAnimated = isAnimated });
                _popupController.ActivePopup(UIType.Create, false);
            }
        }

        /// <summary>
        /// Create
        /// </summary>
        private void OnClickCreate()
        {
            _uiController.SetAction(PanelView + UIType.Create, WebRequestPost, true);
            _popupController.ActivePopup(UIType.Create, true);
        }

        private async void WebRequestPost(string colorType, bool isAnimated, string num)
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
            _uiController.SetAction(PanelView + UIType.Delete, delete);
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