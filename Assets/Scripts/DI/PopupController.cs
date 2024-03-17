using CardsSystem;
using System;
using VContainer;
using static Core.Constants;

namespace Game.UI
{
    public class PopupController
    {
        [Inject] private UIController _uiController;
        [Inject] private CardsController _cardsController;

        public Action<bool> UpdateViewPopup;

        public void ActivePopup(UIType type, bool value)
        {
            if (value)
            {
                _uiController.SetAction(BtnClosePopup + type, () => ActivePopup(type, false));
            }
            
            _uiController.SetAnimPlay(PopupAnim + type, value ? ShowKey : HideKey);
            _uiController.SetAnimPlay(PanelMainView, !value ? ShowKey : HideKey);
            _cardsController.ActiveCards(!value);
            UpdateViewPopup?.Invoke(value);
            _uiController.SetText(ErrorText + type, "");
        }
    }
}