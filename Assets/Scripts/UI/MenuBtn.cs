using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static Core.Constants;

namespace Game.UI
{
    public class MenuBtn : MonoBehaviour
    {
        [SerializeField] private UIType _type;
        
        [Inject] private UIController _uiController;

        [SerializeField] private Button _button;

        [Inject]
        public void Construct()
        {
            _uiController.AddItemUI(_type.ToString(), new ItemUI(_button));
        }
    }
}