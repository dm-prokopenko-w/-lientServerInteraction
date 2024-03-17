using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardsSystem;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using static Core.Constants;

namespace NetworkSystem
{
	public class ClientController
	{
		public async Task<bool> WebRequestPut(string colorType, bool isAnimated, string num)
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

		public async Task<string> WebRequestPost(string colorType, bool isAnimated)
		{
			WWWForm form = new WWWForm();
			CardItem item = new()
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

			await request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
				return null;
			}
			return request.downloadHandler.text;
		}

		public async Task<bool> WebRequestDetele(string num)
		{
			UnityWebRequest request = UnityWebRequest.Delete(URL + "/" + num);
			await request.SendWebRequest();
			if (request.isNetworkError || request.isHttpError) return false;
			return true;
		}

		public async Task<List<CardItem>> WebRequestGetAll()
		{
			UnityWebRequest request = UnityWebRequest.Get(URL);
			await request.SendWebRequest();
			if (request.isNetworkError || request.isHttpError) return null;
			return JsonConvert.DeserializeObject<IEnumerable<CardItem>>(request.downloadHandler.text).ToList();
		}

		public async Task<CardItem> WebRequestGet(int num)
		{
			UnityWebRequest request = UnityWebRequest.Get(URL);
			await request.SendWebRequest();
			if (request.isNetworkError || request.isHttpError) return null;
			var req = JsonConvert.DeserializeObject<IEnumerable<CardItem>>(request.downloadHandler.text).ToList();
			var card = req.Find(x => x.id == num);
			if (card == null) return null;
			return card;
		}
	}
}