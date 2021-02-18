using System.Linq;
using Line;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gates
{
	public class KnobSmall : LineDrawer
	{
		private SpriteRenderer _spriteRenderer;

		public GridPosition OffsetFromGate;

		[HideInInspector]
		public Gate gate;

		public void SetPosition(GridPosition newPosition)
		{
			GridObject.GridPosition = new GridPosition(newPosition.X + OffsetFromGate.X, newPosition.Y + OffsetFromGate.Y);
		}

		protected override void StartAddon()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		protected override void SpriteColorUpdate()
		{
			_spriteRenderer.sprite = IsSelected ? Constants.C.smallKnobSpriteSelected : IsActive ? Constants.C.smallKnobSpriteOn : Constants.C.smallKnobSpriteOff;
		}

		protected override void ChangeActive()
		{
			if (lineDrawerMode == LineDrawerMode.Input)
			{
				Constants.C.AddGateToQueue(gate);
			}
		}
	}
}