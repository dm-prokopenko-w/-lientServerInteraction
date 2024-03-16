using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
	public class UIController
	{
		private Dictionary<string, List<ItemUI>> _items = new ();

		public void AddItemUI(string id, ItemUI item)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				items.Add(item);
			}
			else
			{
				List<ItemUI> newItems = new List<ItemUI> { item };
				_items.Add(id, newItems);
			}
		}
		
		public List<TMP_Dropdown> GetDropdown(string id)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				List<TMP_Dropdown> drops = new();
				foreach (var item in items)
				{
					if (item.Drop == null) continue;
					
					drops.Add(item.Drop);
				}

				return drops;
			}

			return null;
		}

		public Transform GetTransform(string id)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				foreach (var item in items)
				{
					if (item.Tr == null) continue;
					return item.Tr;
				}

				return null;
			}

			return null;
		}
		
		public void SetAction(string id, UnityAction<string> func)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				foreach (var item in items)
				{
					if (item.Input == null) continue;
					item.Btn.onClick.AddListener(() => func(item.Input.text));
				}
			}
		}

		public void SetAction(string id, UnityAction func)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				foreach (var item in items)
				{
					if (item.Btn == null) continue;
					item.Btn.onClick.AddListener(func);
				}
			}
		}
		
		public void SetAction(string id, UnityAction<string, bool, string> func, bool isResetBtn = false)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				foreach (var item in items)
				{ 
					if (isResetBtn) item.Btn.onClick.RemoveAllListeners();
					if (item.Input == null)
					{
						item.Btn.onClick.AddListener(() => func(item.Drop.captionText.text, item.Tgl.isOn, null));
					}
					else if (item.Drop == null)
					{
						item.Btn.onClick.AddListener(() => func(null, item.Tgl.isOn, item.Input.text));
					}
					else
					{
						item.Btn.onClick.AddListener(() => func(item.Drop.captionText.text, item.Tgl.isOn, item.Input.text));
					}
				}
			}
		}

		public void SetInteractableBtn(string id, bool value)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				foreach (var item in items)
				{
					if (item.Btn == null) continue;
					item.Btn.interactable = value;
				}
			}
		}
		
		public void SetText(string id, string text)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				foreach (var item in items)
				{
					if (item.Text == null) continue;
					item.Text.text = text;
				}
			}
		}

		public void SetAnimPlay(string id, string animId)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				foreach (var item in items)
				{
					if (item.Anim == null) continue;
					item.Anim.Play(animId);
				}
			}
		}
	}

	public class ItemUI
	{
		public Button Btn;
		public TMP_Dropdown Drop;
		public Toggle Tgl;
		public Transform Tr;
		public Animator Anim;
		public TMP_Text Text;
		public TMP_InputField Input;

		public ItemUI(Button btn)
		{
			Btn = btn;
		}
		
		public ItemUI(TMP_Dropdown drop)
		{
			Drop = drop;
		}

		public ItemUI(Button btn, Toggle toggle, TMP_Dropdown drop, TMP_InputField input)
		{
			Btn = btn;
			Tgl = toggle;
			Drop = drop;
			Input = input;
		}

		public ItemUI(Animator anim)
		{
			Anim = anim;
		}
		
		public ItemUI(TMP_Text text)
		{
			Text = text;
		}
		
		public ItemUI(Button btn, TMP_InputField input)
		{
			Btn = btn;
			Input = input;
		}
		
		public ItemUI(Transform tr)
		{
			Tr = tr;
		}
	}
}
