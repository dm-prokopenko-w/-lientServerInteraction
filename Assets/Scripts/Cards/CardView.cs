using UnityEngine;

namespace CardsSystem
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] private Animator _anim;
		[SerializeField] private GameObject _effect;

		public CardItem Item { get; private set; }
		private bool _isAnimated;
		
		public void Init(CardItem item)
		{
			Item = item;
			_anim.enabled = item.isAnimated;
			
			ActiveEffect(_isAnimated);
		}

		public void ActiveEffect(bool value)
		{
			if (Item.isAnimated)
			{
				_effect.gameObject.SetActive(value);
			}
		}
	}
}
