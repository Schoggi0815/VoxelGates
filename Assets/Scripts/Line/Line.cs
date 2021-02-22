using System;
using SaveObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Line
{
    public class Line : MonoBehaviour
    {
        private static readonly Vector2 SpriteSize = new Vector2(.1f, .3f);

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider2D;
        private GridPosition _gridPosition1;
        private GridPosition _gridPosition2;
        private bool _isActive;

        private LineDrawer _lineParent;
        private LineDrawer _lineChild;

        public LineDirection lineDirection;
        private bool _isCursorOver;

        private bool IsCursorOver
        {
            get => _isCursorOver;
            set
            {
                _isCursorOver = value;
                
                SetColor();
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;

                _spriteRenderer.color = value ? Constants.C.lineActiveColor : Constants.C.lineInactiveColor;
                if (_lineChild != null && _lineChild != _lineParent)
                {
                    _lineChild.IsActive = value;
                }
                
                SetColor();
            }
        }

        private void SetColor()
        {
            _spriteRenderer.color = IsCursorOver ? Constants.C.lineHoverColor : IsActive ? Constants.C.lineActiveColor : Constants.C.lineInactiveColor;
        }

        public GridPosition GridPosition1
        {
            get => _gridPosition1;
            set
            {
                _gridPosition1 = value;
                HandleLineChange();
            }
        }

        public GridPosition GridPosition2
        {
            get => _gridPosition2;
            set
            {
                _gridPosition2 = value;
                HandleLineChange();
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            Constants.C.saveHandler.lines.Add(this);
        }

        public LineSave ToLineSave()
        {
            return new LineSave(lineDirection, _lineParent.lineDrawerSave, _lineChild.lineDrawerSave);
        }

        public void Delete(bool cascadeDelete = true)
        {
            Constants.C.saveHandler.lines.Remove(this);
            _lineParent.lines.Remove(this);
            if (cascadeDelete)
            {
                _lineChild.parentLine = null;
                _lineChild.CascadeDelete();
            }
            Destroy(gameObject);
        }

        private void OnMouseEnter()
        {
            IsCursorOver = true;
        }

        private void OnMouseExit()
        {
            IsCursorOver = false;
        }

        private void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Delete();
            }
        }

        private void HandleLineChange()
        {
            if (_gridPosition1.X == _gridPosition2.X)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);

                var distance = Mathf.Abs(_gridPosition1.Y - _gridPosition2.Y);
                
                var size = new Vector2(distance, 1) * SpriteSize;
                
                _spriteRenderer.size = size;
                _boxCollider2D.size = size;
                
                transform.position = new Vector3(GridObject.GridToWorldPos(_gridPosition1).x, (GridObject.GridToWorldPos(_gridPosition1).y + GridObject.GridToWorldPos(_gridPosition2).y) / 2, 1);
            }
            else if (_gridPosition1.Y == _gridPosition2.Y)
            {
                transform.rotation = new Quaternion();
                
                var distance = Mathf.Abs(_gridPosition1.X - _gridPosition2.X);

                var size = new Vector2(distance, 1) * SpriteSize;
                
                _spriteRenderer.size = size;
                _boxCollider2D.size = size;
                
                transform.position = new Vector3((GridObject.GridToWorldPos(_gridPosition1).x + GridObject.GridToWorldPos(_gridPosition2).x) / 2, GridObject.GridToWorldPos(_gridPosition1).y, 1);
            }
        }

        public void SetParents(LineDrawer lineParent, LineDrawer lineChild)
        {
            _lineParent = lineParent;
            _lineChild = lineChild;
        }

        public static Line Create(GridPosition from, GridPosition to, bool isActive, LineDrawer lineParent, LineDrawer lineChild, LineDirection lineDirection)
        {
            var gameObject = Instantiate(Constants.C.linePrefab, Constants.C.lineParent, true);

            var line = gameObject.GetComponent<Line>();

            line.lineDirection = lineDirection;
            line._gridPosition1 = from;
            line._gridPosition2 = to;
            
            line.HandleLineChange();
            
            line._lineParent = lineParent;
            line._lineChild = lineChild;

            line.IsActive = isActive;

            return line;
        }
    }
}
