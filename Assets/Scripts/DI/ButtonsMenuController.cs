using System;
using CardsSystem;
using NetworkSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using static Core.Constants;

namespace Game.UI
{
    public class ButtonsMenuController: IStartable, IDisposable
    {
        [Inject] private UIController _uiController;
        [Inject] private CardsController _cardsController;

        private bool _isClikedDelete = false;
        
        public void Start()
        {
            _cardsController.OnUpdateCountCards += (count) =>
            {
                _isClikedDelete = (count > 0);
                _uiController.SetInteractableBtn(UIType.Delete.ToString(), _isClikedDelete);
            };
            UpdateInteractable(true);
        }

        public void UpdateInteractable(bool value)
        {
            _uiController.SetInteractableBtn(UIType.Create.ToString(), value);

            if (_isClikedDelete)
            {
                _uiController.SetInteractableBtn(UIType.Delete.ToString(), value);
            }
            else
            {
                _uiController.SetInteractableBtn(UIType.Delete.ToString(), false);
            }
            
            _uiController.SetInteractableBtn(UIType.Update.ToString(), value);
            _uiController.SetInteractableBtn(UIType.Refresh.ToString(), value);
        }

        public void Dispose()
        {
            _cardsController.OnUpdateCountCards -= (count) =>
            {
                _isClikedDelete = (count > 0);
                _uiController.SetInteractableBtn(UIType.Delete.ToString(), _isClikedDelete);
            };
        }
    }
}
