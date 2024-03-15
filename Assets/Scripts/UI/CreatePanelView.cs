using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static Core.Constants;

namespace Game.UI
{
    public class CreatePanelView : MonoBehaviour
    {
        [Inject] private UIController _uiController;

        [SerializeField] private UIType _type;
        [SerializeField] private Button _buttonOk;
        [SerializeField] private Toggle _toggleIsAnimated;
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private Animator _anim;
        [SerializeField] private Button _buttonCancel;

        [Inject]
        public void Construct()
        {
            _uiController.AddItemUI(Popup + _type, new ItemUI(_anim));
            _uiController.AddItemUI(DropdownColors, new ItemUI(_dropdown));
            _uiController.AddItemUI(PanelView + _type, new ItemUI(_buttonOk, _toggleIsAnimated, _dropdown, null));
            _uiController.AddItemUI(BtnClosePopup + _type, new ItemUI(_buttonCancel));
        }
    }
}