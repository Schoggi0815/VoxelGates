using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundGenerator : MonoBehaviour
{
    // References to scene objects
    [SerializeField] private Camera mainCamera;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;

    private Vector2 _spriteSize;
    
    private float _cameraSizeStart;
    private bool _mouseIsOn;
    private Vector3 _startMousePos;
    private bool _mouseWasOn;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();

        _spriteSize = _spriteRenderer.size;
        _cameraSizeStart = mainCamera.orthographicSize;
    }

    private void Update()
    {
        var size = _spriteSize / _cameraSizeStart * mainCamera.orthographicSize;
        var spriteLength = 8.6f;
        size = new Vector2(size.x - size.x % spriteLength + spriteLength, size.y - size.y % spriteLength + spriteLength);
        _spriteRenderer.size = size;
        _boxCollider.size = size;
        
        var transformPosition = mainCamera.transform.position;
        transform.position = new Vector3(transformPosition.x - transformPosition.x % 4.3f + .05f, transformPosition.y - transformPosition.y % 4.3f + .05f, 2);
        
        if (!_mouseIsOn) return;

        if (Input.GetMouseButtonDown(2) && !EventSystem.current.IsPointerOverGameObject())
        {
            _startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startMousePos.z = 0.0f;
            _mouseWasOn = true;
        }

        if (Input.GetMouseButton(2) && _mouseWasOn && !EventSystem.current.IsPointerOverGameObject())
        {
            var nowMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nowMousePos.z = 0.0f;
            mainCamera.transform.position += _startMousePos - nowMousePos;
        }
        else
        {
            _mouseWasOn = false;
        }
    }

    private void OnMouseEnter()
    {
        _mouseIsOn = true;
    }

    private void OnMouseExit()
    {
        _mouseIsOn = false;
    }
}