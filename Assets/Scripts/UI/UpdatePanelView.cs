using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static Core.Constants;

namespace Game.UI
{
    public class UpdatePanelView : PopupView
	{
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _textError;

        [SerializeField] private TMP_InputField _input;

        [SerializeField] private Toggle _toggleIsAnimated;


        [Inject]
        public override void Construct()
        {
			_type = UIType.Update;

            _uiController.AddItemUI(DropdownColors, new ItemUI(_dropdown));
            _uiController.AddItemUI(DropdownColors + _type, new ItemUI(_dropdown));
           
            _uiController.AddItemUI(TextCountCards + _type, new ItemUI(_text));
            _uiController.AddItemUI(ErrorText + _type, new ItemUI(_textError));

            _uiController.AddItemUI(InputCountCard + _type, new ItemUI(_input));

            _uiController.AddItemUI(IsAnimatedCard + _type, new ItemUI(_toggleIsAnimated));

            base.Construct();
        }
    }
}