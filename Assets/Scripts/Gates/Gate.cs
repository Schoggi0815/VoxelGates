using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gates
{
	public class Gate : MonoBehaviour
	{
		public List<KnobSmall> inputKnobs;
		public List<KnobSmall> outputKnobs;

		public string gateName;

		public Text text;

		public BoxCollider2D boxCollider2D;

		public virtual void HandleChange() { }

		private int _height;
		public Color color;

		private int _width;

		private Vector3 _offset;

		public GridObject gridObject;

		private void CalculateObjectSize()
		{
			_width = Mathf.CeilToInt(gateName.Length * 3.5f + 6);

			var maxKnobLength = Math.Max(inputKnobs.Count, outputKnobs.Count);

			_height = maxKnobLength * 6 + 3;

			_offset = new Vector3(0, -.3f * (maxKnobLength - 1));
		}

		private void SetKnobPositions()
		{
			for (var i = 0; i < inputKnobs.Count; i++)
			{
				var inputKnob = inputKnobs[i];
				inputKnob.OffsetFromGate = new GridPosition(_width / -2 - 1, i * -6);
				inputKnob.SetPosition(gridObject.GridPosition);
				inputKnob.transform.position = GridObject.GridToWorldPos(inputKnob.GridObject.GridPosition);
			}

			for (var i = 0; i < outputKnobs.Count; i++)
			{
				var outputKnob = outputKnobs[i];
				outputKnob.OffsetFromGate = new GridPosition(_width / 2 + 1, i * 6);
				outputKnob.SetPosition(gridObject.GridPosition);
				outputKnob.transform.position = GridObject.GridToWorldPos(outputKnob.GridObject.GridPosition);
			}
		}

		private void Start()
		{
			foreach (var inputKnob in inputKnobs)
			{
				inputKnob.gate = this;
			}
			
			foreach (var outputKnob in outputKnobs)
			{
				outputKnob.gate = this;
			}
			
			text.text = gateName;

			CalculateObjectSize();

			SetKnobPositions();

			transform.position += _offset;

			var spriteRenderer = GetComponent<SpriteRenderer>();

			spriteRenderer.sprite = SpriteGenerator.CreateSprite(_width, _height);
			spriteRenderer.color = color;

			boxCollider2D.size = new Vector2(_width / 10f, _height / 10f);

			HandleChange();
		}
	}
}