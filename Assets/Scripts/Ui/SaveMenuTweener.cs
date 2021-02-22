using System;
using Platinio.UI;
using UnityEngine;

namespace Ui
{
	public class SaveMenuTweener : MonoBehaviour
	{
		public RectTransform canvas;

		public GameObject child;
		
		private RectTransform _rectTransform;
		
		private void Start()
		{
			_rectTransform = GetComponent<RectTransform>();
			
			MoveDown();
		}

		public void MoveDown()
		{
			_rectTransform.MoveUI(new Vector2(.5f, -.5f), canvas, 1).SetOnComplete(() =>
			{
				child.SetActive(false);
				Constants.C.saveMenuResponseText.text = string.Empty;
			});
		}

		public void MoveUp()
		{
			child.SetActive(true);
			
			_rectTransform.MoveUI(new Vector2(.5f, .5f), canvas, 1);
		}
	}
}
