using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Line
{
	public class LineDrawer : MonoBehaviour
	{
		public GridObject GridObject { get; set; }

		[HideInInspector] public Line parentLine;
		[HideInInspector] public List<Line> lines = new List<Line>();
		private bool _isSelected;
		[SerializeField] protected LineDrawerMode lineDrawerMode;

		[SerializeField] private bool updateSpriteColor;


		public bool IsActive
		{
			get => _isActive;
			set
			{
				_isActive = value;

				SetSpriteColor();

				foreach (var line in lines)
				{
					line.IsActive = value;
				}
			}
		}

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;

				SetSpriteColor();
			}
		}

		private void Start()
		{
			Constants.C.lineDrawers.Add(this);
			GridObject = GetComponent<Movable>();
			StartAddon();
		}

		protected virtual void StartAddon()
		{
		}

		private bool _isFirstFrame;
		private bool _isActive;

		private void Update()
		{
			if (IsSelected)
			{
				var gridPosition = GetSnappingPos(out var lineDirection);
				Constants.C.selectionDrawer.GridPosition = gridPosition;
				Constants.C.selectionDrawer.transform.position += new Vector3(0, 0, .5f);
				var line = lines.Last();
				line.GridPosition1 = GridObject.GridPosition;
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
					lineDrawer.lines.Add(Line.Create(GridObject.GridPosition, GridObject.GridPosition, IsActive, lineDrawer, lineDrawer, LineDirection.Horizontal));
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

			var distanceX = Math.Abs(worldToGridPos.X - GridObject.GridPosition.X);
			var distanceY = Math.Abs(worldToGridPos.Y - GridObject.GridPosition.Y);

			if (distanceY < distanceX)
			{
				worldToGridPos = new GridPosition(worldToGridPos.X, GridObject.GridPosition.Y);
				lineDirection = LineDirection.Vertical;
			}
			else
			{
				worldToGridPos = new GridPosition(GridObject.GridPosition.X, worldToGridPos.Y);
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

		private void OnMouseOver()
		{
			if (lineDrawerMode != LineDrawerMode.Input && Input.GetMouseButtonDown(1) && !Constants.C.lineDrawers.Any(x => x.IsSelected))
			{
				_isFirstFrame = true;
				IsSelected = true;
				Constants.C.selectionDrawer.gameObject.SetActive(true);
				Constants.C.selectionDrawer.GetComponent<SpriteRenderer>().color = IsActive ? Constants.C.lineActiveColor : Constants.C.lineInactiveColor;

				lines.Add(Line.Create(GridObject.GridPosition, GridObject.GridPosition, IsActive, this, this, LineDirection.Horizontal));
			}

			if (Input.GetMouseButtonDown(0) && lineDrawerMode == LineDrawerMode.Input && !IsSelected && Constants.C.lineDrawers.Any(x => x.IsSelected))
			{
				var first = Constants.C.lineDrawers.First(x => x.IsSelected);

				if (first.GridObject.GridPosition.X == GridObject.GridPosition.X || first.GridObject.GridPosition.Y == GridObject.GridPosition.Y)
				{
					first.IsSelected = false;

					var line = first.lines.Last();

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
				int count = lines.Count;
				
				for (int i = 0; i < count; i++)
				{
					lines.First().Delete();
				}

				Constants.C.lineDrawers.Remove(this);
				Destroy(gameObject);
			}
			else
			{
				IsActive = false;
			}
		}

		protected virtual void SpriteColorUpdate()
		{
		}
	}
}