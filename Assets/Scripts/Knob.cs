using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Knob : MonoBehaviour
{
	[SerializeField] private bool isActive;

	[SerializeField] private KnobType knobType;

	private List<Knob> _connectedKnobs;
	public List<Line.Line> Lines { get; set; }

	public bool IsSelected
	{
		get => _isSelected;
		set
		{
			_isSelected = value;
			
			SetSprite();
		}
	}

	public bool IsActive
	{
		get => isActive;
		set
		{
			isActive = value;

			SetSprite();

			if (knobType == KnobType.Output)
			{
				foreach (var line in Lines)
				{
					line.IsActive = value;
				}
			}
		}
	}

	private SpriteRenderer _spriteRenderer;
	private bool _isSelected;

	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		Constants.C.knobs.Add(this);
		_connectedKnobs = knobType == KnobType.Output ? new List<Knob>() : new List<Knob>(1);
		Lines = knobType == KnobType.Output ? new List<Line.Line>() : new List<Line.Line>(1);
	}

	private void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && knobType == KnobType.Output)
		{
			IsActive = !IsActive;
		}

		if (Input.GetMouseButtonDown(1))
		{
			IsSelected = !IsSelected;

			if (IsSelected)
			{
				if (Constants.C.knobs.Any(x => x != this && x.IsSelected))
				{
					var knob = Constants.C.knobs.Single(x => x != this && x.IsSelected);

					if (TryGetDifferentKnobTypes(this, knob, out var knobOutput, out var knobInput))
					{
						var outputGridPos = GridObject.WorldToGridPos(knobOutput.transform.position);
						var inputGridPos = GridObject.WorldToGridPos(knobInput.transform.position);

						if (outputGridPos.X == inputGridPos.X || outputGridPos.Y == inputGridPos.Y)
						{
							var line = Line.Line.Create(outputGridPos, inputGridPos, IsActive, knobOutput, knobInput);

							knobOutput.Lines.Add(line);
							if (knobInput.Lines.Count > 0)
							{
								knobInput.Lines.First().Delete();
							}
							knobInput.Lines.Add(line);
						}
					}
					
					knob.IsSelected = false;
					IsSelected = false;
				}
			}
		}
	}

	private void SetSprite()
	{
		_spriteRenderer.sprite = IsSelected ? Constants.C.knobSpriteSelected : IsActive ? Constants.C.knobSpriteOn : Constants.C.knobSpriteOff;
	}

	private static bool TryGetDifferentKnobTypes(Knob knob1, Knob knob2, out Knob knobOutput, out Knob knobInput)
	{
		if (knob1.knobType == knob2.knobType)
		{
			knobOutput = null;
			knobInput = null;
			return false;
		}

		if (knob1.knobType == KnobType.Output)
		{
			knobOutput = knob1;
			knobInput = knob2;
			return true;
		}

		knobOutput = knob2;
		knobInput = knob1;
		return true;
	}
}