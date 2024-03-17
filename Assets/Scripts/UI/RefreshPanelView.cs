using UnityEngine;
using TMPro;
using UnityEngine.UI;
using VContainer;
using static Core.Constants;

namespace Game.UI
{
	public class RefreshPanelView : PopupView
	{
		[SerializeField] private TMP_Text _text;
		[SerializeField] private TMP_Text _textError;

		[SerializeField] private TMP_InputField _input;

		[SerializeField] private Toggle _toggleIsRefreshAll;
		[SerializeField] private GameObject _inputParent;

		[Inject]
		public override void Construct()
		{
			_type = UIType.Refresh;

			_uiController.AddItemUI(TextCountCards + _type, new ItemUI(_text));
			_uiController.AddItemUI(ErrorText + _type, new ItemUI(_textError));

			_uiController.AddItemUI(InputCountCard + _type, new ItemUI(_input));

			_uiController.AddItemUI(IsRefreshAllCards + _type, new ItemUI(_toggleIsRefreshAll));

			_toggleIsRefreshAll.onValueChanged.AddListener((bool value) => _inputParent.SetActive(!value));
			base.Construct();
		}
	}
}
