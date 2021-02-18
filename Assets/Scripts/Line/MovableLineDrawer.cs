using System;
using System.Collections.Generic;
using System.Linq;

namespace Line
{
	public class MovableLineDrawer : Movable
	{
		private LineDrawer _lineDrawer;

		protected override void StartAddon()
		{
			_lineDrawer = GetComponent<LineDrawer>();
		}

		public override GridPosition GridPositionCalculation(GridPosition gridPosition)
		{
			var newGridPosition = gridPosition;

			bool hasBothDirections = false;
			bool hasSetFirst = false;
			LineDirection lineDirection = LineDirection.Horizontal;

			var lineDrawerLines = new List<Line>(_lineDrawer.lines);
			
			if (_lineDrawer.parentLine != null)
			{
				lineDrawerLines.Add(_lineDrawer.parentLine);
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
				_lineDrawer.OnMove(newGridPosition);
				return newGridPosition;
			}
			
			if (hasBothDirections)
			{
				newGridPosition = _lineDrawer.GridObject.GridPosition;
				_lineDrawer.OnMove(newGridPosition);
				return newGridPosition;
			}

			newGridPosition = lineDirection == LineDirection.Horizontal ? new GridPosition(_lineDrawer.GridObject.GridPosition.X, newGridPosition.Y) : new GridPosition(newGridPosition.X, _lineDrawer.GridObject.GridPosition.Y);
			
			
			_lineDrawer.OnMove(newGridPosition);
			return newGridPosition;
		}
	}
}