using UnityEngine;
using UnityEngine.EventSystems;

public class Movable : GridObject
{ 
    private bool _isGettingMoved;

    public bool canBeMoved = true;

    protected override void Update()
    {
        base.Update();
        
        if (!_isGettingMoved) return;
        
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Delete();
            Destroy(gameObject);
        }

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

    protected override void OnMouseOver()
    {
        base.OnMouseOver();
        
        if (Input.GetMouseButtonDown(2) && !EventSystem.current.IsPointerOverGameObject() && canBeMoved)
        {
            _isGettingMoved = true;
        }
    }
}
