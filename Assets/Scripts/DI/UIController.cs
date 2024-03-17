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
		
		public List<TMP_Dropdown> GetDropdowns(string id)
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

		public TMP_Dropdown GetDropdown(string id)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				foreach (var item in items)
				{
					if (item.Drop == null) continue;
					return item.Drop;
				}
			}

			return null;
		}

		public TMP_InputField GetInputField(string id)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				foreach (var item in items)
				{
					if (item.Input == null) continue;
					return item.Input;
				}
			}

			return null;
		}

		public Toggle GetToggle(string id)
		{
			if (_items.TryGetValue(id, out List<ItemUI> items))
			{
				List<Toggle> drops = new();
				foreach (var item in items)
				{
					if (item.Tgl == null) continue;
					return item.Tgl;
				}
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
					item.Btn.onClick.RemoveAllListeners();
					item.Btn.onClick.AddListener(func);
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

		public ItemUI(Animator anim)
		{
			Anim = anim;
		}
		
		public ItemUI(TMP_Text text)
		{
			Text = text;
		}
		
		public ItemUI(Toggle toggle)
		{
			Tgl = toggle;
		}
		
		public ItemUI(TMP_InputField input)
		{
			Input = input;
		}
		
		public ItemUI(Transform tr)
		{
			Tr = tr;
		}
	}
}
