using System;
using UnityEngine;

public class GridObject : MonoBehaviour
{
	private GridPosition _gridPosition;

	public Vector3 offset;

	public GridPosition GridPosition
	{
		get => _gridPosition;
		set
		{
			_gridPosition = value;
			transform.position = GridToWorldPos(_gridPosition) + offset;
		}
	}

	private void Start()
	{
		_gridPosition = WorldToGridPos(transform.position);
		
		StartAddon();
	}

	protected virtual void StartAddon() { }
	
	public static Vector3 GridToWorldPos(GridPosition gridPosition)
	{
		return new Vector3((float)gridPosition.X / 10, (float)gridPosition.Y / 10);
	}

	public static GridPosition WorldToGridPos(Vector3 worldPosition)
	{
		return new GridPosition(Mathf.RoundToInt(worldPosition.x * 10), Mathf.RoundToInt(worldPosition.y * 10));
	}
}