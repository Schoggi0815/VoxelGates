using System;
using System.Collections.Generic;
using UnityEngine;

namespace Line
{
    public class Line : MonoBehaviour
    {
        private static Vector2 _spriteSize = new Vector2(.1f, .3f);

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider2D;
        private GridPosition _gridPosition1;
        private GridPosition _gridPosition2;
        private bool _isActive;

        private Knob _knobParent;
        private Knob _knobChild;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;

                _spriteRenderer.color = value ? Constants.C.lineActiveColor : Constants.C.lineInactiveColor;
                _knobChild.IsActive = value;
            }
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

        public void Delete()
        {
            _knobParent.Lines.Remove(this);
            _knobChild.Lines.Remove(this);
            Destroy(gameObject);
        }

        private void OnMouseEnter()
        {
            _spriteRenderer.color = Constants.C.lineHoverColor;
        }

        private void OnMouseExit()
        {
            _spriteRenderer.color = IsActive ? Constants.C.lineActiveColor : Constants.C.lineInactiveColor;
        }

        private void OnMouseDown()
        {
            Delete();
        }

        private void HandleLineChange()
        {
            if (_gridPosition1.X == _gridPosition2.X)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);

                var distance = Mathf.Abs(_gridPosition1.Y - _gridPosition2.Y);
                
                var size = new Vector2(distance, 1) * _spriteSize;
                
                _spriteRenderer.size = size;
                _boxCollider2D.size = size;
                
                transform.position = new Vector3(GridObject.GridToWorldPos(_gridPosition1).x, (GridObject.GridToWorldPos(_gridPosition1).y + GridObject.GridToWorldPos(_gridPosition2).y) / 2, 1);
            }
            else if (_gridPosition1.Y == _gridPosition2.Y)
            {
                transform.rotation = new Quaternion();
                
                var distance = Mathf.Abs(_gridPosition1.X - _gridPosition2.X);

                var size = new Vector2(distance, 1) * _spriteSize;
                
                _spriteRenderer.size = size;
                _boxCollider2D.size = size;
                
                transform.position = new Vector3((GridObject.GridToWorldPos(_gridPosition1).x + GridObject.GridToWorldPos(_gridPosition2).x) / 2, GridObject.GridToWorldPos(_gridPosition1).y, 1);
            }
        }

        public static Line Create(GridPosition from, GridPosition to, bool isActive, Knob knobParent, Knob knobChild)
        {
            var gameObject = Instantiate(Constants.C.linePrefab, Constants.C.knobParent, true);

            var line = gameObject.GetComponent<Line>();

            line._gridPosition1 = from;
            line._gridPosition2 = to;
            
            line.HandleLineChange();
            
            line._knobParent = knobParent;
            line._knobChild = knobChild;

            line.IsActive = isActive;

            return line;
        }
    }
}
