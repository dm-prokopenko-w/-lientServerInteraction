using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardsSystem
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] private Animator _anim;
		[SerializeField] private GameObject _effect;

		public void Init(bool isAnim)
		{
			_anim.enabled = isAnim;
			_effect.gameObject.SetActive(isAnim);
		}
	}
}
