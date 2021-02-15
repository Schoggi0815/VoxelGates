using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movable : GridObject
{ 
    private bool _isGettingMoved;

    private const float Snapper = 10;

    private void Update()
    {
        if (!_isGettingMoved) return;

        if (Input.GetMouseButton(2))
        {
            var cursorInWorldPos = Constants.CursorInWorldPos();
            GridPosition = GridPositionCalculation(WorldToGridPos(cursorInWorldPos));
        }
        else
        {
            _isGettingMoved = false;
        }
    }

    public virtual GridPosition GridPositionCalculation(GridPosition gridPosition)
    {
        return gridPosition;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(2) && !EventSystem.current.IsPointerOverGameObject())
        {
            _isGettingMoved = true;
        }
    }

    private static Vector3 GridToWorldPos(Vector2 gridPos)
    {
        return gridPos / Snapper;
    }
}
