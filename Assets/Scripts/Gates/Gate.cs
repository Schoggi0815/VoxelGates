using System;
using System.Collections.Generic;
using System.Linq;
using Line;
using UnityEngine;
using UnityEngine.UI;

namespace Gates
{
	public class Gate : Movable
	{
		public SpriteRenderer spriteRenderer;
		
		public List<KnobSmall> inputKnobs;
		public List<KnobSmall> outputKnobs;

		public string gateName;

		public Text text;

		public virtual void HandleChange() { }

		private int _height;
		public Color color;

		private int _width;

		private void CalculateObjectSize()
		{
			_width = Mathf.CeilToInt(gateName.Length * 3.5f + 6);

			var maxKnobLength = Math.Max(inputKnobs.Count, outputKnobs.Count);

			_height = maxKnobLength * 6 + 3;
		}

		private void SetKnobPositions()
		{
			var maxKnobLength = Math.Max(inputKnobs.Count, outputKnobs.Count);

			for (var i = 0; i < inputKnobs.Count; i++)
			{
				var inputKnob = inputKnobs[i];
				inputKnob.OffsetFromGate = new GridPosition(_width / -2 - 1, i * -6 - (maxKnobLength - 1) * -3);
				inputKnob.SetPosition(GridPosition);
				inputKnob.transform.position = GridToWorldPos(inputKnob.GridPosition);
			}

			for (var i = 0; i < outputKnobs.Count; i++)
			{
				var outputKnob = outputKnobs[i];
				outputKnob.OffsetFromGate = new GridPosition(_width / 2 + 1, i * 6 - (maxKnobLength - 1) * -3);
				outputKnob.SetPosition(GridPosition);
				outputKnob.transform.position = GridToWorldPos(outputKnob.GridPosition);
			}
		}

		protected override void Start()
		{
			base.Start();
			
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

			spriteRenderer.sprite = SpriteGenerator.CreateSprite(_width, _height);
			spriteRenderer.color = color;

			GetComponent<BoxCollider2D>().size = new Vector2(_width / 10f, _height / 10f);

			HandleChange();
		}

		public override void Delete()
		{
			base.Delete();
			
			Constants.C.TryRemoveFromQueue(this);

			for (var i = 0; i < inputKnobs.Count; i++)
			{
				inputKnobs[i].Delete();
			}

			for (var i = 0; i < outputKnobs.Count; i++)
			{
				outputKnobs[i].Delete();
			}
			
			Destroy(gameObject);
		}

		public override GridPosition GridPositionCalculation(GridPosition gridPosition)
		{
			var newGridPosition = gridPosition;

			var hasBothDirections = false;
			var hasSetFirst = false;
			var lineDirection = LineDirection.Horizontal;

			var knobSmalls = new List<KnobSmall>(inputKnobs);
			knobSmalls.AddRange(outputKnobs);

			var lineDrawerLines = new List<Line.Line>(knobSmalls.SelectMany(x => x.lines));


			foreach (var knobSmall in knobSmalls)
			{
				if (knobSmall.parentLine != null)
				{
					lineDrawerLines.Add(knobSmall.parentLine);
				}
			}
			
			foreach (var drawerLine in lineDrawerLines)
			{
				if (!hasSetFirst)
				{
					hasSetFirst = true;
					lineDirection = drawerLine.lineDirection;
					continue;
				}

				if (drawerLine.lineDirection != lineDirection)
				{
					hasBothDirections = true;
					break;
				}
			}

			if (!hasSetFirst)
			{
				CallOnMove(knobSmalls, newGridPosition);
				return newGridPosition;
			}
			
			if (hasBothDirections)
			{
				newGridPosition = GridPosition;
				CallOnMove(knobSmalls, newGridPosition);
				return newGridPosition;
			}

			newGridPosition = lineDirection == LineDirection.Horizontal ? new GridPosition(GridPosition.X, newGridPosition.Y) : new GridPosition(newGridPosition.X, GridPosition.Y);
			
			
			CallOnMove(knobSmalls, newGridPosition);
			return newGridPosition;
		}

		private void CallOnMove(List<KnobSmall> knobSmalls, GridPosition newPosition)
		{
			foreach (var knobSmall in knobSmalls)
			{
				knobSmall.OnMove(new GridPosition(newPosition.X + knobSmall.OffsetFromGate.X, newPosition.Y + knobSmall.OffsetFromGate.Y));
				knobSmall.SetPosition(newPosition);
			}
		}
	}
}