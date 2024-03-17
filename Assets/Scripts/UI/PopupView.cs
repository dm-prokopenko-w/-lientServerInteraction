using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static Core.Constants;

namespace Game.UI
{
	public abstract class PopupView : MonoBehaviour
	{
		[Inject] protected UIController _uiController;

		[SerializeField] protected Button _buttonOk;
		[SerializeField] protected Button _buttonCancel;
		[SerializeField] protected Animator _anim;

		protected UIType _type;

		[Inject]
		public virtual void Construct()
		{
			_uiController.AddItemUI(BtnOkPopup + _type, new ItemUI(_buttonOk));
			_uiController.AddItemUI(BtnClosePopup + _type, new ItemUI(_buttonCancel));
			_uiController.AddItemUI(PopupAnim + _type, new ItemUI(_anim));
		}
	}
}
