using System.Collections.Generic;
using Line;
using UnityEngine;

public class Constants : MonoBehaviour
{
	public static Constants C;
	
	public List<LineDrawer> lineDrawers = new List<LineDrawer>();

	public Sprite knobSpriteOn;
	public Sprite knobSpriteOff;
	public Sprite knobSpriteSelected;

	public GameObject inputKnobPrefab;
	public GameObject outputKnobPrefab;
	public GameObject linePrefab;
	public GameObject lineCornerPrefab;
	
	public Vector2 knobSpawnOffset;

	public Transform knobParent;
	public Transform lineParent;
	public Transform lineCornerParent;

	public Color lineActiveColor;
	public Color lineInactiveColor;
	public Color lineHoverColor;

	public GridObject selectionDrawer;

	private void Awake()
	{
		C = this;
	}

	public static Vector3 CursorInWorldPos()
	{
		if (Camera.main == null)
		{
			return new Vector3();
		}

		var screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return new Vector3(screenToWorldPoint.x, screenToWorldPoint.y);
	}

	public GameObject SpawnPrefab(GameObject prefab)
	{
		return Instantiate(prefab);
	}
}