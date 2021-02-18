using System.Collections.Generic;
using System.Linq;
using Line;
using UnityEngine;

namespace Gates
{
	public class MovableGate : Movable
	{
		[SerializeField]private Gate gate;

		public override GridPosition GridPositionCalculation(GridPosition gridPosition)
		{
			var newGridPosition = gridPosition;

			bool hasBothDirections = false;
			bool hasSetFirst = false;
			LineDirection lineDirection = LineDirection.Horizontal;

			var knobSmalls = new List<KnobSmall>(gate.inputKnobs);
			knobSmalls.AddRange(gate.outputKnobs);

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
				newGridPosition = gate.gridObject.GridPosition;
				CallOnMove(knobSmalls, newGridPosition);
				return newGridPosition;
			}

			newGridPosition = lineDirection == LineDirection.Horizontal ? new GridPosition(gate.gridObject.GridPosition.X, newGridPosition.Y) : new GridPosition(newGridPosition.X, gate.gridObject.GridPosition.Y);
			
			
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