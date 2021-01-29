using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movable : GridObject
{ 
    private static readonly List<Movable> AllMovables = new List<Movable>();

    private bool _isGettingMoved;
    public bool CanBeMovedByPlayer { get; set; }

    private const float Snapper = 10;

    private void Start()
    {
        AllMovables.Add(this);
    }
    
    private void Update()
    {
        if (!_isGettingMoved) return;

        if (Input.GetMouseButton(2))
        {
            var cursorInWorldPos = Constants.CursorInWorldPos();
            GridPosition = GridObject.WorldToGridPos(cursorInWorldPos);
        }
        else
        {
            _isGettingMoved = false;
        }
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
