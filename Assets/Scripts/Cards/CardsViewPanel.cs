using static Core.Constants;
using Game.UI;
using UnityEngine;
using VContainer;

namespace CardsSystem
{
    public class CardsViewPanel : MonoBehaviour
    {
        [Inject] private UIController _uiController;
    
        [SerializeField] private Transform _inactiveCards;
        [SerializeField] private Transform _activeCards;
        [SerializeField] private Animator _anim;

        [Inject]
        public void Construct()
        {
            _uiController.AddItemUI(ActiveCards, new ItemUI(_activeCards));
            _uiController.AddItemUI(InactiveCards, new ItemUI(_inactiveCards));
            _uiController.AddItemUI(PanelMainView, new ItemUI(_anim));
        }

}
}
