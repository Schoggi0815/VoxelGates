using System.Collections.Generic;
using Line;
using UnityEngine;

public class Constants : MonoBehaviour
{
	public static Constants C;
	
	public List<Knob> knobs = new List<Knob>();
	public List<LineDrawer> lineDrawers = new List<LineDrawer>();

	public Sprite knobSpriteOn;
	public Sprite knobSpriteOff;
	public Sprite knobSpriteSelected;

	public GameObject inputKnobPrefab;
	public GameObject outputKnobPrefab;
	public GameObject linePrefab;
	
	public Vector2 knobSpawnOffset;

	public Transform knobParent;
	public Transform lineParent;

	public Color lineActiveColor;
	public Color lineInactiveColor;
	public Color lineHoverColor;

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