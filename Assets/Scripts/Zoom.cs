using UnityEngine;

public class Zoom : MonoBehaviour
{
	private Camera _camera;

	public float sensitivity;

	private float _size;

	private void Start()
	{
		_camera = GetComponent<Camera>();
		_size = _camera.orthographicSize;
	}

	private void Update()
	{
		_size -= Input.mouseScrollDelta.y * sensitivity * _camera.orthographicSize;

		var clamp = Mathf.Clamp(_size, 3, 10);
		_camera.orthographicSize = clamp;
		_size = clamp;
	}
}