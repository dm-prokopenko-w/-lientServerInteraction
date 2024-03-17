using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static Core.Constants;

namespace Game.UI
{
    public class CreatePanelView : PopupView
	{
        [SerializeField] private Toggle _toggleIsAnimated;
        [SerializeField] private TMP_Dropdown _dropdown;

        [Inject]
        public override void Construct()
        {
			_type = UIType.Create;

			_uiController.AddItemUI(IsAnimatedCard + _type, new ItemUI(_toggleIsAnimated));

			_uiController.AddItemUI(DropdownColors, new ItemUI(_dropdown));
			_uiController.AddItemUI(DropdownColors + _type, new ItemUI(_dropdown));
			base.Construct();
		}
	}
}