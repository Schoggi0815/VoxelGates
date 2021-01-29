using UnityEngine;

public class GridObject : MonoBehaviour
{
	private GridPosition _gridPosition = new GridPosition();

	public GridPosition GridPosition
	{
		get => _gridPosition;
		set
		{
			_gridPosition = value;
			transform.position = GridToWorldPos(_gridPosition);
		}
	}

	public static Vector3 GridToWorldPos(GridPosition gridPosition)
	{
		return new Vector3((float)gridPosition.X / 10, (float)gridPosition.Y / 10);
	}

	public static GridPosition WorldToGridPos(Vector3 worldPosition)
	{
		return new GridPosition(Mathf.RoundToInt(worldPosition.x * 10), Mathf.RoundToInt(worldPosition.y * 10));
	}
}