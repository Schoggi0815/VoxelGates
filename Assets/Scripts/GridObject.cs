using System;
using SaveObjects;
using UnityEngine;

public class GridObject : MonoBehaviour
{
	private GridPosition _gridPosition;
	
	[SerializeField] private bool createSave = true;

	public Vector3 offset;

	[SerializeField]
	private bool updatePosition = true;

	public GridPosition GridPosition
	{
		get => _gridPosition;
		set
		{
			_gridPosition = value;
			if (updatePosition)
			{
				transform.position = GridToWorldPos(_gridPosition) + offset;
			}
		}
	}

	public virtual void Delete()
	{
		Constants.C.saveHandler.gridObjects.Remove(this);
	}

	protected virtual void Start()
	{
		_gridPosition = WorldToGridPos(transform.localPosition);

		if (createSave)
		{
			Constants.C.saveHandler.gridObjects.Add(this);
		}
	}

	protected virtual void Awake() { }

	protected virtual void Update() { }

	protected virtual void OnMouseOver() { }

	public virtual GridObjectSave ToSaveObject()
	{
		return null;
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