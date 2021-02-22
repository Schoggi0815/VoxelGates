using System.Collections.Generic;
using System.Linq;
using Gates;
using Line;
using NUnit.Framework;
using SaveObjects;
using Ui;
using UnityEngine;
using UnityEngine.UI;

public class Constants : MonoBehaviour
{
	public static Constants C;

	public SaveHandler saveHandler;

	public Text debugText;
	public Text saveMenuResponseText;

	public OverwritePopupUi overwritePopupUi;

	public Color saveMenuErrorColor;
	public Color saveMenuSuccessColor;
	
	public InputField saveNameText;

	public List<LineDrawer> lineDrawers = new List<LineDrawer>();
	
	public List<Knob> outputKnobs = new List<Knob>();
	public List<Knob> inputKnobs = new List<Knob>();

	public Sprite knobSpriteOn;
	public Sprite knobSpriteOff;
	public Sprite knobSpriteSelected;
	
	public Sprite smallKnobSpriteOn;
	public Sprite smallKnobSpriteOff;
	public Sprite smallKnobSpriteSelected;

	public GameObject inputKnobPrefab;
	public GameObject outputKnobPrefab;
	public GameObject linePrefab;
	public GameObject lineCornerPrefab;

	public GameObject andGatePrefab;
	public GameObject notGatePrefab;
	
	public Vector2 knobSpawnOffset;
	public Vector2 gateSpawnOffset;

	public Transform knobParent;
	public Transform lineParent;
	public Transform lineCornerParent;
	public Transform gateParent;

	public Color lineActiveColor;
	public Color lineInactiveColor;
	public Color lineHoverColor;

	public GridObject selectionDrawer;
	
	private readonly List<Gate> _gatesToUpdate = new List<Gate>();

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

	private void FixedUpdate()
	{
		if (_gatesToUpdate.Count > 0)
		{
			List<Gate> gates = new List<Gate>(_gatesToUpdate);
			
			_gatesToUpdate.Clear();

			foreach (var gate in gates)
			{
				gate.HandleChange();
			}
		}
	}

	public void AddGateToQueue(Gate gate)
	{
		if (!_gatesToUpdate.Contains(gate))
		{
			_gatesToUpdate.Add(gate);
		}
	}

	public bool TryRemoveFromQueue(Gate gate)
	{
		if (!_gatesToUpdate.Contains(gate)) return false;
		
		_gatesToUpdate.Remove(gate);
		return true;
	}

	public void UpdateKnobIDs()
	{
		for (var i = 0; i < outputKnobs.Count; i++)
		{
			outputKnobs[i].ID = i + 1;
			outputKnobs[i].OnUpdateID();
		}
		
		for (var i = 0; i < inputKnobs.Count; i++)
		{
			inputKnobs[i].ID = i + 1;
			inputKnobs[i].OnUpdateID();
		}
	}
}