using System;
using System.Collections.Generic;
using System.Linq;
using SaveObjects;
using UnityEngine;

namespace Line
{
	public class LineDrawer : Movable
	{
		[HideInInspector] public LineDrawerSave lineDrawerSave;

		[HideInInspector] public Line parentLine;
		[HideInInspector] public List<Line> lines = new List<Line>();
		private bool _isSelected;
		public LineDrawerMode lineDrawerMode;

		[SerializeField] private bool updateSpriteColor;


		public bool IsActive
		{
			get => _isActive;
			set
			{
				_isActive = value;

				SetSpriteColor();
				
				ChangeActive();

				foreach (var line in lines)
				{
					line.IsActive = value;
				}
			}
		}

		protected virtual void ChangeActive() { }

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;

				SetSpriteColor();
			}
		}

		protected override void Awake()
		{
			base.Awake();
			
			StartAddon();
		}

		protected override void Start()
		{
			base.Start();
			
			Constants.C.lineDrawers.Add(this);
		}

		public override GridObjectSave ToSaveObject()
		{
			lineDrawerSave = new LineDrawerSave(GridPosition, lineDrawerMode, IsActive);
			return lineDrawerSave;
		}

		protected virtual void StartAddon()
		{
		}

		private bool _isFirstFrame;
		private bool _isActive;

		protected override void Update()
		{
			base.Update();
			
			if (IsSelected)
			{
				var gridPosition = GetSnappingPos(out var lineDirection);
				Constants.C.selectionDrawer.GridPosition = gridPosition;
				Constants.C.selectionDrawer.transform.position += new Vector3(0, 0, .5f);
				var line = lines.Last();
				line.GridPosition1 = GridPosition;
				line.GridPosition2 = gridPosition;

				line.lineDirection = lineDirection;

				if (Input.GetMouseButtonDown(0))
				{
					IsSelected = false;
					var instantiate = Instantiate(Constants.C.lineCornerPrefab, Constants.C.lineCornerParent, true);
					instantiate.GetComponent<GridObject>().GridPosition = gridPosition;
					var lineDrawer = instantiate.GetComponent<LineDrawer>();
					lineDrawer.IsActive = IsActive;
					lineDrawer.parentLine = line;
					line.SetParents(this, lineDrawer);
					lineDrawer.IsSelected = true;
					Constants.C.selectionDrawer.GetComponent<SpriteRenderer>().color = lineDrawer.IsActive ? Constants.C.lineActiveColor : Constants.C.lineInactiveColor;
					lineDrawer.lines.Add(Line.Create(GridPosition, GridPosition, IsActive, lineDrawer, lineDrawer, LineDirection.Horizontal));
				}

				if (Input.GetMouseButtonDown(1) && !_isFirstFrame)
				{
					IsSelected = false;
					line.Delete(false);
					Constants.C.selectionDrawer.gameObject.SetActive(false);
				}

				_isFirstFrame = false;
			}
		}

		private GridPosition GetSnappingPos(out LineDirection lineDirection)
		{
			var worldToGridPos = GridObject.WorldToGridPos(Constants.CursorInWorldPos());

			var distanceX = Math.Abs(worldToGridPos.X - GridPosition.X);
			var distanceY = Math.Abs(worldToGridPos.Y - GridPosition.Y);

			if (distanceY < distanceX)
			{
				worldToGridPos = new GridPosition(worldToGridPos.X, GridPosition.Y);
				lineDirection = LineDirection.Vertical;
			}
			else
			{
				worldToGridPos = new GridPosition(GridPosition.X, worldToGridPos.Y);
				lineDirection = LineDirection.Horizontal;
			}

			return worldToGridPos;
		}

		public void OnMove(GridPosition gridPosition)
		{
			foreach (var line in lines)
			{
				line.GridPosition1 = gridPosition;
			}

			if (parentLine != null)
			{
				parentLine.GridPosition2 = gridPosition;
			}
		}

		protected override void OnMouseOver()
		{
			base.OnMouseOver();
			
			if (lineDrawerMode != LineDrawerMode.Input && Input.GetMouseButtonDown(1) && !Constants.C.lineDrawers.Any(x => x.IsSelected))
			{
				_isFirstFrame = true;
				IsSelected = true;
				Constants.C.selectionDrawer.gameObject.SetActive(true);
				Constants.C.selectionDrawer.GetComponent<SpriteRenderer>().color = IsActive ? Constants.C.lineActiveColor : Constants.C.lineInactiveColor;

				lines.Add(Line.Create(GridPosition, GridPosition, IsActive, this, this, LineDirection.Horizontal));
			}

			if (Input.GetMouseButtonDown(0) && lineDrawerMode == LineDrawerMode.Input && !IsSelected && Constants.C.lineDrawers.Any(x => x.IsSelected))
			{
				var first = Constants.C.lineDrawers.First(x => x.IsSelected);

				if (first.GridPosition.X == GridPosition.X || first.GridPosition.Y == GridPosition.Y)
				{
					first.IsSelected = false;

					var line = first.lines.Last();

					if (parentLine != null)
					{
						parentLine.Delete();
					}
					
					IsActive = first.IsActive;
					parentLine = line;
					line.SetParents(first, this);
					Constants.C.selectionDrawer.gameObject.SetActive(false);
				}
			}

			OnMouseOverAddon();
		}

		protected virtual void OnMouseOverAddon()
		{
		}

		private void SetSpriteColor()
		{
			if (updateSpriteColor)
			{
				Color color;

				var spriteRenderer = GetComponent<SpriteRenderer>();

				if (IsSelected)
				{
					color = Constants.C.lineHoverColor;
				}
				else if (IsActive)
				{
					color = Constants.C.lineActiveColor;
				}
				else
				{
					color = Constants.C.lineInactiveColor;
				}

				spriteRenderer.color = color;
			}

			SpriteColorUpdate();
		}

		public void CascadeDelete()
		{
			if (lineDrawerMode == LineDrawerMode.Adaptive)
			{
				Delete();
			}
			else
			{
				IsActive = false;
			}
		}

		protected virtual void SpriteColorUpdate()
		{
		}

		public override void Delete()
		{
			base.Delete();
			
			if (parentLine != null)
			{
				parentLine.Delete(false);
			}

			int count = lines.Count;
			
			for (var i = 0; i < count; i++)
			{
				lines.First().Delete();
			}

			Constants.C.lineDrawers.Remove(this);

			if (GetType() == typeof(Knob))
			{
				var knob = (Knob) this;
				
				knob.OnDelete();
			}
			
			Destroy(gameObject);
		}

		public override GridPosition GridPositionCalculation(GridPosition gridPosition)
		{
			var newGridPosition = gridPosition;

			var hasBothDirections = false;
			var hasSetFirst = false;
			var lineDirection = LineDirection.Horizontal;

			var lineDrawerLines = new List<Line>(lines);
			
			if (parentLine != null)
			{
				lineDrawerLines.Add(parentLine);
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
				OnMove(newGridPosition);
				return newGridPosition;
			}
			
			if (hasBothDirections)
			{
				newGridPosition = GridPosition;
				OnMove(newGridPosition);
				return newGridPosition;
			}

			newGridPosition = lineDirection == LineDirection.Horizontal ? new GridPosition(GridPosition.X, newGridPosition.Y) : new GridPosition(newGridPosition.X, GridPosition.Y);
			
			
			OnMove(newGridPosition);
			return newGridPosition;
		}
	}
}