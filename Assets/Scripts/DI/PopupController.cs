using CardsSystem;
using VContainer;
using static Core.Constants;

namespace Game.UI
{
    public class PopupController
    {
        [Inject] private UIController _uiController;
        [Inject] private CardsController _cardsController;
        [Inject] private ButtonsMenuController _buttonsMenuController;
        
        public void ActivePopup(UIType type, bool value)
        {
            if (value)
            {
                _uiController.SetAction(BtnClosePopup + type, () => ActivePopup(type, false));
            }
            
            _uiController.SetAnimPlay(Popup + type, value ? ShowKey : HideKey);
            _uiController.SetAnimPlay(PanelMainView, !value ? ShowKey : HideKey);
            _cardsController.ActiveCards(!value);
            _buttonsMenuController.UpdateInteractable(!value);
            _uiController.SetText(ErrorText + type, "");
        }
    }
}