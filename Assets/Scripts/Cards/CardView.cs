using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardsSystem
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] private Animator _anim;
		[SerializeField] private GameObject _effect;

		public int Id => _id;
		private bool _isAnimated;
		private int _id;
		
		public void Init(int id, bool isAnim)
		{
			_id = id;
			_anim.enabled = isAnim;
			_isAnimated = isAnim;
			
			ActiveEffect(isAnim);
		}

		public void ActiveEffect(bool value)
		{
			if (_isAnimated)
			{
				_effect.gameObject.SetActive(value);
			}
		}
	}
}
