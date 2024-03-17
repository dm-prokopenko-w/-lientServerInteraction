using TMPro;
using UnityEngine;
using VContainer;
using static Core.Constants;

namespace Game.UI
{
    public class DeletePanelView : PopupView
	{
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _textError;
        [SerializeField] private TMP_InputField _input;

        [Inject]
        public override void Construct()
        {
			_type = UIType.Delete;

			_uiController.AddItemUI(TextCountCards + _type, new ItemUI(_text));
			_uiController.AddItemUI(ErrorText + _type, new ItemUI(_textError));

			_uiController.AddItemUI(InputCountCard + _type, new ItemUI(_input));

			base.Construct();
		}
	}
}
